using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.UI;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Addons;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Common.View.ViewControllers.Inventory;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.InterceptionPrototype.UI;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewControllers.Inventory;
using PhoenixPoint.Geoscape.View.ViewModules;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.ActorsInstance;
using PhoenixPoint.Tactical.Entities.Effects;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Levels;
using PhoenixPoint.Tactical.Levels.FactionObjectives;
using PhoenixPoint.Tactical.View.ViewControllers;
using PhoenixPoint.Tactical.View.ViewStates;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    class VolandsPlayground
    {
        
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static void Apply_Changes()
        {
            // @Voland: play down form here
            try
            {
                //ID all the factions for later
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));

                //Source for creating new events
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));

              
                //Testing Less Pandoran Colonies
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                veryhard.NestLimitations.MaxNumber = 3;
                veryhard.NestLimitations.HoursBuildTime = 90;
                veryhard.LairLimitations.MaxNumber = 3;
                veryhard.LairLimitations.MaxConcurrent = 3;
                veryhard.LairLimitations.HoursBuildTime = 100;
                veryhard.CitadelLimitations.HoursBuildTime = 180;
                veryhard.EvolutionProgressPerDay = 50;            

                //Changes to SDI Events
                GeoscapeEventDef sdi1 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_01_GeoscapeEventDef"));
                sdi1.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI1_OUTCOME";
                GeoscapeEventDef sdi6 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_06_GeoscapeEventDef"));
                sdi6.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI6_OUTCOME";
                GeoscapeEventDef sdi16 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_16_GeoscapeEventDef"));
                sdi16.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "SDI16_OUTCOME";
                GeoscapeEventDef sdi20 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("SDI_20_GeoscapeEventDef"));
                sdi20.GeoscapeEventData.Choices[0].Outcome.GameOverVictoryFaction = null;

                //Adding peaceful option for Saving Helena
                GeoscapeEventDef savingHelenaWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE0_WIN_GeoscapeEventDef"));
                GeoscapeEventDef savingHelenaMiss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE0_MISS_GeoscapeEventDef"));
                savingHelenaMiss.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("PROG_LE0_MISS_CHOICE_2_TEXT"),
                    Requirments = new GeoEventChoiceRequirements()
                    {
                        Diplomacy = new List<GeoEventChoiceDiplomacy>()

                        {
                            new GeoEventChoiceDiplomacy ()
                            {
                            Target = GeoEventChoiceDiplomacy.DiplomacyTarget.SiteFaction,
                            Operator = GeoEventChoiceDiplomacy.DiplomacyOperator.Greater,
                            Value = 24,
                            }
                         },
                    },

                    Outcome = new GeoEventChoiceOutcome()
                    {
                        UntrackEncounters = savingHelenaWin.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        VariablesChange = savingHelenaWin.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        Cinematic = savingHelenaWin.GeoscapeEventData.Choices[0].Outcome.Cinematic,
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_LE0_MISS_CHOICE_2_OUTCOME_GENERAL")
                        },

                    }
                });

                // Add new choices to DLC1
                // Snitch to NJ
                GeoscapeEventDef prog_PU2_Choice2Event = Helper.CreateDefFromClone(sourceLoseGeoEvent, "903EB514-4807-4E1B-8650-421524C0F68E", "PROG_PU2_CHOICE2EVENT_GeoscapeEventDef");
                prog_PU2_Choice2Event.GeoscapeEventData.EventID = "PROG_PU2_CHOICE2EVENT";
                prog_PU2_Choice2Event.GeoscapeEventData.Leader = "NJ_TW";
                prog_PU2_Choice2Event.GeoscapeEventData.Title.LocalizationKey = "PROG_PU2_CHOICE2EVENT_TITLE";
                prog_PU2_Choice2Event.GeoscapeEventData.Description[0].General.LocalizationKey = "PROG_PU2_CHOICE2EVENT_TEXT_GENERAL_0";
                prog_PU2_Choice2Event.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()

                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +3
                });
                prog_PU2_Choice2Event.GeoscapeEventData.Choices[0].Outcome.Resources.Add(new ResourceUnit()

                {
                    Type = ResourceType.Materials,
                    Value = 300
                });

                //Publicly denounce NJ
                GeoscapeEventDef prog_PU2_Choice3Event = Helper.CreateDefFromClone(sourceLoseGeoEvent, "957656FF-BE70-40FA-8091-015112331970", "PROG_PU2_CHOICE3EVENT_GeoscapeEventDef");
                prog_PU2_Choice3Event.GeoscapeEventData.EventID = "PROG_PU2_CHOICE3EVENT";
                prog_PU2_Choice3Event.GeoscapeEventData.Leader = "SY_Nikolai";
                prog_PU2_Choice3Event.GeoscapeEventData.Title.LocalizationKey = "PROG_PU2_CHOICE3EVENT_TITLE";
                prog_PU2_Choice3Event.GeoscapeEventData.Description[0].General.LocalizationKey = "PROG_PU2_CHOICE3EVENT_TEXT_GENERAL_0";

                prog_PU2_Choice3Event.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });

                prog_PU2_Choice3Event.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +5
                });

                prog_PU2_Choice3Event.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = NewJericho,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });

                prog_PU2_Choice3Event.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Synedrion,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });

                //Add the choices to the event
                //New events have to be created rather than using Outcomes within each choice to replace leader pic
                GeoscapeEventDef subject24offer = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU2_GeoscapeEventDef"));
                subject24offer.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU2_CHOICE_2_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        TriggerEncounterID = "PROG_PU2_CHOICE2EVENT",
                    }
                });

                subject24offer.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU2_CHOICE_3_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        TriggerEncounterID = "PROG_PU2_CHOICE3EVENT",
                    }
                });

                //Add options for DLC1MISS WIN
                GeoscapeEventDef DLC1missWIN = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU4_WIN_GeoscapeEventDef"));

                //Anu option
                GeoscapeEventDef an28event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("AN28_GeoscapeEventDef"));
                DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.Units = an28event.GeoscapeEventData.Choices[0].Outcome.Units;
                DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "PROG_PU4_WIN_CHOICE_0_OUTCOME_GENERAL";

                //Syn choice
                DLC1missWIN.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU4_WIN_CHOICE_1_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_PU4_WIN_CHOICE_1_OUTCOME_GENERAL")
                        },
                        VariablesChange = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        UntrackEncounters = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        RemoveTimers = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.RemoveTimers,
                    },
                });
                DLC1missWIN.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Synedrion,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +3
                });
                DLC1missWIN.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = NewJericho,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +3
                });
                DLC1missWIN.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -6
                });
                DLC1missWIN.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +2
                });

                //Deny deny deny option
                DLC1missWIN.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU4_WIN_CHOICE_2_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_PU4_WIN_CHOICE_2_OUTCOME_GENERAL")
                        },
                        VariablesChange = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        UntrackEncounters = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        RemoveTimers = DLC1missWIN.GeoscapeEventData.Choices[0].Outcome.RemoveTimers,
                    },
                });
                DLC1missWIN.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Anu,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -3
                });
                DLC1missWIN.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Synedrion,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -3
                });
                DLC1missWIN.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -3
                });

                //Add options to Guided by Whispers
                //If relations with Synedrion Aligned, can opt for HD vs Pure
                //Event if HD successful
                GeoscapeEventDef sourceWinGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_WIN_GeoscapeEventDef"));
                var pu12miss = sourceWinGeoEvent.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters[0];
                GeoscapeEventDef newWinPU12 = Helper.CreateDefFromClone(sourceWinGeoEvent, "23435C5E-B933-484D-990E-5B4C0B2B32FE", "PROG_PU12_WIN2_GeoscapeEventDef");
                newWinPU12.GeoscapeEventData.EventID = "PROG_PU12WIN2";
                newWinPU12.GeoscapeEventData.Title.LocalizationKey = "PROG_PU12_WIN2_TITLE";
                newWinPU12.GeoscapeEventData.Description[0].General.LocalizationKey = "PROG_PU12_WIN2_TEXT_GENERAL_0";
                newWinPU12.GeoscapeEventData.Choices[0].Text.LocalizationKey = "PROG_PU12_WIN2_CHOICE_0_TEXT";
                newWinPU12.GeoscapeEventData.Choices[0].Outcome.Diplomacy[0] = new OutcomeDiplomacyChange()

                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    Value = 6,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                };

                //Event if HD Failure
                GeoscapeEventDef newFailPU12 = Helper.CreateDefFromClone(sourceLoseGeoEvent, "D77EB7A7-FE26-49EF-BB7A-449A51D4D519", "PROG_PU12_FAIL2_GeoscapeEventDef");
                newFailPU12.GeoscapeEventData.EventID = "PROG_PU12FAIL2";
                newFailPU12.GeoscapeEventData.Title.LocalizationKey = "PROG_PU12_FAIL2_TITLE";
                newFailPU12.GeoscapeEventData.Description[0].General.LocalizationKey = "PROG_PU12_FAIL2_TEXT_GENERAL_0";
                newFailPU12.GeoscapeEventData.Choices[0].Text.LocalizationKey = "PROG_PU12_FAIL2_CHOICE_0_TEXT";
                newFailPU12.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters.Add(pu12miss);
                newFailPU12.GeoscapeEventData.Choices[0].Outcome.RemoveTimers.Add(pu12miss);

                //New event for outcome if selling info about lab or research after stealing it by completing original mission                
                GeoscapeEventDef newPU12NJOption = Helper.CreateDefFromClone(sourceLoseGeoEvent, "D556A16F-41D8-4852-8DC4-5FB945652C50", "PROG_PU12_NewNJOption_GeoscapeEventDef");
                newPU12NJOption.GeoscapeEventData.EventID = "PROG_PU12NewNJOption";
                newPU12NJOption.GeoscapeEventData.Leader = "NJ_Abongameli";
                newPU12NJOption.GeoscapeEventData.Title.LocalizationKey = "PROG_PU12_NEWNJOPT_TITLE";
                newPU12NJOption.GeoscapeEventData.Description[0].General.LocalizationKey = "PROG_PU12_NEWNJOPT_GENERAL";
                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.Resources.Add(new ResourceUnit()
                {
                    Type = ResourceType.Materials,
                    Value = 750
                });

                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    Value = 3,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                });
                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters.Add(pu12miss);
                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.RemoveTimers.Add(pu12miss);

                //Adding options to the original event, fetching it first
                GeoscapeEventDef guidedByWhispers = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_MISS_GeoscapeEventDef"));

                //Fetching Syn HD vs Pure with protect civillians type, to use as alternative mission
                CustomMissionTypeDef havenDefPureSY_CustomMissionTypeDef = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenDefPureSY_Civ_CustomMissionTypeDef"));

                //Adding Syn Aligned options
                guidedByWhispers.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU12_MISS_CHOICE_2_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_PU12_MISS_CHOICE_2_OUTCOME_GENERAL")
                        },
                        StartMission = new OutcomeStartMission()
                        {
                            MissionTypeDef = havenDefPureSY_CustomMissionTypeDef,
                            WonEventID = "PROG_PU12WIN2",
                            LostEventID = "PROG_PU12_FAIL2"
                        }
                    },
                    Requirments = new GeoEventChoiceRequirements()
                    {
                        Diplomacy = new List<GeoEventChoiceDiplomacy>()

                        {
                            new GeoEventChoiceDiplomacy ()
                            {
                            Target = GeoEventChoiceDiplomacy.DiplomacyTarget.SiteFaction,
                            Operator = GeoEventChoiceDiplomacy.DiplomacyOperator.Greater,
                            Value = 49,
                            }
                         },
                    },
                });

                //Adding sell info to NJ option to original event
                guidedByWhispers.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU12_MISS_CHOICE_3_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        TriggerEncounterID = "PROG_PU12NewNJOption",
                    }
                });

                //Add option after winning original mission to sell research to NJ
                sourceWinGeoEvent.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU12_WIN_CHOICE_1_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        TriggerEncounterID = "PROG_PU12NewNJOption",
                    },

                });

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

                //Allow equipment before The Hatching
                CustomMissionTypeDef storyFS1_CustomMissionTypeDef = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("StoryFS1_CustomMissionTypeDef"));
                storyFS1_CustomMissionTypeDef.SkipDeploymentSelection = false;

                //Experiment new HD
                // var survive3turnsobjective = AmbushALN.CustomObjectives[0];
                // CustomMissionTypeDef ALNvsANUHD = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenDefAlienAN_CustomMissionTypeDef"));
                //CustomMissionTypeDef NewALNvsANUHD = Helper.CreateDefFromClone(AmbushALN, "C5BD29BE-2A61-4C5E-A578-F58FCB40BE14", "NewHavenDefAlienAN_CustomMissionTypeDef");
                // ALNvsANUHD.ParticipantsData[0].InfiniteReinforcements = true;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Max = 0.3f;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Min = 0.3f;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                // ALNvsANUHD.CustomObjectives[0]=survive3turnsobjective;

                //Change medbay
                //PhoenixFacilityDef medicalBay_PhoenixFacilityDef = Repo.GetAllDefs<PhoenixFacilityDef>().FirstOrDefault(ged => ged.name.Equals("MedicalBay_PhoenixFacilityDef"));
                HealFacilityComponentDef e_HealMedicalBay_PhoenixFacilityDe = Repo.GetAllDefs<HealFacilityComponentDef>().FirstOrDefault(ged => ged.name.Equals("E_Heal [MedicalBay_PhoenixFacilityDef]"));
                e_HealMedicalBay_PhoenixFacilityDe.BaseHeal = 16;

                //Bonus damage from corruption reduce to 0%
                CorruptionStatusDef corruption_StatusDef = Repo.GetAllDefs<CorruptionStatusDef>().FirstOrDefault(ged => ged.name.Equals("Corruption_StatusDef"));
                corruption_StatusDef.Multiplier = 0.0f;
            }

            catch (Exception e)
            {
                Logger.Error(e);
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

                        if (numberOfBionics==1)
                        {
                            maxCorruption -= (int)(maxCorruption * 0.33);
                        }

                        if (numberOfBionics==2)
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
                    float wpReduction = 0; // stamina > 35
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

        public static void SetStaminaToZero(GeoCharacter __instance)
        {
            try
            {
                __instance.Fatigue.Stamina.SetToMin();
            }

            catch (Exception e)
            {
                Logger.Error(e);
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

                    if (num >= 0 && num <= 100)
                    {
                        foreach (TacticalAbilityDef abilityDef in abilityList)
                        {
                            if (__instance.Progression.Abilities.Contains(abilityDef))
                            {
                                abilityList.Remove(abilityDef);
                            }
                        }

                        if (abilityList.Count >= 0)
                        {
                            __instance.Progression.AddAbility(abilityList.GetRandomElement());
                        }
                    }   
                    if (num > 100 && num <= 150)                       
                    {
                        SetStaminaToZero(__instance);
                    }
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        

        [HarmonyPatch(typeof(UIModuleMutationSection), "ApplyMutation")]
        public static class UIModuleMutationSection_ApplyMutation_SetStaminaTo0_patch
        {
            public static void Postfix(IAugmentationUIModule ____parentModule)
            {
                try
                {
                   ____parentModule.CurrentCharacter.Fatigue.Stamina.SetToMin(); 
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
                    //set Stamina to zero after installing a bionic
                    __instance.CurrentCharacter.Fatigue.Stamina.SetToMin();

                    //check number of augments the character has
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;
                    int numberOfBionics = AugmentScreenUtilities.GetNumberOfBionicsAugmentations(__instance.CurrentCharacter);
                    for (int i = 0; i < numberOfBionics; i++)
                    { 
                        float corruption = (float)(__instance.CurrentCharacter.CharacterStats.Corruption - __instance.CurrentCharacter.CharacterStats.Corruption * 0.33);
                        __instance.CurrentCharacter.CharacterStats.Corruption.Set(corruption);
                    }

                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        // Setting Stamina to zero if character suffered a disabled limb during tactical
        
        //A list of operatives that get disabled limbs. This list is cleared when the game is exited, so saving a game in tactical, exiting the game and reloading will probably make the game "forget" the character was ever injured.
        public static List<int> charactersWithBrokenLimbs = new List<int>();

        // This first patch is to "register" the injury in the above list
        [HarmonyPatch(typeof(BodyPartAspect), "OnSetToDisabled")]
        internal static class BodyPartAspect_OnSetToDisabled_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(BodyPartAspect __instance)
            {
                // The way to get access to base.OwnerItem
                // 'base' it the class this object is derived from and with Harmony we can't directly access these base classes
                // looking in dnSpy we can see, that 'base' is of type 'TacticalItemAspectBase' and we want to access it's property 'OwnerItem' that is of type 'TacticalItem'
                // 'AccessTools.Property' are tools from Harmony to make such an access easier, the usual way through Reflections is a bit more complicated.
                TacticalItem base_OwnerItem = (TacticalItem)AccessTools.Property(typeof(TacticalItemAspectBase), "OwnerItem").GetValue(__instance, null);

                if (!charactersWithBrokenLimbs.Contains(base_OwnerItem.TacticalActorBase.GeoUnitId))
                {
                    charactersWithBrokenLimbs.Add(base_OwnerItem.TacticalActorBase.GeoUnitId);
                }
            }
        }

       // This second patch reads from the list and drains Stamina from everyone who is in it. 
        [HarmonyPatch(typeof(GeoCharacter), "Init")]
        internal static class GeoCharacter_Init_StaminaToZeroIfBodyPartDisabled_patch
        {
            public static void Postfix(GeoCharacter __instance)
            {
                try
                {
                  
                        if (charactersWithBrokenLimbs.Contains(__instance.Id))
                        {
                            SetStaminaToZero(__instance);
                            charactersWithBrokenLimbs.Remove(__instance.Id);
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
    }

}

