using Base.Core;
using Base.Defs;
using Base.UI;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Common.View.ViewControllers;
using PhoenixPoint.Geoscape.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.Missions.Outcomes;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.View.ViewControllers.Modal;
using System;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class InfestationOptionMission
    {

        public static SharedData sharedData = GameUtl.GameComponent<SharedData>();
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Infestation_Changes()
        {
            try
            {
                AlienRaidsSetupDef raidsSetup = Repo.GetAllDefs<AlienRaidsSetupDef>().FirstOrDefault(ged => ged.name.Equals("_AlienRaidsSetupDef"));
                raidsSetup.RaidBands[0].RollResultMax = 60;
                raidsSetup.RaidBands[1].RollResultMax = 80;
                raidsSetup.RaidBands[2].RollResultMax = 100;
                raidsSetup.RaidBands[3].RollResultMax = 130;
                raidsSetup.RaidBands[4].RollResultMax = 9999;
                raidsSetup.RaidBands[4].AircraftTypesAllowed = 0;

                CustomMissionTypeDef Anu_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationAN_CustomMissionTypeDef"));
                CustomMissionTypeDef NewJericho_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationSY_CustomMissionTypeDef"));
                CustomMissionTypeDef Synderion_Infestation = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenInfestationNJ_CustomMissionTypeDef"));

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

        // Copied and adapted from Mad´s Assorted Adjustments
        internal static GeoHavenDefenseMission DefenseMission = null;
        internal static GeoSite geoSite = null;
        public static string InfestedHavensVariable = "Number_of_Infested_Havens";

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

                    

                    int roll = UnityEngine.Random.Range(0, 6 + __instance.GeoLevel.CurrentDifficultyLevel.Order);
                    int[] rolledVoidOmens = VoidOmens.CheckForAlreadyRolledVoidOmens(__instance.GeoLevel);
                    if (attacker.PPFactionDef == sharedData.AlienFactionDef && __instance.IsInMist && __instance.GeoLevel.EventSystem.GetVariable("Infestation_Encounter_Variable") == 1
                     && (roll >= 6 || rolledVoidOmens.Contains(17)))
                    {
                        geoSite = __instance;
                        __instance.ActiveMission = null;
                        __instance.ActiveMission?.Cancel();

                        Logger.Always("We got to here, defense mission should be successful and haven should look infested");
                        __instance.GeoLevel.EventSystem.SetVariable("Number_of_Infested_Havens", __instance.GeoLevel.EventSystem.GetVariable(InfestedHavensVariable) + 1);

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


        [HarmonyPatch(typeof(GeoscapeLog), "AddEntry")]
        public static class GeoscapeLog_AddEntry_Patch_ConvertDestructionToInfestation
        {
            public static void Prefix(GeoscapeLogEntry entry)
            {
                try
                {
                    if (DefenseMission != null && entry.Parameters.Contains(DefenseMission.Site.SiteName))
                    {
                        entry.Text = new LocalizedTextBind(DefenseMission.Site.Owner + " " + DefenseMission.Haven.Site.Name + " has succumbed to Pandoran infestation!", true);

                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoscapeLog), "Map_SiteMissionEnded")]
        public static class GeoscapeLog_Map_SiteMissionEnded_Patch_ConvertDestructionToInfestation
        {

            public static void Postfix(GeoSite site, GeoMission mission)
            {
                try
                {
                    Logger.Always("Method is invoked");

                    if (geoSite != null && site == geoSite && mission is GeoHavenDefenseMission)
                    {
                        Logger.Always("geoSite is " + geoSite.name);
                        site.GeoLevel.AlienFaction.InfestHaven(geoSite);
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

       
        [HarmonyPatch(typeof(InfestedHavenOutcomeDataBind), "ModalShowHandler")]

        public static class InfestedHavenOutcomeDataBind_Patch_ConvertDestructionToInfestation
        {
            public static bool Prefix(InfestedHavenOutcomeDataBind __instance, UIModal modal, bool ____shown, UIModal ____modal)
            {

                try
                { 
                    Logger.Always("OutcomeDataBind triggered");  
                    if (!____shown)
                    {
                        ____shown = true;
                        if (____modal == null)
                        {
                            ____modal = modal;
                            ____modal.OnModalHide += __instance.ModalHideHandler;
                        }

                        GeoInfestationCleanseMission geoInfestationCleanseMission = (GeoInfestationCleanseMission)modal.Data;
                        GeoSite site = geoInfestationCleanseMission.Site;
                         __instance.Background.sprite = Helper.CreateSpriteFromImageFile("BG_Intro_1.jpg");
                       Sprite icon = __instance.CommonResources.GetFactionInfo(site.Owner).Icon;
                       __instance.TopBar.Icon.sprite = icon;
                       __instance.TopBar.Subtitle.text = site.LocalizedSiteName;
                                               GeoFactionRewardApplyResult applyResult = geoInfestationCleanseMission.Reward.ApplyResult;
                        __instance.AttitudeChange.SetAttitudes(applyResult.Diplomacy);
                        __instance.Rewards.SetReward(geoInfestationCleanseMission.Reward);
                         Logger.Always("InfestedHavensVariable before method is " + site.GeoLevel.EventSystem.GetVariable(InfestedHavensVariable));                               
                         site.GeoLevel.EventSystem.SetVariable(InfestedHavensVariable, site.GeoLevel.EventSystem.GetVariable(InfestedHavensVariable) - 1);
                         Logger.Always("InfestedHavensVariable is " + site.GeoLevel.EventSystem.GetVariable(InfestedHavensVariable));
                    }
                                           
                    return false;

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return true;
                }

            }
        }


    }
}
