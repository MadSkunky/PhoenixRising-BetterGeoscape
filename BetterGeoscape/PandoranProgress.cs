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
                //Testing Less Pandoran Colonies
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                veryhard.NestLimitations.MaxNumber = 3;
                veryhard.NestLimitations.HoursBuildTime = 90;
                veryhard.LairLimitations.MaxNumber = 3;
                veryhard.LairLimitations.MaxConcurrent = 3;
                veryhard.LairLimitations.HoursBuildTime = 100;
                veryhard.CitadelLimitations.HoursBuildTime = 180;
                veryhard.EvolutionProgressPerDay = 50;
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
