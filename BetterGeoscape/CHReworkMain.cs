using AK.Wwise;
using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.Eventus;
using Base.Eventus.Filters;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Entities.Research.Requirement;
using PhoenixPoint.Geoscape.Entities.Research.Reward;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Conditions;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Events.Eventus.Filters;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewModules;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.ActorsInstance;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    class CHReworkMain
    {


        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static void Apply_Changes()
        {
                        
            try
            {
                // Voland messing with Corruption
                // Put Barnabas in the [CHO] picture
                GeoscapeEventDef CH0Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH0_GeoscapeEventDef"));
                CH0Event.GeoscapeEventData.Leader = "SY_Barnabas";

                // Get corruption going from the start of the game... eh with Meteor.
                GeoscapeEventDef geoEventCH0WIN = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH0_WIN_GeoscapeEventDef"));
                var corruption = geoEventCH0WIN.GeoscapeEventData.Choices[0].Outcome.VariablesChange[1];
                GeoscapeEventDef sdi1 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_01_GeoscapeEventDef"));
                sdi1.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(corruption);

                // Remove original Corruption variable change after winning first mission
                //geoEventCH0WIN.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Remove(corruption);

                // Put Barnabas in the picture
                geoEventCH0WIN.GeoscapeEventData.Leader = "SY_Barnabas";

                // Make Acheron research available to Alien Faction without requiring completion of Unexpected Emergency, instead make it appear with Sirens and Chirons (ALN Lair Research)
                ResearchDef ALN_SirenResearch1 = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("ALN_Siren1_ResearchDef"));
                var requirementForAlienAcheronResearch = ALN_SirenResearch1.RevealRequirements.Container[0];
                ResearchDef ALN_AcheronResearch1 = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("ALN_Acheron1_ResearchDef"));
                ALN_AcheronResearch1.RevealRequirements.Container[0] = requirementForAlienAcheronResearch;

                // Make CH0 Mission appear when Player completes Acheron Autopsy and Capture and Containment doesn't work atmo
                GeoResearchEventFilterDef PP_ResearchConditionCH0_Miss = Repo.GetAllDefs<GeoResearchEventFilterDef>().FirstOrDefault(ged => ged.name.Equals("E_PROG_CH0_ResearchCompleted [GeoResearchEventFilterDef]"));
                
                OrEventFilterDef triggerCH1 = Repo.GetAllDefs<OrEventFilterDef>().FirstOrDefault(ged => ged.name.Equals("E_PROG_CH1_MultipleTriggers [OrEventFilterDef]"));
                triggerCH1.OR_Filters[1] = PP_ResearchConditionCH0_Miss;
                GeoscapeEventDef CH0_Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH0_GeoscapeEventDef"));
                CH0Event.Filters[0] = triggerCH1;
                                

                // Make CH1 Mission appear when Player win CH0 Mission; CH1 Event will not be used!
                GeoscapeEventDef CH1_Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH1_GeoscapeEventDef"));
                CH0Event.GeoscapeEventData.Conditions.Add(CH1_Event.GeoscapeEventData.Conditions[1]);
                CH0Event.GeoscapeEventData.Conditions.Add(CH1_Event.GeoscapeEventData.Conditions[3]);
                
                var revealSiteCH1_Miss = CH1_Event.GeoscapeEventData.Choices[0].Outcome.RevealSites[0];
                var setEventCH1_Miss = CH1_Event.GeoscapeEventData.Choices[0].Outcome.SetEvents[0];
                var trackEventCH1_Miss = CH1_Event.GeoscapeEventData.Choices[0].Outcome.TrackEncounters[0];
                GeoscapeEventDef CH0_Won = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH0_WIN_GeoscapeEventDef"));
                CH0_Won.GeoscapeEventData.Choices[0].Outcome.RevealSites.Add(revealSiteCH1_Miss);
                CH0_Won.GeoscapeEventData.Choices[0].Outcome.SetEvents.Add(setEventCH1_Miss);
                CH0_Won.GeoscapeEventData.Choices[0].Outcome.TrackEncounters.Add(trackEventCH1_Miss);
                CH1_Event.GeoscapeEventData.Mute = true;

                // Make Treatment available after completing Research of Escaped Specimen, instead of after Acheron Autopsy
                // Copy unlock from Autopsy research to Specimen 2 (formerly Specimen 0) research and then remove it 
                ResearchDef specimen2Research = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("PX_OriginalAcheron_ResearchDef"));
                specimen2Research.Unlocks = new ResearchRewardDef[]
                {
                        Repo.GetAllDefs<ResearchRewardDef>().FirstOrDefault(ged => ged.name.Equals("PX_Alien_Acheron_ResearchDef_UnlockPandoranSpecializationResearchRewardDef_0")),
                        Repo.GetAllDefs<ResearchRewardDef>().FirstOrDefault(ged => ged.name.Equals("PX_Alien_Acheron_ResearchDef_UnlockFunctionalityResearchRewardDef_0"))
                };
                ResearchDef acheronAutopsy = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("PX_Alien_Acheron_ResearchDef"));
                acheronAutopsy.Unlocks = new ResearchRewardDef[0];

                // Remove requirement to research Mutoid Technology to reserach Specimen 2 (former 0)
                ExistingResearchRequirementDef mutoidRequirement = Repo.GetAllDefs<ExistingResearchRequirementDef>().FirstOrDefault(ged => ged.name.Equals("PX_OriginalAcheron_ResearchDef_ExistingResearchRequirementDef_0"));
                mutoidRequirement.ResearchID = "PX_CaptureTech_ResearchDef";

                // Put Barnabas in the picture of CH1MISSWIN
                GeoscapeEventDef CH1_Won = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH1_WIN_GeoscapeEventDef"));
                CH1_Won.GeoscapeEventData.Leader = "SY_Barnabas";
                //Break the panel in 2, to avoid text wall and give rep bonus with Syn
                CH1_Won.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "PROG_CH1_WIN_OUTCOME_0";
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));
                CH1_Won.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +4
                });


                // Remove event reminding Lair is needed 
                GeoscapeEventDef CH_Event_NeedLair = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH_NEED_LAIR_GeoscapeEventDef"));
                CH_Event_NeedLair.GeoscapeEventData.Mute = true;

                // Change requirements for appearance of CH2MISS works!
                // Create new research requirements
                // Clone trigger for CH2 re Research of Specimen 2 twice
                GeoResearchEventFilterDef sourceResearchTriggerCH2 = Repo.GetAllDefs<GeoResearchEventFilterDef>().FirstOrDefault(ged => ged.name.Equals("E_PROG_CH2_ResearchCompleted [GeoResearchEventFilterDef]"));
                GeoResearchEventFilterDef newResearchTrigger1CH2 = Helper.CreateDefFromClone(sourceResearchTriggerCH2, "4A1E4DA6-A89C-4D7E-B863-FB6B429882CE", "E_PROG_CH2_ResearchCompleted [GeoResearchEventFilterDef]");
                GeoResearchEventFilterDef newResearchTrigger2CH2 = Helper.CreateDefFromClone(sourceResearchTriggerCH2, "2FE2EC90-CBA4-4473-84D7-B343277B2225", "E_PROG_CH2_ResearchCompleted [GeoResearchEventFilterDef]");
                // Set new research triggers to complete virophage research and Scylla vivisection 
                newResearchTrigger1CH2.ResearchID = "PX_VirophageWeapons_ResearchDef";
                newResearchTrigger2CH2.ResearchID = "PX_Alien_LiveQueen_ResearchDef";
                // Add new Research triggers to CH2 event trigger;
                OrEventFilterDef triggerCH2 = Repo.GetAllDefs<OrEventFilterDef>().FirstOrDefault(ged => ged.name.Equals("E_PROG_CH2_MultipleTriggers [OrEventFilterDef]"));
                triggerCH2.OR_Filters[0] = newResearchTrigger1CH2;
                triggerCH2.OR_Filters[1] = newResearchTrigger2CH2;
                // Clone condition 3 (Research of Specimen 2) twice
                FactionConditionDef sourceConditionCH2Research = Repo.GetAllDefs<FactionConditionDef>().FirstOrDefault(ged => ged.name.Equals("[PROG_CH2] Condition 3"));
                FactionConditionDef newCond1CH2E = Helper.CreateDefFromClone(sourceConditionCH2Research, "67D454D6-0BF3-4A13-B503-5A297EEC22CE", "[PROG_CH2] Condition 4");
                FactionConditionDef newCond2CH2E = Helper.CreateDefFromClone(sourceConditionCH2Research, "FDD644C8-A209-4B23-B3A6-C05545E6DAC7", "[PROG_CH2] Condition 5");
                // Set new conditions to complete virophage research and Scylla vivisection               
                newCond1CH2E.CompletedResearchID = "PX_VirophageWeapons_ResearchDef";
                newCond2CH2E.CompletedResearchID = "PX_Alien_LiveQueen_ResearchDef";
                // Add the new conditions to CH2Event
                GeoscapeEventDef CH2_Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH2_GeoscapeEventDef"));
                CH2_Event.GeoscapeEventData.Conditions.Add(newCond1CH2E);
                CH2_Event.GeoscapeEventData.Conditions.Add(newCond2CH2E);
                // Add Barnabas pic to CH2Event
                CH2_Event.GeoscapeEventData.Leader = "SY_Barnabas";
                // Remove final cinematic
                GeoscapeEventDef winCH2 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_CH2_WIN_GeoscapeEventDef"));
                winCH2.GeoscapeEventData.Choices[0].Outcome.Cinematic = CH_Event_NeedLair.GeoscapeEventData.Choices[0].Outcome.Cinematic;

                //Changes to SDI Events
                sdi1.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI1_OUTCOME";
                GeoscapeEventDef sdi6 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_06_GeoscapeEventDef"));
                sdi6.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI6_OUTCOME";
                GeoscapeEventDef sdi16 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_16_GeoscapeEventDef"));
                sdi16.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI16_OUTCOME";
                GeoscapeEventDef sdi20 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_20_GeoscapeEventDef"));
                sdi20.GeoscapeEventData.Choices[0].Outcome.GameOverVictoryFaction = null;

            }
            
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        // Current and last ODI level
        public static int CurrentODI_Level = 0;
        // All SDI (ODI) event IDs, levels as array, index 0 - 19
        public static readonly string[] ODI_EventIDs = new string[]
        {
    "SDI_01",
    "SDI_02",
    "SDI_03",
    "SDI_04",
    "SDI_05",
    "SDI_06",
    "SDI_07",
    "SDI_08",
    "SDI_09",
    "SDI_10",
    "SDI_11",
    "SDI_12",
    "SDI_13",
    "SDI_14",
    "SDI_15",
    "SDI_16",
    "SDI_17",
    "SDI_18",
    "SDI_19",
    "SDI_20"
        };

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        internal static class BC_GeoAlienFaction_UpdateFactionDaily_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(GeoAlienFaction __instance, int ____evolutionProgress)
            {
                Calculate_ODI_Level(__instance, ____evolutionProgress);
            }
        }

        internal static void Calculate_ODI_Level(GeoAlienFaction geoAlienFaction, int evolutionProgress)
        {
            try
            {
                // Index of last element of the ODI event ID array is Length - 1
                int ODI_EventIDs_LastIndex = ODI_EventIDs.Length - 1;
                // Set a maximum number to determine the upper limit from when the maximum ODI level is reached
                int maxODI_Progress = 470 * ODI_EventIDs_LastIndex;
                // Calculate the current ODI level = index for the ODI event ID array
                // Mathf.Min = cap the lavel at max index, after that the index will not longer get increased wiht higher progress
                CurrentODI_Level = Mathf.Min(ODI_EventIDs_LastIndex, evolutionProgress * ODI_EventIDs_LastIndex / maxODI_Progress);
                // Get the GeoLevelController to get access to the event system and the variable
                GeoLevelController geoLevelController = geoAlienFaction.GeoLevel;
                // If current calculated level is different to last saved one then new ODI level is reached, show the new ODI event
                if (geoLevelController.EventSystem.GetVariable("CorruptionActive") == 0 && geoLevelController.EventSystem.GetVariable("PandoraVirus") == 1)
                { }

                else

                    if (CurrentODI_Level != geoLevelController.EventSystem.GetVariable("BC_SDI", -1))
                {
                    // Get the Event ID from array dependent on calculated level index
                    string eventID = ODI_EventIDs[CurrentODI_Level];
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(geoAlienFaction, geoLevelController.ViewerFaction);
                    geoLevelController.EventSystem.TriggerGeoscapeEvent(ODI_EventIDs[CurrentODI_Level], geoscapeEventContext);
                    geoLevelController.EventSystem.SetVariable("BC_SDI", CurrentODI_Level);
                }



            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        [HarmonyPatch(typeof(CorruptionStatus), "GetMultiplier")]
        internal static class BG_CorruptionStatus_GetMultiplier_Mutations_patch
        {
            private static void Postfix(ref float __result, CorruptionStatus __instance)
            {
                try
                {
                    float numberOfMutations = 0;
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.AnuMutationTag;
                    TacticalActor base_TacticalActor = (TacticalActor)AccessTools.Property(typeof(TacStatus), "TacticalActor").GetValue(__instance, null);


                    foreach (TacticalItem armourItem in base_TacticalActor.BodyState.GetArmourItems())
                    {
                        if (armourItem.GameTags.Contains(bionicalTag))
                        {
                            numberOfMutations++;
                        }
                    }                   
                    Logger.Always(numberOfMutations.ToString());

                    if (numberOfMutations > 0)
                    {
                        __result = 1f + (numberOfMutations*2)/100 * (float)base_TacticalActor.CharacterStats.Corruption;
                    }
                    Logger.Always(base_TacticalActor.CharacterStats.Corruption.ToString());
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }    



                // Harmony patch to change the result of CorruptionStatus.CalculateValueIncrement() to be capped by ODI
                // When ODI is <25%, max corruption is 1/3, between 25 and 50% ODI, max corruption is 2/3, and ODI >50%, corruption can be 100%
                // Tell Harmony what original method in what class should get patched, the following class after this directive will be used to perform own code by injection
                [HarmonyPatch(typeof(CorruptionStatus), "CalculateValueIncrement")]

        // The class that holds the code we want to inject, the name can be anything, but the more accurate the better it is for bug hunting
        internal static class BC_CorruptionStatus_CalculateValueIncrement_patch
        {
            // This directive is only to prevent a VS message that the following method is never called (it will be called, but through Harmony and not our mod code)
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]

            // Finally the method that is called before (Prefix) or after (Postfix) the original method
            // In our case we use Postfix that is called after 'CalculateValueIncrement' was executed
            // The parameters are special variables with their names defined by Harmony:
            // 'ref int __result' is the return value of the original method 'CalculateValueIncrement' and with the prefix 'ref' we get write access to change it (without it would be readonly)
            // 'CorruptionStatus __instance' is status object that holds the original method, each character will have its own instance of this status and so we have access to their individual stats
            private static void Postfix(ref int __result, CorruptionStatus __instance)
            {
                // 'try ... catch' to make the code more stable, errors will most likely not result in game crashes or freezes but log an error message in the mods log file
                try
                {
                    // With Harmony patches we cannot directly access base.TacticalActor, Harmony's AccessTools uses Reflection to get it through the backdoor
                    TacticalActor base_TacticalActor = (TacticalActor)AccessTools.Property(typeof(TacStatus), "TacticalActor").GetValue(__instance, null);

                    //Check for bionics
                    int numberOfBionics = 0;
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;

                    foreach (TacticalItem armourItem in base_TacticalActor.BodyState.GetArmourItems())
                    {
                        if (armourItem.GameTags.Contains(bionicalTag))
                        {
                            numberOfBionics++;
                        }
                    }

                    // Calculate the percentage of current ODI level, these two variables are globally set by our ODI event patches
                    int odiPerc = CurrentODI_Level * 100 / ODI_EventIDs.Length;

                    // Get max corruption dependent on max WP of the selected actor
                    int maxCorruption = 0;
                    if (odiPerc < 25)
                    {
                        maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax / 3;

                        if (numberOfBionics == 1)
                        {
                            maxCorruption -= (int)(maxCorruption * 0.33);
                        }

                        if (numberOfBionics == 2)
                        {
                            maxCorruption -= (int)(maxCorruption * 0.66);
                        }

                    }
                    else
                    {
                        if (odiPerc < 75)
                        {
                            maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax * 1 / 2;

                            if (numberOfBionics == 1)
                            {
                                maxCorruption -= (int)(maxCorruption * 0.33);
                            }

                            if (numberOfBionics == 2)
                            {
                                maxCorruption -= (int)(maxCorruption * 0.66);
                            }
                        }
                        else // > 75%
                        {
                            maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax;

                            if (numberOfBionics == 1)
                            {
                                maxCorruption -= (int)(maxCorruption * 0.33);
                            }

                            if (numberOfBionics == 2)
                            {
                                maxCorruption -= (int)(maxCorruption * 0.66);
                            }

                        }
                    }

                    // Like the original calculation, but adapted with 'maxCorruption'
                    // Also '__result' for 'return', '__instance' for 'this' and 'base_TacticalActor' for 'base.TacticalActor'
                    __result = Mathf.Min(__instance.CorruptionStatusDef.ValueIncrement, maxCorruption - base_TacticalActor.CharacterStats.Corruption.IntValue);

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }


        // Dictionary to transfer the characters geoscape stamina to tactical level by actor ID
        public static Dictionary<GeoTacUnitId, int> StaminaMap = new Dictionary<GeoTacUnitId, int>();

        // Harmony patch to save the characters geoscape stamina by acor ID, this mehtod is called in the deployment phase before switching to tactical mode
        [HarmonyPatch(typeof(CharacterFatigue), "ApplyToTacticalInstance")]

        internal static class BC_CharacterFatigue_ApplyToTacticalInstance_Patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(CharacterFatigue __instance, TacCharacterData data)
            {
                try
                {
                    //Logger.Always($"BC_CharacterFatigue_ApplyToTacticalInstance_Patch.POSTFIX called, GeoUnitID {data.Id} with {__instance.Stamina.IntValue} stamina added to dictionary.", false);
                    if (StaminaMap.ContainsKey(data.Id))
                    {
                        StaminaMap[data.Id] = __instance.Stamina.IntValue;
                    }
                    else
                    {
                        StaminaMap.Add(data.Id, __instance.Stamina.IntValue);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }


        // Harmony patch to change the result of CorruptionStatus.GetStatModification() to take Stamina into account
        // Corruption application get reduced by 100% when Stamina is between 35-40, by 75% between 30-35, by 50% between 25-30.
        [HarmonyPatch(typeof(CorruptionStatus), "GetStatModification")]
        internal static class BC_CorruptionStatus_GetStatModification_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            // We use again Postfix that is called after 'GetStatModification' was executed
            // 'ref StatModification __result' is the return value of the original method 'GetStatModification'
            // 'CorruptionStatus __instance' again like above the status object that holds the original method for each character
            private static void Postfix(ref StatModification __result, CorruptionStatus __instance)
            {
                try
                {
                    // With Harmony patches we cannot directly access base.TacticalActor, Harmony's AccessTools uses Reflection to get it through the backdoor
                    TacticalActor base_TacticalActor = (TacticalActor)AccessTools.Property(typeof(TacStatus), "TacticalActor").GetValue(__instance, null);

                    // Get characters geoscape stamina by his actor ID
                    int odiPerc = CurrentODI_Level * 100 / ODI_EventIDs.Length;
                    int stamina = 40;
                    if (StaminaMap.ContainsKey(base_TacticalActor.GeoUnitId))
                    {
                        stamina = StaminaMap[base_TacticalActor.GeoUnitId];
                    }

                    // Calculate WP reduction dependent on stamina
                    float wpReduction = 0; // stamina > 35 and ODI < 25
                    
                    if (odiPerc < 25)
                    {
                        

                        if (stamina > 30 && stamina <= 35)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.25f);
                        }
                        else if (stamina > 25 && stamina <= 30)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.5f);
                        }
                        else if (stamina > 20 && stamina <= 25)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.75f);
                        }
                        else if (stamina <= 20)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption;
                        }
                    }
                    if (odiPerc >= 25 && odiPerc < 50)
                    {
                        
                        if (stamina > 35 && stamina <= 40)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.25f);
                        }
                        else if (stamina > 30 && stamina <= 35)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.5f);
                        }
                        else if (stamina > 25 && stamina <= 30)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.75f);
                        }
                        else if (stamina <= 25)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption;
                        }
                    }

                    if (odiPerc >= 50 && odiPerc < 75)
                    {

                        if (stamina > 35 && stamina <= 40)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.5f);
                        }
                        else if (stamina > 30 && stamina <= 35)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.75f);
                        }
                        else if (stamina <= 30)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption;
                        }
                    }

                    if (odiPerc >= 75)
                    {

                        if (stamina > 35 && stamina <= 40)
                        {
                            wpReduction = Mathf.Round(base_TacticalActor.CharacterStats.Corruption * 0.75f);
                        }
                        else 
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption;
                        }
                    }



                    // Like the original calculation, but adapted with 'maxCorruption'
                    __result = new StatModification(StatModificationType.Add,
                                                    StatModificationTarget.Willpower.ToString(),
                                                    -wpReduction,
                                                    __instance.CorruptionStatusDef,
                                                    -wpReduction);

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }

            }

        }


        [HarmonyPatch(typeof(GeoCharacter), "CureCorruption")]
        public static class GeoCharacter_CureCorruption_SetStaminaTo0_patch
        {
            public static void Postfix(GeoCharacter __instance)
            {

                try
                {

                    PassiveModifierAbilityDef shutEye_Ability = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("ShutEye_AbilityDef"));
                    PassiveModifierAbilityDef hallucinating_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Hallucinating_AbilityDef"));
                    PassiveModifierAbilityDef solipsism_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Solipsism_AbilityDef"));
                    PassiveModifierAbilityDef angerIssues_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("AngerIssues_AbilityDef"));
                    PassiveModifierAbilityDef photophobia_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Photophobia_AbilityDef"));
                    PassiveModifierAbilityDef nails_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Nails_AbilityDef"));
                    PassiveModifierAbilityDef immortality_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Immortality_AbilityDef"));
                    PassiveModifierAbilityDef feral_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Feral_AbilityDef"));
                    DamageMultiplierAbilityDef oneOfUs_AbilityDef = Repo.GetAllDefs<DamageMultiplierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("OneOfUs_AbilityDef"));
                    ApplyStatusAbilityDef fleshEater_AbilityDef = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(ged => ged.name.Equals("FleshEater_AbilityDef"));

                    List<TacticalAbilityDef> abilityList = new List<TacticalAbilityDef>
                    { shutEye_Ability, hallucinating_AbilityDef, solipsism_AbilityDef, angerIssues_AbilityDef, photophobia_AbilityDef, nails_AbilityDef, immortality_AbilityDef, feral_AbilityDef,
                    oneOfUs_AbilityDef, fleshEater_AbilityDef
                    };

                    int num = UnityEngine.Random.Range(0, 200);

                                       
                    if (num >= 0 && num <= 50)
                    {
                        for (int i = 0; i < 100; i++)
                        { 
                           TacticalAbilityDef abilityToAdd=abilityList.GetRandomElement();
                           if (!__instance.Progression.Abilities.Contains(abilityToAdd)) 
                           {
                                __instance.Progression.AddAbility(abilityToAdd);
                                i = 100;
                           }                            
                        }                                           
                    }

                    if (num > 50 && num <= 125)
                    {
                        CommonMethods.SetStaminaToZero(__instance);
                    }
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(PhoenixStatisticsManager), "OnTacticalLevelStart")]
        public static class TacticalLevelController_OnLevelStart_Patch
        {
            public static void Postfix(TacticalLevelController level)
            {
                DefRepository Repo = GameUtl.GameComponent<DefRepository>();
                try
                {
                    foreach (TacticalFaction faction in level.Factions)
                    {
                        if (faction.IsViewerFaction)
                        {
                            foreach (TacticalActor actor in faction.TacticalActors)
                            {

                                PassiveModifierAbilityDef abilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("AngerIssues_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("Frenzy_StatusDef")));
                                }

                                PassiveModifierAbilityDef abilityDef1 = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Hallucinating_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef1) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("ActorSilenced_StatusDef")));
                                }

                                PassiveModifierAbilityDef abilityDef2 = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("FleshEater_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef2) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutog_Devour_AbilityDef")), actor);
                                }

                                PassiveModifierAbilityDef abilityDef3 = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("OneOfUsPassive_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef3) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("OneOfUs_AbilityDef")), actor);
                                }

                                PassiveModifierAbilityDef abilityDef5 = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Nails_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef5) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutoid_Adapt_RightArm_Slasher_AbilityDef")), actor);
                                }

                                PassiveModifierAbilityDef abilityDef6 = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Nails_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef6) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutog_CanLeap_AbilityDef")), actor);
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutog_Leap_AbilityDef")), actor);
                                }
                                /*
                                TacticalAbilityDef abilityDef7 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Immortality_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef7) != null)
                                {
                                    actor.GetArmor().Add(50);
                                    actor.CharacterStats.Armour.Add(100);
                                    //actor.UpdateStats();
                                }
                                */
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        //When getting an augment, the character's Stamina is set to 0 and each augment reduces corruption by a 1/3
        [HarmonyPatch(typeof(UIModuleBionics), "OnAugmentApplied")]
        public static class UIModuleBionics_OnAugmentApplied_SetStaminaTo0_patch
        {
            public static void Postfix(UIModuleBionics __instance)
            {
                try
                {
                    //check number of augments the character has
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;
                    int numberOfBionics = AugmentScreenUtilities.GetNumberOfBionicsAugmentations(__instance.CurrentCharacter);
                    
                    for (int i = 0; i < numberOfBionics; i++)
                    {
                        if (__instance.CurrentCharacter.CharacterStats.Corruption - __instance.CurrentCharacter.CharacterStats.Willpower * 0.33 >= 0)
                        {
                        __instance.CurrentCharacter.CharacterStats.Corruption.Set((float)(__instance.CurrentCharacter.CharacterStats.Corruption - __instance.CurrentCharacter.CharacterStats.Willpower * 0.33));
                        }
                        else
                        {
                         __instance.CurrentCharacter.CharacterStats.Corruption.Set(0);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }


    }
}
