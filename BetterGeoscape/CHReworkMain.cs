using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.Eventus.Filters;
using Base.UI;
using Base.UI.MessageBox;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.UI;
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
using PhoenixPoint.Geoscape.Levels.Objectives;
using PhoenixPoint.Geoscape.View.DataObjects;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewControllers.BaseRecruits;
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
                GeoscapeEventDef sdi3 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_03_GeoscapeEventDef"));
                sdi3.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(CommonMethods.GenerateVariableChange("Umbra_Encounter_Variable", 1, false));
                GeoscapeEventDef sdi6 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_06_GeoscapeEventDef"));
                sdi6.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI6_OUTCOME";
                GeoscapeEventDef sdi7 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_07_GeoscapeEventDef"));
                //Need to fix a broken SDI event!
                sdi7.GeoscapeEventData.Choices = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_06_GeoscapeEventDef")).GeoscapeEventData.Choices;
                sdi7.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI7_OUTCOME";
                sdi7.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(CommonMethods.GenerateVariableChange("Infestation_Encounter_Variable", 1, false));
                GeoscapeEventDef sdi09 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_09_GeoscapeEventDef"));
                sdi09.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(CommonMethods.GenerateVariableChange("Umbra_Encounter_Variable", 1, false));
                GeoscapeEventDef sdi10 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_10_GeoscapeEventDef"));
                sdi10.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI10_OUTCOME";
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

        public static readonly string[] VoidOmens_Title = new string[]
        {
        "VOID_OMEN_TITLE_01","VOID_OMEN_TITLE_02","VOID_OMEN_TITLE_03","VOID_OMEN_TITLE_04","VOID_OMEN_TITLE_05","VOID_OMEN_TITLE_06",
        "VOID_OMEN_TITLE_07","VOID_OMEN_TITLE_08","VOID_OMEN_TITLE_09","VOID_OMEN_TITLE_10","VOID_OMEN_TITLE_11","VOID_OMEN_TITLE_12",
        "VOID_OMEN_TITLE_13","VOID_OMEN_TITLE_14","VOID_OMEN_TITLE_15","VOID_OMEN_TITLE_16","VOID_OMEN_TITLE_17","VOID_OMEN_TITLE_18",
        "VOID_OMEN_TITLE_19","VOID_OMEN_TITLE_20",
        };
        public static readonly string[] VoidOmens_Description = new string[]
        {
        "VOID_OMEN_DESCRIPTION_TEXT_01","VOID_OMEN_DESCRIPTION_TEXT_02","VOID_OMEN_DESCRIPTION_TEXT_03","VOID_OMEN_DESCRIPTION_TEXT_04",
        "VOID_OMEN_DESCRIPTION_TEXT_05","VOID_OMEN_DESCRIPTION_TEXT_06","VOID_OMEN_DESCRIPTION_TEXT_07","VOID_OMEN_DESCRIPTION_TEXT_08",
        "VOID_OMEN_DESCRIPTION_TEXT_09","VOID_OMEN_DESCRIPTION_TEXT_10","VOID_OMEN_DESCRIPTION_TEXT_11","VOID_OMEN_DESCRIPTION_TEXT_12",
        "VOID_OMEN_DESCRIPTION_TEXT_13","VOID_OMEN_DESCRIPTION_TEXT_14","VOID_OMEN_DESCRIPTION_TEXT_15","VOID_OMEN_DESCRIPTION_TEXT_16",
        "VOID_OMEN_DESCRIPTION_TEXT_17","VOID_OMEN_DESCRIPTION_TEXT_18","VOID_OMEN_DESCRIPTION_TEXT_19","VOID_OMEN_DESCRIPTION_TEXT_20",
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
                {
                    VoidOmens.RemoveAllVoidOmens(geoLevelController);
                }

                else

                    if (CurrentODI_Level != geoLevelController.EventSystem.GetVariable("BC_SDI", -1))
                {
                    // Get the Event ID from array dependent on calculated level index

                    string eventID = ODI_EventIDs[CurrentODI_Level];
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(geoAlienFaction, geoLevelController.ViewerFaction);
                    GeoscapeEventDef oDIEventToTrigger = geoLevelController.EventSystem.GetEventByID(ODI_EventIDs[CurrentODI_Level]);

                    // Void Omens roll
                    // Before the roll, Void Omen has not been rolled
                    bool voidOmenRolled = false;
                    int voidOmenRoll = 0;
                    // Create variable reflecting difficulty level, 1 being the easiest, and 4 the hardest
                    // This will determine amount of possible simultaneous Void Omens
                    int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                    string triggeredVoidOmens = "TriggeredVoidOmen_";
                    string voidOmen = "VoidOmen_";

                    if (geoLevelController.EventSystem.GetVariable("BC_SDI") > 0)
                    {
                        // Here comes the roll, for testing purposes with 1/10 chance of no VO happening    
                        int roll = UnityEngine.Random.Range(1, 10);
                        Logger.Always("The roll on the 1D10 is " + roll);
                        if (roll == 1)
                        {
                            VoidOmens.RemoveEarliestVoidOmen(geoLevelController);
                        }

                        if (roll >= 2 && roll <= 10)
                        {

                            // If a Void Omen rolls
                            // Create list of Void Omens currently implemented
                            List<int> voidOmensList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14};
                            Logger.Always("The Void Omens initially available for the roll are " + voidOmensList.Count());
                            if (geoAlienFaction.Research.HasCompleted("ALN_CrabmanUmbra_ResearchDef"))
                            {
                                voidOmensList.Add(15);
                                voidOmensList.Add(16);
                                Logger.Always("The list of Void Omens is now " + voidOmensList.Count() + " long");
                            }
                            if (geoAlienFaction.GeoLevel.EventSystem.GetVariable("BehemothEggHatched") == 1)
                            {
                                voidOmensList.Add(11);
                                Logger.Always("The list of Void Omens is now " + voidOmensList.Count() + " long");
                            }
                            if (geoAlienFaction.GeoLevel.EventSystem.GetVariable("Infestation_Encounter_Variable") == 1)
                            {
                                voidOmensList.Add(17);
                                Logger.Always("The list of Void Omens is now " + voidOmensList.Count() + " long");
                            }

                            // Check for already rolled Void Omens
                            int[] allVoidOmensAlreadyRolled = VoidOmens.CheckForAlreadyRolledVoidOmens(geoLevelController);
                            //Remove already rolled Void Omens from the list of available Void Omens
                            for (int i = 1; i < allVoidOmensAlreadyRolled.Length; i++)
                            {
                                if (voidOmensList.Contains(allVoidOmensAlreadyRolled[i]) && allVoidOmensAlreadyRolled[i] != 0)
                                {
                                    voidOmensList.Remove(allVoidOmensAlreadyRolled[i]);
                                }
                            }
                            Logger.Always("The Void Omens already rolled are " + allVoidOmensAlreadyRolled);
                            Logger.Always("The Void Omens available for the roll after the crib are " + voidOmensList);

                            // Get a random dark event from the available Void Omens list
                            voidOmenRoll = voidOmensList.GetRandomElement();
                            Logger.Always("The Void Omen rolled is " + voidOmenRoll);

                            // We can have as many simulateneous Void Omens in play as the mathematical expression of the difficulty level
                            // Lets check how many Void Omens are already in play and if there is space for more
                            int[] voidOmensInPlay = VoidOmens.CheckFordVoidOmensInPlay(geoLevelController);
                            Logger.Always("The Void Omens currently in play are " + voidOmensInPlay);
                            //If there is no space, we have to remove the earliest one
                            if (!voidOmensInPlay.Contains(0))
                            {
                                VoidOmens.RemoveEarliestVoidOmen(geoLevelController);
                            }
                            //Then let's find a spot for the new Void Omen
                            for (int t = 0; t < difficulty; t++)
                            {
                                // There will be as many Void Omen variables (each storing an active Void Omen) as the ME of the difficulty level
                                // The first empty Dark Event variable will receive the new Void Omen
                                if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - t)) == 0)
                                {
                                    // This is the regular code to modify a Def, in this case the ODI event to which the Void Omen will be attached,
                                    // so that it sets the Void Omen variable
                                    oDIEventToTrigger.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange
                                    {
                                        VariableName = voidOmen + (difficulty - t),
                                        Value = { Min = voidOmenRoll, Max = voidOmenRoll },
                                        IsSetOperation = true,
                                    });
                                    // This records which ODI event triggered which Void Omen
                                    geoLevelController.EventSystem.SetVariable(triggeredVoidOmens + CurrentODI_Level, voidOmenRoll);
                                    // Raise the flag, we have a Void Omen!
                                    voidOmenRolled = true;
                                    // Then close the loop:
                                    t = 4;
                                }
                            }
                        }
                    }

                    // The ODI event is triggered
                    geoLevelController.EventSystem.TriggerGeoscapeEvent(ODI_EventIDs[CurrentODI_Level], geoscapeEventContext);
                    geoLevelController.EventSystem.SetVariable("BC_SDI", CurrentODI_Level);
                    //UpdateODITracker(CurrentODI_Level, geoLevelController); not used currently, because clogs the UI
                    // And if a Void Omen has been rolled, a Void Omen will appear
                    if (voidOmenRolled && geoLevelController.EventSystem.GetVariable(voidOmen + difficulty) == voidOmenRoll && geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - 1)) == 0)
                    {
                        GeoscapeEventDef voidOmenIntro = geoLevelController.EventSystem.GetEventByID("VoidOmen");
                        voidOmenIntro.GeoscapeEventData.Title.LocalizationKey = "VOID_OMEN_INTRO_TITLE";
                        voidOmenIntro.GeoscapeEventData.Description[0].General.LocalizationKey = "VOID_OMEN_INTRO";
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("VoidOmenIntro", geoscapeEventContext);
                    }
                    // This adds the Void Omen to the objective list, for now still called Dark Events in the code because don't want to mess with existing savegames
                    if (voidOmenRolled)
                    {
                        string title = (string)VoidOmens_Title.GetValue(voidOmenRoll - 1);
                        string description = (string)VoidOmens_Description.GetValue(voidOmenRoll - 1);
                        GeoscapeEventDef voidOmenEvent = geoLevelController.EventSystem.GetEventByID("VoidOmen");
                        voidOmenEvent.GeoscapeEventData.Title.LocalizationKey = title;
                        voidOmenEvent.GeoscapeEventData.Description[0].General.LocalizationKey = description;
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("VoidOmen", geoscapeEventContext);
                        CreateVoidOmenObjective(title, description, geoLevelController);
                    }
                }
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void CreateVoidOmenObjective(string title, string description, GeoLevelController level)
        {
            try
            {
                DiplomaticGeoFactionObjective voidOmenObjective = new DiplomaticGeoFactionObjective(level.PhoenixFaction, level.PhoenixFaction)
                {
                    Title = new LocalizedTextBind(title),
                    Description = new LocalizedTextBind(description),
                };
                level.PhoenixFaction.AddObjective(voidOmenObjective);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        /*
        // Show additional info in objectives
        [HarmonyPatch(typeof(UIModuleGeoObjectives), "InitObjective")]
        public static class UIModuleGeoObjectives_InitObjective_Patch
        {
            
            public static void Prefix(UIModuleGeoObjectives __instance, ref GeoFactionObjective objective)
            {
                try
                {
                    if (!(objective is MissionGeoFactionObjective missionGeoFactionObjective) || !(missionGeoFactionObjective.Mission is GeoHavenDefenseMission geoHavenDefenseMission))
                    {
                        return;
                    }

                    IGeoFactionMissionParticipant enemyFaction = geoHavenDefenseMission.GetEnemyFaction();
                    Color enemyColor = enemyFaction.ParticipantViewDef.FactionColor;
                    string enemyColorHex = $"#{ColorUtility.ToHtmlStringRGB(enemyColor)}";
                    string enemyName = enemyFaction.ParticipantName.Localize();
                    string enemyText = $"<color={enemyColorHex}>{enemyName}</color>";

                    objective.Title = new LocalizedTextBind("Defend {0} against " + enemyText, true);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        */


        [HarmonyPatch(typeof(GeoFactionObjective), "GetIcon")]
        internal static class BG_GeoFactionObjective_GetIcon_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(ref Sprite __result, GeoFactionObjective __instance)
            {
                try
                {
                    if (__instance.Title != null && __instance.Title.LocalizationKey.Contains("VOID_OMEN_TITLE_"))
                    {
                        __result = Helper.CreateSpriteFromImageFile("Void-04P.png");
                    }

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
        /*    [HarmonyPatch(typeof(GeoObjectiveElementController), "SetObjective")]
            internal static class BG_GeoObjectiveElementController_SetObjective_patch
            {
                [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
                private static void Postfix(string objectiveText, Sprite icon, LocalizedTextBind tooltipText)
                {
                    try
                    {
                        if (VoidOmens_Title.Contains(objectiveText))
                        {
                            icon = Helper.CreateSpriteFromImageFile("Void-04P.png");
                            tooltipText.LocalizationKey = objectiveText;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                    }
                }
            }*/


        public static void RemoveVoidOmenObjective(string title, GeoLevelController level)
        {
            try
            {
                DiplomaticGeoFactionObjective voidOmenObjective =
            (DiplomaticGeoFactionObjective)level.PhoenixFaction.Objectives.FirstOrDefault(ged => ged.Title.LocalizationKey.Equals(title));
                string checktitle = voidOmenObjective.GetTitle();
                Logger.Always("the title in the RemoveVoidOmenObjective method is " + title);
                Logger.Always("if we found the objective, there should be something here " + checktitle);
                level.PhoenixFaction.RemoveObjective(voidOmenObjective);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        //ODI Tracker
        /*
        public static void UpdateODITracker(int odiLevel, GeoLevelController level)
        {

            if (odiLevel > 0)
            {
                DiplomaticGeoFactionObjective oldOdiTracker =
               (DiplomaticGeoFactionObjective)level.PhoenixFaction.Objectives.FirstOrDefault(ged => ged.Title.LocalizationKey.Equals("SDI_TITLE_" + (odiLevel)));

                level.PhoenixFaction.RemoveObjective(oldOdiTracker);
            }

            DiplomaticGeoFactionObjective newOdiTracker = new DiplomaticGeoFactionObjective(level.AlienFaction, level.PhoenixFaction)
            {
                Title = new LocalizedTextBind("SDI_TITLE_"+(odiLevel+1)),
                Description = new LocalizedTextBind("test"),
                IsCriticalPath = true,
            };
            level.PhoenixFaction.AddObjective(newOdiTracker);
        }*/

        // Patch to increase damage by 2% per mutated body part per Delirium point
        [HarmonyPatch(typeof(CorruptionStatus), "GetMultiplier")]
        internal static class BG_CorruptionStatus_GetMultiplier_Mutations_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
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


                    if (numberOfMutations > 0)
                    {
                        __result = 1f + (numberOfMutations * 2) / 100 * (float)base_TacticalActor.CharacterStats.Corruption;
                    }

                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        //General method to calculate max Delirium a character on geo can have taking into account ODI and bionics
        public static float CalculateMaxCorruption(GeoCharacter character)
        {

            try
            {
                float maxCorruption = 0;
                int bionics = 0;
                int currentODIlevel = character.Faction.GeoLevel.EventSystem.GetVariable("BC_SDI");
                int odiPerc = currentODIlevel * 100 / ODI_EventIDs.Length;

                GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;
                foreach (GeoItem bionic in character.ArmourItems)
                {
                    if (bionic.ItemDef.Tags.Contains(bionicalTag))

                        bionics += 1;
                }

                if (!VoidOmens.VoidOmen10Active)
                {
                    if (odiPerc < 25)
                    {
                        maxCorruption = character.CharacterStats.Willpower.IntMax / 3;

                        if (bionics == 1)
                        {
                            return maxCorruption -= maxCorruption * 0.33f;
                        }

                        if (bionics == 2)
                        {
                            return maxCorruption -= maxCorruption * 0.66f;
                        }
                        else
                        {
                            return maxCorruption;
                        }
                    }
                    else
                    {
                        if (odiPerc < 50)
                        {
                            maxCorruption = character.CharacterStats.Willpower.IntMax * 1 / 2;

                            if (bionics == 1)
                            {
                                return maxCorruption -= maxCorruption * 0.33f;
                            }

                            if (bionics == 2)
                            {
                                return maxCorruption -= maxCorruption * 0.66f;
                            }
                            else
                            {
                                return maxCorruption;
                            }
                        }
                        else // > 75%
                        {
                            maxCorruption = character.CharacterStats.Willpower.IntMax;

                            if (bionics == 1)
                            {
                                return maxCorruption -= maxCorruption * 0.33f;
                            }

                            if (bionics == 2)
                            {
                                return maxCorruption -= maxCorruption * 0.66f;
                            }

                            else
                            {
                                return maxCorruption;
                            }
                        }
                    }

                }
                if (VoidOmens.VoidOmen10Active)
                {
                    maxCorruption = character.CharacterStats.Willpower.IntMax;

                    if (bionics == 1)
                    {
                        return maxCorruption -= maxCorruption * 0.33f;
                    }

                    if (bionics == 2)
                    {
                        return maxCorruption -= maxCorruption * 0.66f;
                    }

                    else
                    {
                        return maxCorruption;
                    }

                }

            }
            catch (System.Exception e)
            {
                Logger.Error(e);
            }

            throw new InvalidOperationException();
        }


        // General method to calculate Stamina effect on Delirium, where each 10 stamina reduces Delirium effects by 1
        public static int CalculateStaminaEffectOnDelirium(GeoCharacter character)
        {
            {

                try
                {
                    /* string stamina40 = "<color=#18f005>-4 to Delirium effect(WP loss)</color>";
                     string stamina30to39 = "<color=#c1f005>-3 to Delirium effect(WP loss)</color>";
                     string stamina20to29 = "<color=#f0e805>-2 to Delirium effect(WP loss)</color>";
                     string stamina10to19 = "<color=##f07b05>-1 to Delirium effect(WP loss)</color>";
                     string stamina0to9= "<color=#f00505>Delirium has full effect</color>";*/

                    if (character.Fatigue.Stamina == 40)
                    {
                        return 4;
                    }
                    else if (character.Fatigue.Stamina < 40 && character.Fatigue.Stamina >= 30)
                    {
                        return 3;
                    }
                    else if (character.Fatigue.Stamina < 30 && character.Fatigue.Stamina >= 20)
                    {
                        return 2;
                    }
                    else if (character.Fatigue.Stamina < 20 && character.Fatigue.Stamina >= 10)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                catch (System.Exception e)
                {
                    Logger.Error(e);
                }

                throw new InvalidOperationException();
            }
        }


        //This method changes how WP are displayed in the Edit personnel screen, to show effects of Delirium on WP

        [HarmonyPatch(typeof(UIModuleCharacterProgression), "GetStarBarValuesDisplayString")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
        internal static class BG_UIModuleCharacterProgression_RefreshStatPanel_patch
        {

            private static void Postfix(GeoCharacter ____character, ref string __result, CharacterBaseAttribute attribute, int currentAttributeValue)
            {
                try
                {
                    if (____character.CharacterStats.Corruption > CalculateStaminaEffectOnDelirium(____character) && attribute.Equals(CharacterBaseAttribute.Will))
                    {
                        __result = $"<color=#da5be3>{currentAttributeValue - ____character.CharacterStats.Corruption.Value + CalculateStaminaEffectOnDelirium(____character)}</color>" + $"({currentAttributeValue}) / " +
                        $"{____character.Progression.GetMaxBaseStat(CharacterBaseAttribute.Will)}";
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
        //
        //This changes display of Delirium bar in personnel edit screen to show current Delirium value vs max delirium value the character can have
        // taking into account ODI level and bionics
        [HarmonyPatch(typeof(UIModuleCharacterProgression), "SetStatusesPanel")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
        internal static class BG_UIModuleCharacterProgression_SetStatusesPanel_patch
        {

            private static void Postfix(UIModuleCharacterProgression __instance, GeoCharacter ____character)
            {
                try
                {
                    if (____character.CharacterStats.Corruption > 0f)

                    {
                        __instance.CorruptionSlider.minValue = 0f;
                        __instance.CorruptionSlider.maxValue = CalculateMaxCorruption(____character);
                        __instance.CorruptionSlider.value = ____character.CharacterStats.Corruption.IntValue;
                        __instance.CorruptionStatText.text = $"{____character.CharacterStats.Corruption.IntValue}/{Mathf.RoundToInt(__instance.CorruptionSlider.maxValue)}";

                        int num = (int)(float)____character.Fatigue.Stamina;
                        int num2 = (int)(float)____character.Fatigue.Stamina.Max;
                        __instance.StaminaSlider.minValue = 0f;
                        __instance.StaminaSlider.maxValue = num2;
                        __instance.StaminaSlider.value = num;
                        if (num != num2)
                        {
                            string deliriumReducedStamina = "";
                            for (int i = 0; i < CalculateStaminaEffectOnDelirium(____character); i++)
                            {
                                deliriumReducedStamina += "-";

                            }
                            __instance.StaminaStatText.text = $"<color=#da5be3>{deliriumReducedStamina}</color>" + num + "/" + num2;
                        }
                        else
                        {
                            __instance.StaminaStatText.text = "<color=#da5be3> ---- </color>" + num.ToString();
                        }
                    }
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
                    int maxCorruption = 0;
                    // Get max corruption dependent on max WP of the selected actor
                    if (!VoidOmens.VoidOmen10Active)
                    {

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
                            if (odiPerc < 50)
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
                    }
                    if (VoidOmens.VoidOmen10Active)
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

                    int stamina = 40;
                    if (StaminaMap.ContainsKey(base_TacticalActor.GeoUnitId))
                    {
                        stamina = StaminaMap[base_TacticalActor.GeoUnitId];
                    }

                    // Calculate WP reduction dependent on stamina
                    float wpReduction = base_TacticalActor.CharacterStats.Corruption;

                    if (VoidOmens.VoidOmen3Active || VoidOmens.VoidOmen3Activated)
                    {
                        wpReduction = 0;
                    }
                    else
                    {
                        wpReduction = base_TacticalActor.CharacterStats.Corruption; // stamina between 0 and 10

                        if (stamina == 40)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption - 4;
                        }
                        else if (stamina >= 30 && stamina < 40)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption - 3;
                        }
                        else if (stamina >= 20 && stamina < 30)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption - 2;
                        }
                        else if (stamina >= 10 && stamina < 20)
                        {
                            wpReduction = base_TacticalActor.CharacterStats.Corruption - 1;
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

                    ApplyStatusAbilityDef nails_AbilityDef = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Nails_AbilityDef"));

                    PassiveModifierAbilityDef immortality_AbilityDef = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Immortality_AbilityDef"));

                    ApplyStatusAbilityDef feral_AbilityDef = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(ged => ged.name.Equals("Feral_AbilityDef"));

                    DamageMultiplierAbilityDef oneOfUs_AbilityDef = Repo.GetAllDefs<DamageMultiplierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("OneOfUs_AbilityDef"));

                    ApplyStatusAbilityDef fleshEater_AbilityDef = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(ged => ged.name.Equals("FleshEater_AbilityDef"));


                    List<TacticalAbilityDef> abilityList = new List<TacticalAbilityDef>
                    { shutEye_Ability, hallucinating_AbilityDef, solipsism_AbilityDef, angerIssues_AbilityDef, photophobia_AbilityDef, nails_AbilityDef, immortality_AbilityDef, feral_AbilityDef,
                    oneOfUs_AbilityDef, fleshEater_AbilityDef
                    };

                    int num = UnityEngine.Random.Range(0, 200);
                    // GeoscapeTutorialStepsDef stepTest = Repo.GetAllDefs<GeoscapeTutorialStepsDef>().FirstOrDefault(ged => ged.name.Equals("GeoscapeTutorialStepsDef"));
                    // GeoscapeTutorialStep test = new GeoscapeTutorialStep();
                    // test.Title.LocalizationKey = $"test";
                    // test.Description.LocalizationKey = $"testing";
                    Logger.Always("Treatment rolled " + num);

                    if (num >= 0 && num <= 50)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            TacticalAbilityDef abilityToAdd = abilityList.GetRandomElement();
                            Logger.Always("The randomly chosen ability is " + abilityToAdd.name);
                            if (!__instance.Progression.Abilities.Contains(abilityToAdd))
                            {

                                __instance.Progression.AddAbility(abilityToAdd);
                                //__instance.Faction.GeoLevel.View.GeoscapeModules.TutorialModule.SetTutorialStep(test, false);
                                GameUtl.GetMessageBox().ShowSimplePrompt($"{__instance.GetName()}" + " appears to be afflicted with " + $"<b>{abilityToAdd.ViewElementDef.DisplayName1.LocalizeEnglish()}</b>"
                                    + " as a result of the experimental mutagen treatment. This condition is likely to be permanent."
                                    + "\n\n" + $"<i>{abilityToAdd.ViewElementDef.Description.LocalizeEnglish()}</i>", MessageBoxIcon.None, MessageBoxButtons.OK, null);
                                Logger.Always("Added ability " + abilityToAdd.ViewElementDef.DisplayName1.LocalizeEnglish());
                                i = 100;
                            }
                        }
                    }
                    else if (num > 50 && num <= 125)
                    {
                        CommonMethods.SetStaminaToZero(__instance);
                        GameUtl.GetMessageBox().ShowSimplePrompt($"{__instance.GetName()}" + " did not suffer any lasting side effects, but had to be heavily sedated"
                                    + "\n\n" + $"<i>STAMINA reduced to zero</i>", MessageBoxIcon.None, MessageBoxButtons.OK, null);
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
                                TacticalAbilityDef abilityDef = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("AngerIssues_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("Frenzy_StatusDef")));
                                    Logger.Always(actor.name + " with " + abilityDef.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());
                                }

                                TacticalAbilityDef abilityDef1 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Hallucinating_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef1) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("Hallucinating_StatusDef")));
                                    Logger.Always(actor.name + " with " + abilityDef1.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());
                                }

                                TacticalAbilityDef abilityDef2 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("FleshEater_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef2) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("FleshEaterHP_AbilityDef")), actor);
                                    Logger.Always(actor.name + " with " + abilityDef2.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());
                                }
                                TacticalAbilityDef abilityDef3 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("OneOfUsPassive_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef3) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("MistResistance_StatusDef")));
                                    actor.GameTags.Add(Repo.GetAllDefs<GameTagDef>().FirstOrDefault(sd => sd.name.Equals("Takashi_GameTagDef")), GameTagAddMode.ReplaceExistingExclusive);
                                    Logger.Always(actor.name + " with " + abilityDef3.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());
                                }

                                TacticalAbilityDef abilityDef5 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Nails_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef5) != null)
                                {
                                    actor.AddAbility(Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("NailsPassive_AbilityDef")), actor);
                                    Logger.Always(actor.name + " with " + abilityDef5.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());
                                }

                                TacticalAbilityDef abilityDef7 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Immortality_AbilityDef"));
                                if (actor.GetAbilityWithDef<Ability>(abilityDef7) != null)
                                {
                                    actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("ArmorBuffStatus_StatusDef")));
                                    Logger.Always(actor.name + " with " + abilityDef7.name + " has the following statuses: " + actor.Status.CurrentStatuses.ToString());

                                }
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



        [HarmonyPatch(typeof(TacticalAbility), "FumbleActionCheck")]
        public static class TacticalAbility_FumbleActionCheck_Patch
        {
            public static void Postfix(TacticalAbility __instance, ref bool __result)
            {
                DefRepository Repo = GameUtl.GameComponent<DefRepository>();

                try
                {
                    TacticalAbilityDef abilityDef9 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Feral_AbilityDef"));
                    if (__instance.TacticalActor.GetAbilityWithDef<TacticalAbility>(abilityDef9) != null && __instance.Source is Equipment)
                    {
                        __result = UnityEngine.Random.Range(0, 100) < 10;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
        //Dtony's Delirium perks patch
        [HarmonyPatch(typeof(RecruitsListElementController), "SetRecruitElement")]
        public static class RecruitsListElementController_SetRecruitElement_Patch
        {
            public static bool Prefix(RecruitsListElementController __instance, RecruitsListEntryData entryData, List<RowIconTextController> ____abilityIcons)
            {
                try
                {
                    if (____abilityIcons == null)
                    {
                        ____abilityIcons = new List<RowIconTextController>();
                        if (__instance.PersonalTrackRoot.transform.childCount < entryData.PersonalTrackAbilities.Count())
                        {
                            RectTransform parent = __instance.PersonalTrackRoot.GetComponent<RectTransform>();
                            RowIconTextController source = parent.GetComponentInChildren<RowIconTextController>();
                            parent.DetachChildren();
                            source.Icon.GetComponent<RectTransform>().sizeDelta = new Vector2(95f, 95f);
                            for (int i = 0; i < entryData.PersonalTrackAbilities.Count(); i++)
                            {
                                RowIconTextController entry = UnityEngine.Object.Instantiate(source, parent, true);
                            }
                        }
                        UIUtil.GetComponentsFromContainer(__instance.PersonalTrackRoot.transform, ____abilityIcons);
                    }
                    __instance.RecruitData = entryData;
                    __instance.RecruitName.SetSoldierData(entryData.Recruit);
                    BC_SetAbilityIcons(entryData.PersonalTrackAbilities.ToList(), ____abilityIcons);
                    if (entryData.SuppliesCost != null && __instance.CostText != null && __instance.CostColorController != null)
                    {
                        __instance.CostText.text = entryData.SuppliesCost.ByResourceType(ResourceType.Supplies).RoundedValue.ToString();
                        __instance.CostColorController.SetWarningActive(!entryData.IsAffordable, true);
                    }
                    __instance.NavHolder.RefreshNavigation();
                    return false;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return true;
                }
            }


            private static void BC_SetAbilityIcons(List<TacticalAbilityViewElementDef> abilities, List<RowIconTextController> abilityIcons)
            {
                foreach (RowIconTextController rowIconTextController in abilityIcons)
                {
                    rowIconTextController.gameObject.SetActive(false);
                }
                for (int i = 0; i < abilities.Count; i++)
                {
                    abilityIcons[i].gameObject.SetActive(true);
                    abilityIcons[i].SetController(abilities[i].LargeIcon, abilities[i].DisplayName1, abilities[i].Description);
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
