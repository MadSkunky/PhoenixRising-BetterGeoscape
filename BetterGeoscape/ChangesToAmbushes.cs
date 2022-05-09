using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Tactical.Levels.FactionObjectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class ChangesToAmbushes
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Changes()
        {
            try
            {
                //Changing ambush missions so that all of them have crates
                CustomMissionTypeDef AmbushALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAlien_CustomMissionTypeDef"));
                CustomMissionTypeDef SourceScavCratesALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("ScavCratesALN_CustomMissionTypeDef"));
                var pickResourceCratesObjective = SourceScavCratesALN.CustomObjectives[2];
                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushALN.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushALN.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushALN.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushALN.CratesDeploymentPointsRange.Min = 30;
                AmbushALN.CratesDeploymentPointsRange.Max = 50;
                AmbushALN.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushAN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAN_CustomMissionTypeDef"));
                AmbushAN.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushAN.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushAN.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushAN.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushAN.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushAN.CratesDeploymentPointsRange.Min = 30;
                AmbushAN.CratesDeploymentPointsRange.Max = 50;
                AmbushAN.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushBandits = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushBandits_CustomMissionTypeDef"));
                AmbushBandits.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushBandits.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushBandits.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushBandits.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushBandits.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushBandits.CratesDeploymentPointsRange.Min = 30;
                AmbushBandits.CratesDeploymentPointsRange.Max = 50;
                AmbushBandits.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushFallen = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushFallen_CustomMissionTypeDef"));
                AmbushFallen.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushFallen.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushFallen.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushFallen.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushFallen.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushFallen.CratesDeploymentPointsRange.Min = 30;
                AmbushFallen.CratesDeploymentPointsRange.Max = 50;
                AmbushFallen.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushNJ = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushNJ_CustomMissionTypeDef"));
                AmbushNJ.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushNJ.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushNJ.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushNJ.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushNJ.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushNJ.CratesDeploymentPointsRange.Min = 30;
                AmbushNJ.CratesDeploymentPointsRange.Max = 50;
                AmbushNJ.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushPure = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushPure_CustomMissionTypeDef"));
                AmbushPure.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushPure.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushPure.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushPure.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushPure.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushPure.CratesDeploymentPointsRange.Min = 30;
                AmbushPure.CratesDeploymentPointsRange.Max = 50;
                AmbushPure.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushSY = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushSY_CustomMissionTypeDef"));
                AmbushSY.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                AmbushSY.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                AmbushSY.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushSY.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushSY.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushSY.CratesDeploymentPointsRange.Min = 30;
                AmbushSY.CratesDeploymentPointsRange.Max = 50;
                AmbushSY.CustomObjectives[2] = pickResourceCratesObjective;

                //Reduce XP for Ambush mission
                SurviveTurnsFactionObjectiveDef surviveAmbush_CustomMissionObjective = Repo.GetAllDefs<SurviveTurnsFactionObjectiveDef>().FirstOrDefault(ged => ged.name.Equals("SurviveAmbush_CustomMissionObjective"));
                surviveAmbush_CustomMissionObjective.MissionObjectiveData.ExperienceReward = 100;

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        //Harmony patch to increase ambush chance/remove ambush protection
        [HarmonyPatch(typeof(GeoscapeEventSystem), "OnLevelStart")]

        public static class GeoscapeEventSystem_PhoenixFaction_OnLevelStart_Patch
        {
            public static void Prefix(GeoscapeEventSystem __instance)
            {

                try
                {
                    Logger.Debug($"[GeoscapeEventSystem_PhoenixFaction_OnLevelStart_PREFIX] Increasing ambush chance.");
                    __instance.ExplorationAmbushChance = 35;
                    __instance.AmbushExploredSitesProtection = 0;
                    __instance.StartingAmbushProtection = 0;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

    }
}       
