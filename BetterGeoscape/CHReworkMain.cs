using AK.Wwise;
using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.Eventus;
using Base.Eventus.Filters;
using Base.UI;
using Base.UI.MessageBox;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Geoscape.Achievements;
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
using PhoenixPoint.Geoscape.View.ViewControllers;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewModules;
using PhoenixPoint.Geoscape.View.ViewStates;
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

      //  public static string darkEventDescription;
      //  public static string darkEventTitle;

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

        public static readonly string[] DarkEvents_Title = new string[]
        {
        "DARK_EVENT_TITLE_01","DARK_EVENT_TITLE_02","DARK_EVENT_TITLE_03","DARK_EVENT_TITLE_04","DARK_EVENT_TITLE_05","DARK_EVENT_TITLE_06",
        "DARK_EVENT_TITLE_07","DARK_EVENT_TITLE_08","DARK_EVENT_TITLE_09","DARK_EVENT_TITLE_10","DARK_EVENT_TITLE_11","DARK_EVENT_TITLE_12",
        "DARK_EVENT_TITLE_13","DARK_EVENT_TITLE_14","DARK_EVENT_TITLE_15","DARK_EVENT_TITLE_16","DARK_EVENT_TITLE_17","DARK_EVENT_TITLE_18",
        "DARK_EVENT_TITLE_19","DARK_EVENT_TITLE_20",
        };
        public static readonly string[] DarkEvents_Description = new string[]
        {
        "DARK_EVENT_DESCRIPTION_TEXT_01","DARK_EVENT_DESCRIPTION_TEXT_02","DARK_EVENT_DESCRIPTION_TEXT_03","DARK_EVENT_DESCRIPTION_TEXT_04",
        "DARK_EVENT_DESCRIPTION_TEXT_05","DARK_EVENT_DESCRIPTION_TEXT_06","DARK_EVENT_DESCRIPTION_TEXT_07","DARK_EVENT_DESCRIPTION_TEXT_08",
        "DARK_EVENT_DESCRIPTION_TEXT_09","DARK_EVENT_DESCRIPTION_TEXT_10","DARK_EVENT_DESCRIPTION_TEXT_11","DARK_EVENT_DESCRIPTION_TEXT_12",
        "DARK_EVENT_DESCRIPTION_TEXT_13","DARK_EVENT_DESCRIPTION_TEXT_14","DARK_EVENT_DESCRIPTION_TEXT_15","DARK_EVENT_DESCRIPTION_TEXT_16",
        "DARK_EVENT_DESCRIPTION_TEXT_17","DARK_EVENT_DESCRIPTION_TEXT_18","DARK_EVENT_DESCRIPTION_TEXT_19","DARK_EVENT_DESCRIPTION_TEXT_20",
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

                }

                else

                    if (CurrentODI_Level != geoLevelController.EventSystem.GetVariable("BC_SDI", -1))
                {
                    // Get the Event ID from array dependent on calculated level index

                    string eventID = ODI_EventIDs[CurrentODI_Level];
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(geoAlienFaction, geoLevelController.ViewerFaction);
                    GeoscapeEventDef oDIEventToTrigger = geoLevelController.EventSystem.GetEventByID(ODI_EventIDs[CurrentODI_Level]);

                    //Need to fix a broken SDI event!
                    if(oDIEventToTrigger.GeoscapeEventData.EventID== "SDI_07") 
                    { 
                    oDIEventToTrigger.GeoscapeEventData.Choices= Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_06_GeoscapeEventDef")).GeoscapeEventData.Choices;
                    }

                    // Dark Events roll
                    // Before the roll, Dark Event has not been rolled
                    bool darkEventRolled = false;
                    int darkEventRoll = 0;
                    // We want this in case a Dark Event is taken out of play (because replaced by another)
                    int darkEventReplaced = 0;
                    // Create variable reflecting difficulty level, 1 being the easiest, and 4 the hardest
                    // This will determine amount of possible simultaneous dark events
                    int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                    string triggeredVoidOmens = "TriggeredVoidOmen_";
                    string voidOmen = "VoidOmen_";

                    if (geoLevelController.EventSystem.GetVariable("BC_SDI") > 0)
                    {              
                        // Here comes the roll, for testing purposes with 1/3 chance of no DE happening    
                        int roll = UnityEngine.Random.Range(0, 11);
                        if (roll > 0 && roll < 11)
                        { 
                            // If a Dark Event rolls

                            // Create list of dark events currently implemented
                            List<int> darkEvents = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                            // Array to track how many Dark Events have already appeared (will get filled up later)
                            int[] alreadyRolledDarkEvents = new int[19];
                            int countI = 0;
                            // This is the Voland loop, the loop you do when you don't know any better
                            // The loop will try a 100 times if necessary to get a valid random dark event (one that has not been in play before)                                                 
                            for (int i = 0; i < 100; i++)
                            {
                                
                                // Get a random dark event from the Dark Events list
                                darkEventRoll = darkEvents.GetRandomElement();
                                countI++;
                                // Check if this event has already appeared 
                                for (int j = 1; j < 19; j++)
                                {
                                    // There are 19 variables documenting what ODI event accompanied which Dark Event, the "TriggeredVoidOmens[1-19]"
                                    // If the dark event chosen at random has been triggered before, it will be added to the alreadyRolledDarkEvents array
                                    if (geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + j) == darkEventRoll)
                                    {
                                        alreadyRolledDarkEvents[j] = darkEventRoll;
                                    }
                                }
                                Logger.Always("The Void Omen rolled is_" + darkEventRoll);
                                // If the randomly chosen Dark Event has not appeared yet, make it happen!     
                                if (!alreadyRolledDarkEvents.Contains(darkEventRoll))
                                {
                                    // We can have as many simulateneous Dark Events in play as the mathematical expression of the difficulty level
                                    for (int t = 0; t < difficulty; t++)
                                    {
                                        // There will be as many Dark Event variables (each storing an active Dark Event) as the ME of the difficulty level
                                        // The first empty Dark Event variable will receive the new Dark Event
                                        if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - t)) == 0)
                                        {
                                            // This is the regular code to modify a Def, in this case the ODI event to which the Dark Event will be attached,
                                            // so that it sets the Dark Event variable
                                            oDIEventToTrigger.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange
                                            {
                                                VariableName = voidOmen + (difficulty - t),
                                                Value = { Min = darkEventRoll, Max = darkEventRoll },
                                                IsSetOperation = true,
                                            });
                                            // This records which ODI event triggered which Dark Event
                                            geoLevelController.EventSystem.SetVariable(triggeredVoidOmens + CurrentODI_Level, darkEventRoll);
                                            // Raise the flag, we have a Dark Event!
                                            darkEventRolled = true;
                                            // Then close both loops:
                                            t = 4;
                                            i = 100;
                                        }
                                        // If that Dark Event variable is already used, we will record it in our array, by assigning 1 to the position
                                        /* else
                                         {
                                             array[difficulty - 1 - t] = 1;
                                         }
                                         */
                                    }
                                    // If we managed to roll a Dark Event, because we found a dark event not in use and we found a variable to log it in,
                                    // the Voland loop ends here
                                    if (darkEventRolled)
                                    {

                                    }
                                    // But if all the Dark Event variables are already in use, we have to find the earliest TriggeredDarkEvent still in play
                                    // to replace it with the new darkevent
                                    
                                    else if (darkEventRolled == false)
                                    {
                                        Logger.Always("The i loop was done " + countI + " times");
                                        // So we create a new array and a new loop to record all the Dark Events already rolled.
                                        int[] allTheDarkEventsAlreadyRolled = new int[19];
                                        // And an array to record which variables hold which Dark Events
                                        int[] allTheDarkEventsVariables = new int[difficulty];

                                        for (int x = 1; x < 20; x++)
                                        {
                                            if (geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x) != 0)
                                            {
                                                allTheDarkEventsAlreadyRolled[x] = geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x);
                                                Logger.Always("Check Triggered Void Omens " + allTheDarkEventsAlreadyRolled[x]);
                                            }
                                        }
                                        int countVoidOmensY = 0;
                                        int countVoidOmensX = 0;
                                        int variablesUsed = 0;
                                        // Then we check our Dark Event variables to see which one has the earliest Dark Event already rolled                                
                                        for (int x = 1; x < 20; x++)
                                        {
                                            
                                            // We will look through the DarkEvents variables in the order in which they were filled
                                            for (int y = 0; y < difficulty; y++)
                                            {
                                                
                                                // And record which variable holds which Dark Event
                                                if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)) == allTheDarkEventsAlreadyRolled[x])
                                                {
                                                    allTheDarkEventsVariables[difficulty - y - 1] = allTheDarkEventsAlreadyRolled[x];
                                                    Logger.Always("Check Variable " + (difficulty - y) + " holding Void Omen " + allTheDarkEventsVariables[difficulty - y - 1]);
                                                    variablesUsed++;
                                                }
                                                
                                                countVoidOmensY++;
                                            }
                                            Logger.Always("the count of variables used is " + variablesUsed);
                                            if (variablesUsed==difficulty)
                                            {
                                                x = 20;
                                            }
                                            countVoidOmensX++;
                                        }
                                        Logger.Always("The y loop was done " + countVoidOmensY);
                                        Logger.Always("The x loop was done " + countVoidOmensX);
                                        // Then we try to find in the array of the Dark Variables which one appeared the earliest
                                        int xCounter = 0;
                                        for (int x = 1; x < 20; x++)
                                        {
                                            xCounter++;
                                            // We check, starting from the earliest, which Dark Event is still in play
                                            if (allTheDarkEventsVariables.Contains(geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x)))
                                            {
                                                
                                                // Then we locate in which Variable it is recorded
                                                for (int y = 0; y < difficulty; y++)
                                                {
                                                    
                                                    // Once we find it, that's where we want to put our new Dark Event
                                                    if (allTheDarkEventsVariables[difficulty - y - 1] == geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x))
                                                    {
                                                        
                                                        darkEventReplaced = allTheDarkEventsVariables[difficulty - y - 1];
                                                        Logger.Always("The Void Omen that will be replaced is "+ darkEventReplaced);
                                                        oDIEventToTrigger.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange
                                                        {
                                                            VariableName = voidOmen + (difficulty - y),
                                                            Value = { Min = darkEventRoll, Max = darkEventRoll },
                                                            IsSetOperation = true,
                                                        });
                                                        Logger.Always("The Void Omen Variable we are using is " + voidOmen + (difficulty-y));
                                                        geoLevelController.EventSystem.SetVariable(triggeredVoidOmens + CurrentODI_Level, darkEventRoll);
                                                        // And the flag is raised here too!
                                                        darkEventRolled = true;
                                                        Logger.Always("Void Omen rolled "+ darkEventRolled);
                                                        // Close the loops when you leave!
                                                        y = 5;
                                                        x = 20;
                                                        i = 100;
                                                        Logger.Always("Everything working in this method");
                                                        Logger.Always("This x loop was done " + xCounter + " times");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // The ODI event is triggered
                    geoLevelController.EventSystem.TriggerGeoscapeEvent(ODI_EventIDs[CurrentODI_Level], geoscapeEventContext);
                    geoLevelController.EventSystem.SetVariable("BC_SDI", CurrentODI_Level);
                    //UpdateODITracker(CurrentODI_Level, geoLevelController); not used currently, because clogs the UI
                    // And if a Dark Event has been rolled, a Dark Event will appear
                    if(darkEventRolled && geoLevelController.EventSystem.GetVariable(voidOmen + difficulty) == darkEventRoll && geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - 1)) == 0) 
                    {
                        GeoscapeEventDef voidOmenIntro = geoLevelController.EventSystem.GetEventByID("VoidOmen");
                        voidOmenIntro.GeoscapeEventData.Title.LocalizationKey = "VOID_OMEN_INTRO_TITLE";
                        voidOmenIntro.GeoscapeEventData.Description[0].General.LocalizationKey = "VOID_OMEN_INTRO";
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("VoidOmen", geoscapeEventContext);
                    } 

                    if (darkEventRolled)
                    {
                        string title = (string)DarkEvents_Title.GetValue(darkEventRoll-1);
                        string description = (string)DarkEvents_Description.GetValue(darkEventRoll-1);
                        GeoscapeEventDef darkEvent = geoLevelController.EventSystem.GetEventByID("DarkEvent");
                        darkEvent.GeoscapeEventData.Title.LocalizationKey = title;
                        darkEvent.GeoscapeEventData.Description[0].General.LocalizationKey = description;
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("DarkEvent", geoscapeEventContext);
                        CreateDarkEventObjective(title, description, geoLevelController);

                        if (darkEventReplaced != 0) 
                        {
                            string objectiveToBeReplaced = (string)DarkEvents_Title.GetValue(darkEventReplaced - 1);                                
                            Logger.Always("The target event that will be replaced is " + objectiveToBeReplaced);
                            RemoveDarkEventObjective(objectiveToBeReplaced, geoLevelController);
                            darkEventReplaced= 0;
                        } 
                    }                     
                }
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void CreateDarkEventObjective(string title, string description, GeoLevelController level)
        {
            try
            {

                DiplomaticGeoFactionObjective darkEventObjective = new DiplomaticGeoFactionObjective(level.PhoenixFaction, level.PhoenixFaction)
            {
                Title = new LocalizedTextBind(title),
                Description = new LocalizedTextBind(description),
            };
           level.PhoenixFaction.AddObjective(darkEventObjective);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        [HarmonyPatch(typeof(GeoFactionObjective), "GetIcon")]
        internal static class BG_GeoFactionObjective_GetIcon_patch
        {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
        private static void Postfix(ref Sprite __result, GeoFactionObjective __instance)
        {
                try
                {
                    if (__instance.Title!=null && __instance.Title.LocalizationKey.Contains("DARK_EVENT_TITLE_"))
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
                        if (DarkEvents_Title.Contains(objectiveText))
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


        public static void RemoveDarkEventObjective(string title, GeoLevelController level)
        {
            try
            {
                DiplomaticGeoFactionObjective darkEventObjective = 
            (DiplomaticGeoFactionObjective)level.PhoenixFaction.Objectives.FirstOrDefault(ged => ged.Title.LocalizationKey.Equals(title));
            string checktitle = darkEventObjective.GetTitle();
            Logger.Always("the title in the RemoveDarkEventObjective method is " + title);
           // Logger.Always("the localizedTextBind in the RemoveDarkEventObjective method is " + localizedTextBind);
            Logger.Always("if we found the objective, there should be something here " + checktitle);
            level.PhoenixFaction.RemoveObjective(darkEventObjective);
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
                            if (odiPerc < 75)
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
                    else if(character.Fatigue.Stamina <40 && character.Fatigue.Stamina >=30)
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
                        __result = $"<color=#da5be3>{currentAttributeValue - ____character.CharacterStats.Corruption.Value + CalculateStaminaEffectOnDelirium(____character)}</color>" + $"({currentAttributeValue}) / "  +
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
                            for(int i = 0; i < CalculateStaminaEffectOnDelirium(____character); i++) 
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
                    // GeoscapeTutorialStepsDef stepTest = Repo.GetAllDefs<GeoscapeTutorialStepsDef>().FirstOrDefault(ged => ged.name.Equals("GeoscapeTutorialStepsDef"));
                   // GeoscapeTutorialStep test = new GeoscapeTutorialStep();
                   // test.Title.LocalizationKey = $"test";
                   // test.Description.LocalizationKey = $"testing";


                    if (num >= 0 && num <= 50)
                    {
                        for (int i = 0; i < 100; i++)
                        { 
                           TacticalAbilityDef abilityToAdd=abilityList.GetRandomElement();
                           if (!__instance.Progression.Abilities.Contains(abilityToAdd)) 
                           {
                                
                                __instance.Progression.AddAbility(abilityToAdd);
                                //__instance.Faction.GeoLevel.View.GeoscapeModules.TutorialModule.SetTutorialStep(test, false);
                                GameUtl.GetMessageBox().ShowSimplePrompt($"{__instance.GetName()}"+" got a nasty perk, called " + $"<b>{abilityToAdd.ViewElementDef.DisplayName1.LocalizationKey}</b>" + "\n\n"+$"<i>{ abilityToAdd.ViewElementDef.Description.LocalizationKey}</i>", MessageBoxIcon.None, MessageBoxButtons.OK, null);

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
            public static void Postfix(TacticalLevelController level)//, GeoLevelController __level)
            {
                DefRepository Repo = GameUtl.GameComponent<DefRepository>();
                try
                {

                /*    PassiveModifierAbilityDef shutEye_Ability = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(ged => ged.name.Equals("ShutEye_AbilityDef"));
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

                    if (__level.EventSystem.GetVariable("CorruptionActive") == 0 && __level.EventSystem.GetVariable("PandoraVirus") == 1)
                    {
                       foreach (TacticalFaction faction in level.Factions)
                        {
                            if (faction.IsViewerFaction)
                            {
                                foreach (TacticalActor actor in faction.TacticalActors)
                                {                 
                                    foreach (TacticalAbilityDef tacticalAbility in abilityList)
                                    {
                                        actor.RemoveAbility(tacticalAbility);
                                    }
                                }
                            }
                        }
                    }

                    else

                      */  foreach (TacticalFaction faction in level.Factions)
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
