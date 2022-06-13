/*using Base.Core;
using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Common.View.ViewControllers;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.Missions.Outcomes;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Events.Eventus.Filters;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewControllers.Modal;
using System;
using System.Linq;

namespace PhoenixRising.BetterGeoscape
{
    internal class Infestation
    {

        public static SharedData sharedData = GameUtl.GameComponent<SharedData>();
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Infestation_Changes()
        {
            try
            {
                
                ResourceMissionOutcomeDef sourceMissonResourceReward = Repo.GetAllDefs<ResourceMissionOutcomeDef>().FirstOrDefault(ged => ged.name.Equals("HavenDefAN_ResourceMissionOutcomeDef"));
                ResourceMissionOutcomeDef mutagenRewardInfestation = Helper.CreateDefFromClone(sourceMissonResourceReward, "2E579AB8-3744-4994-8036-B5018B5E2E15", "InfestationReward");
                mutagenRewardInfestation.Resources.Values.Clear();
                mutagenRewardInfestation.Resources.Values.Add(new ResourceUnit { Type = ResourceType.Mutagen, Value = 800 });

                

                foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                {
                    if (missionTypeDef.name.Contains("Haven") && missionTypeDef.name.Contains("Infestation"))
                    {
                        missionTypeDef.Outcomes[0].DestroySite = true;
                        missionTypeDef.Outcomes[0].Outcomes[2] = mutagenRewardInfestation;                       
                    }
                }
    
            }
            catch (Exception e)
            {
                Logger.Error(e);

            }
        }

        public static void TestInfestationEvent(GeoLevelController level, GeoSite geoSite)
        { try

            {
                GeoscapeEventDef infestationEvent=CommonMethods.CreateNewEvent("testingAnuInfestaton", "KEY_MISSION_HAVEN_INFESTED_BRIEFING_NAME", "KEY_MISSION_CLEANSE_HAVEN", "");               
                CommonMethods.GenerateGeoEventChoice(infestationEvent, "", "");
                infestationEvent.GeoscapeEventData.Choices[1].Outcome.ReEneableEvent = true;
              //  level.EventSystem.SetEventForSite(geoSite, infestationEvent.EventID);
            }
            catch (Exception e)
            {
                Logger.Error(e);

            }
        }
        public static void CreateInfestationEvent(string faction, int num, GeoLevelController level, GeoSite geoSite) 
        
        {
            try
            {

                CustomMissionTypeDef Anu_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationAN_CustomMissionTypeDef"));
            CustomMissionTypeDef NewJericho_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationSY_CustomMissionTypeDef"));
            CustomMissionTypeDef Synderion_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationNJ_CustomMissionTypeDef"));
            
            GeoscapeEventDef infestationEvent = CommonMethods.CreateNewEvent("Infestation_"+faction+"_"+num, "KEY_MISSION_HAVEN_INFESTED_BRIEFING_NAME", "KEY_MISSION_CLEANSE_HAVEN", "");
            Logger.Always(infestationEvent.EventID);
            GeoscapeEventDef infestationEventWin = CommonMethods.CreateNewEvent("Infestation_Win_" + faction + "_" + num, "KEY_MISSION_HAVEN_INFESTED_VICTORY_NAME", "KEY_MISSION_HAVEN_INFESTED_VICTORY_DESCRIPTION", "");
            Logger.Always(infestationEventWin.EventID);
            GeoscapeEventDef infestationEventLost = CommonMethods.CreateNewEvent("Infestation_Lost_" + faction + "_" + num, "KEY_MISSION_HAVEN_INFESTED_DEFEAT_NAME", "KEY_MISSION_HAVEN_INFESTED_DEFEAT_DESCRIPTION", "");
            Logger.Always(infestationEventLost.EventID);
            CommonMethods.GenerateGeoEventChoice(infestationEvent, "KEY_MISSION_CANCEL", "KEY_MISSION_CLEANSE_HAVEN");
            infestationEvent.GeoscapeEventData.Choices[1].Outcome.ReEneableEvent = true;
            
           infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.WonEventID = infestationEventWin.EventID;
            infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.LostEventID = infestationEventLost.EventID;
            
            if (faction == "Anu") 
            { 
                infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = Anu_Infestation;
            }
            else if (faction == "NewJericho") 
            {
                infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = NewJericho_Infestation;
            }
            else if (faction == "Synderion")
            {
                infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = Synderion_Infestation;
            }
            Logger.Always(infestationEvent.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef.TypeName.LocalizeEnglish());
            level.EventSystem.SetEventForSite(geoSite, infestationEvent.EventID);
            }
            catch (Exception e)
            {
                Logger.Error(e);

            }
        }

        // Copied and adapted from Mad´s Assorted Adjustments
        internal static GeoHavenDefenseMission DefenseMission = null;
        internal static GeoSite geoSite = null;



        // Store mission for other patches
        [HarmonyPatch(typeof(GeoHavenDefenseMission), "UpdateGeoscapeMissionState")]
        public static class GeoHavenDefenseMission_UpdateGeoscapeMissionState_StoreMission_Patch
        {
            public static void Prefix(GeoHavenDefenseMission __instance)
            {
                DefenseMission = __instance;
            }

            public static void Postfix()
            {
                DefenseMission = null;
            }
        }

        [HarmonyPatch(typeof(GeoSite), "DestroySite")]
        public static class GeoSite_DestroySite_Patch_ConvertDestructionToInfestation
        {
            public static bool Prefix(GeoSite __instance)
            {
                try
                {
                    Logger.Always("DestroySite method called");
                    string faction = __instance.Owner.GetPPName();
                    Logger.Always(faction);
                    if (DefenseMission == null)
                    {
                        Logger.Always("Defense mission is gone, so this will return true");
                        return true;
                    }
                    Logger.Always("Defense mission is not null, so the method carries on");
                    IGeoFactionMissionParticipant attacker = DefenseMission.GetEnemyFaction();

                    //__instance.GeoLevel.EventSystem.GetVariable("Infestation_Encounter_Variable") == 1
                    // && (roll >= 6 || rolledVoidOmens.Contains(17))

                    int roll = UnityEngine.Random.Range(0, 6 + __instance.GeoLevel.CurrentDifficultyLevel.Order);
                    int[] rolledVoidOmens = VoidOmens.CheckForAlreadyRolledVoidOmens(__instance.GeoLevel);
                    if (attacker.PPFactionDef == sharedData.AlienFactionDef && __instance.IsInMist)
                    {
                        __instance.ActiveMission = null;
                        __instance.ActiveMission?.Cancel();
                        __instance.GeoLevel.AlienFaction.InfestHaven(__instance);
                        Logger.Always("We got to here, defense mission should be successful and haven should look infested");
                        __instance.GeoLevel.EventSystem.SetVariable("Number_of_Infested_Havens", __instance.GeoLevel.EventSystem.GetVariable("Number_of_Infested_Havens")+1);
                        //   geoSite = __instance;

                        TestInfestationEvent(__instance.GeoLevel, __instance);
                        __instance.GeoLevel.EventSystem.SetEventForSite(__instance, "testingAnuInfestaton");
                        
                        //CreateInfestationEvent(faction, __instance.GeoLevel.EventSystem.GetVariable("Number_of_Infested_Havens"), __instance.GeoLevel, __instance);
                         
                        __instance.RefreshVisuals();
                        return false;
                    }
                    Logger.Always("Defense mission is not null, the conditions for infestation were not fulfilled, so return true");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(AlienRaidManager), "CreateRaid")]

        public static class AlienRaidManager_CreateRaid_RemoveFlyerInfestation_Patch
        {
            public static bool Prefix(AlienRaidType type)

            {
                try
                {
                    if (type == AlienRaidType.InfestHaven)
                    {
                        type = AlienRaidType.None;
                        return true;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return true;

                }
            }
        }
        /*
                [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionHourly")]
                public static class GeoSite_SetVisible_Patch_ConvertDestructionToInfestation
                {
                    public static void Postfix(GeoAlienFaction __instance)
                    {
                        try
                        {
                            if (geoSite != null)
                            {
                                Logger.Always("Method SetVisible invoked");
                                Logger.Always("geoSite is " + geoSite.Name);
                                __instance.InfestHaven(geoSite);
                                Logger.Always("We got to here, haven should be infested!");
                                geoSite = null;
                            }


                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                        }
                    }
                }
        /* [HarmonyPatch(typeof(GeoSite), "VehicleArrived")]

         public static class GeoSite_Patch_ConvertDestructionToInfestation 

         {
             public static void Postfix(GeoVehicle vehicle)
             {
                 try
                 {
                     if (vehicle.IsOwnedByViewer && vehicle.CurrentSite==geoSite) 
                     {
                         GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(vehicle.GeoLevel.AlienFaction, vehicle.GeoLevel.ViewerFaction);
                         vehicle.GeoLevel.EventSystem.TriggerGeoscapeEvent("PROG_LW2_MISS", geoscapeEventContext);               
                     }
                 }

                 catch (Exception e)
                 {
                     Logger.Error(e);
                 }

             }

         }
        */
     /*   [HarmonyPatch(typeof(MissionModalDataBind), "ApplyBinding")]
        public static class MissionModalDataBind_Patch_ConvertDestructionToInfestation
        {
            public static void Prefix(ref TacMissionTypeDef.MissionModalBind bind)
            {
                try
                {
                    bind.Title.LocalizationKey = "KEY_MISSION_HAVEN_INFESTED_VICTORY_NAME";
                    bind.Description.LocalizationKey = "KEY_MISSION_HAVEN_INFESTED_VICTORY_DESCRIPTION";
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
         
        }



        /*
                [HarmonyPatch(typeof(InfestedHavenOutcomeDataBind), "TopBarController")]

                public static class InfestedHavenOutcomeDataBind_Patch_ConvertDestructionToInfestation
                {
                    public static void Postfix(TopBarController __instance)
                    {

                        try
                        {
                            __instance.Title.text = "HAVEN CLEANSED";
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                        }

                    }
                }

                
    }
}
*/