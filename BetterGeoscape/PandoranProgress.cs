using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class PandoranProgress
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Changes()
        {
            try
            {
                //Testing Less Pandoran Colonies on Legend
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                            
                veryhard.NestLimitations.MaxNumber = 3; //vanilla 6
                veryhard.NestLimitations.HoursBuildTime = 90; //vanilla 45
                veryhard.LairLimitations.MaxNumber = 3; // vanilla 5
                veryhard.LairLimitations.MaxConcurrent = 3; //vanilla 4
                veryhard.LairLimitations.HoursBuildTime = 100; //vanilla 50
                veryhard.CitadelLimitations.HoursBuildTime = 180; //vanilla 60
               
                //reducing evolution per day because there other sources of evolution points now
                veryhard.EvolutionProgressPerDay = 50; //vanilla 100

                //Remove faction diplo penalties for not destroying revealed PCs and increase rewards for haven leader
                GeoAlienBaseTypeDef nestType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Nest_GeoAlienBaseTypeDef"));              
                GeoAlienBaseTypeDef lairType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Lair_GeoAlienBaseTypeDef"));
                GeoAlienBaseTypeDef citadelType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Citadel_GeoAlienBaseTypeDef"));
                GeoAlienBaseTypeDef palaceType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Palace_GeoAlienBaseTypeDef"));

                nestType.FactionDiplomacyPenaltyPerHaven = 0; //vanilla -1
                nestType.HavenLeaderDiplomacyReward = 12; //vanilla 8 
                lairType.FactionDiplomacyPenaltyPerHaven = 0; //vanilla -1
                lairType.HavenLeaderDiplomacyReward = 16; //vanilla 12 
                citadelType.FactionDiplomacyPenaltyPerHaven = 0; //vanilla -1
                citadelType.HavenLeaderDiplomacyReward = 20; //vanilla 16 
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        [HarmonyPatch(typeof(GeoscapeEventSystem), "PhoenixFaction_OnSiteFirstTimeVisited")]
        public static class GeoscapeEventSystem_PhoenixFaction_OnSiteFirstTimeVisited_Patch
        {
            public static void Postfix(GeoSite site, GeoLevelController ____level)
            {
                try
                {
                    if (site != null)
                    {
                        ____level.AlienFaction.AddEvolutionProgress(10);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        internal static class BC_GeoAlienFaction_UpdateFactionDaily_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(GeoAlienFaction __instance, List<GeoAlienBase> ____bases)
            {
                GeoAlienBaseTypeDef nestType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Nest_GeoAlienBaseTypeDef"));
                GeoAlienBaseTypeDef lairType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Lair_GeoAlienBaseTypeDef"));
                GeoAlienBaseTypeDef citadelType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Citadel_GeoAlienBaseTypeDefv"));
                GeoAlienBaseTypeDef palaceType = Repo.GetAllDefs<GeoAlienBaseTypeDef>().FirstOrDefault(a => a.name.Equals("Palace_GeoAlienBaseTypeDef"));
               
                int nests = 0;
                int lairs = 0;
                int citadels = 0;
                int palace = 0;

                foreach (GeoAlienBase alienBase in ____bases)
                {
                    for (int i = 0; i < ____bases.Count; i++)
                    {
                        if (alienBase.AlienBaseTypeDef.Equals(nestType))
                        {
                            nests++;                          
                        }
                        else if (alienBase.AlienBaseTypeDef.Equals(lairType))
                        {
                            lairs++;                    
                        }
                        else if (alienBase.AlienBaseTypeDef.Equals(citadelType))
                        {
                            citadels++;
                        }
                        else if (alienBase.AlienBaseTypeDef.Equals(palaceType))
                        {
                            palace++;
                        }
                    }
                }

                __instance.AddEvolutionProgress(nests * __instance.GeoLevel.CurrentDifficultyLevel.EvolutionProgressPerDay / 20 +
                                              lairs * 2 * __instance.GeoLevel.CurrentDifficultyLevel.EvolutionProgressPerDay / 20 +
                                              citadels * 3 * __instance.GeoLevel.CurrentDifficultyLevel.EvolutionProgressPerDay / 20);
            }
        }


        // Harmony patch to change the reveal of alien bases when in scanner range, so increases the reveal chance instead of revealing it right away
        [HarmonyPatch(typeof(GeoAlienFaction), "TryRevealAlienBase")]
        internal static class BC_GeoAlienFaction_TryRevealAlienBase_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static bool Prefix(ref bool __result, GeoSite site, GeoFaction revealToFaction, GeoLevelController ____level)
            {
                if (!site.GetVisible(revealToFaction))
                {
                    GeoAlienBase component = site.GetComponent<GeoAlienBase>();
                    if (revealToFaction is GeoPhoenixFaction && ((GeoPhoenixFaction)revealToFaction).IsSiteInBaseScannerRange(site, true))
                    {
                        component.IncrementBaseAttacksRevealCounter();
                        // original code:
                        //site.RevealSite(____level.PhoenixFaction);
                        //__result = true;
                        //return false;
                    }
                    if (component.CheckForBaseReveal())
                    {
                        site.RevealSite(____level.PhoenixFaction);
                        __result = true;
                        return false;
                    }
                    component.IncrementBaseAttacksRevealCounter();
                }
                __result = false;
                return false; // Return without calling the original method
            }
        }


    }
}
