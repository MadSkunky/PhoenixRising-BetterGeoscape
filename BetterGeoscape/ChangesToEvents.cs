using Base.Defs;
using Base.UI;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class ChangesToEvents
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void Apply_Changes()
        {
            try
            {

                //ID all the factions for later
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));

                //Source for creating new events
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));

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


            }
            catch (Exception e)
            {
             Logger.Error(e);
            }

        }

    }
}
