﻿using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoenixRising.BetterGeoscape
{
    internal class PandoranProgress
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Changes()
        {
            try
            {
                int pandoranEvolutionLegend = BetterGeoscapeMain.Config.evolutionPointsLegend;

                // All sources of evolution due to scaling removed, leaving only evolution per day
                // Additional source of evolution will be number of surviving Pandoran colonies, modulated by difficulty level
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                            
                veryhard.NestLimitations.MaxNumber = 3; //vanilla 6
                veryhard.NestLimitations.HoursBuildTime = 90; //vanilla 45
                veryhard.LairLimitations.MaxNumber = 3; // vanilla 5
                veryhard.LairLimitations.MaxConcurrent = 3; //vanilla 4
                veryhard.LairLimitations.HoursBuildTime = 100; //vanilla 50
                veryhard.CitadelLimitations.HoursBuildTime = 180; //vanilla 60
                veryhard.EvolutionPointsGainOnMissionLoss = 0; //vanilla 10
                veryhard.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0; //vanilla 10
                veryhard.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0; //vanilla 20
                veryhard.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0; //vanilla 40
                veryhard.InfestationOutcomeChance = -50;

                //reducing evolution per day because there other sources of evolution points now
                veryhard.EvolutionProgressPerDay = pandoranEvolutionLegend; //vanilla 100

                //Hero
                GameDifficultyLevelDef hard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("Hard_GameDifficultyLevelDef"));

                hard.NestLimitations.MaxNumber = 3; //vanilla 5
                hard.NestLimitations.HoursBuildTime = 90; //vanilla 50
                hard.LairLimitations.MaxNumber = 3; // vanilla 4
                hard.LairLimitations.MaxConcurrent = 3; //vanilla 3
                hard.LairLimitations.HoursBuildTime = 100; //vanilla 80
                hard.CitadelLimitations.HoursBuildTime = 180; //vanilla 100
                hard.EvolutionPointsGainOnMissionLoss = 0; //vanilla 10
                hard.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0; //vanilla 10
                hard.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0; //vanilla 20
                hard.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0; //vanilla 40
                hard.InfestationOutcomeChance = 0;

                //reducing evolution per day because there other sources of evolution points now
                hard.EvolutionProgressPerDay = 60; //vanilla 70

                //Standard
                GameDifficultyLevelDef standard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("Standard_GameDifficultyLevelDef"));

                standard.NestLimitations.MaxNumber = 3; //vanilla 4
                standard.NestLimitations.HoursBuildTime = 90; //vanilla 55
                standard.LairLimitations.MaxNumber = 3; // vanilla 3
                standard.LairLimitations.MaxConcurrent = 3; //vanilla 3
                standard.LairLimitations.HoursBuildTime = 100; //vanilla 120
                standard.CitadelLimitations.HoursBuildTime = 180; //vanilla 145
                standard.EvolutionPointsGainOnMissionLoss = 0; //vanilla 10
                standard.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0; //vanilla 10
                standard.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0; //vanilla 20
                standard.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0; //vanilla 40
                standard.InfestationOutcomeChance = 0;

                //reducing evolution per day because there other sources of evolution points now
                standard.EvolutionProgressPerDay = 40; //vanilla 55

                //Easy
                GameDifficultyLevelDef easy = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("Easy_GameDifficultyLevelDef"));

                easy.EvolutionPointsGainOnMissionLoss = 0; //vanilla 10
                easy.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0; //vanilla 10
                easy.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0; //vanilla 20
                easy.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0; //vanilla 40
                easy.InfestationOutcomeChance = 0;

                //keeping evolution per day because low enough already
                easy.EvolutionProgressPerDay = 35; //vanilla 35

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

        /*/Removing this
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
        }*/

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
                int difficulty = __instance.GeoLevel.CurrentDifficultyLevel.Order;
                __instance.AddEvolutionProgress(nests * 10 + lairs * 20 + citadels * 30);
                __instance.AddEvolutionProgress(__instance.GeoLevel.EventSystem.GetVariable(InfestationOptionMission.InfestedHavensVariable)*20);
                Logger.Always("There are " + nests+ " nests, " + lairs+" lairs and "+citadels+" citadels on " + __instance.GeoLevel.ElaspedTime);
                Logger.Always("The evolution points per day from Pandoran Colonies are " + (nests * 10 + lairs * 20 + citadels * 30) 
                    + " And from Infested Havens " + __instance.GeoLevel.EventSystem.GetVariable(InfestationOptionMission.InfestedHavensVariable)*20);
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
