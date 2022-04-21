using AK.Wwise;
using Assets.Code.PhoenixPoint.Geoscape.Entities.Sites.TheMarketplace;
using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Statuses;
using Base.Eventus;
using Base.Eventus.Filters;
using Base.UI;
using Base.UI.MessageBox;
using Base.Utils;
using Base.Utils.GameConsole;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Abilities;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.Missions.Outcomes;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Entities.Research.Requirement;
using PhoenixPoint.Geoscape.Entities.Research.Reward;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Conditions;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Events.Eventus.Filters;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewStates;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.ActorsInstance;
using PhoenixPoint.Tactical.Entities.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    class VolandsPlayground
    {
        

        // Get config, definition repository and shared data

        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        private static bool ApplyChangeReduceResources = true;

        private static bool ApplyChangeDiplomacy = true;

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

                //Testing MadSkunky reduce rewards by 25%
                if (ApplyChangeReduceResources)
                {

                    foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                    {
                        foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                        {
                            if (choice.Outcome.Resources != null && !choice.Outcome.Resources.IsEmpty)
                            {
                                choice.Outcome.Resources *= 0.8f;
                            }
                        }
                    }
                    ApplyChangeReduceResources = false;
                }
                //Testing increasing diplomacy penalties 
                GeoPhoenixFactionDef geoPhoenixFaction = Repo.GetAllDefs<GeoPhoenixFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));

                if (ApplyChangeDiplomacy)
                {
                    foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                    {
                        
                        if (geoEvent.GeoscapeEventData.EventID != "PROG_PU4_WIN" 
                            && geoEvent.GeoscapeEventData.EventID != "PROG_SY7"
                            && geoEvent.GeoscapeEventData.EventID != "PROG_SY8"
                            && geoEvent.GeoscapeEventData.EventID != "PROG_AN3"
                            && geoEvent.GeoscapeEventData.EventID != "PROG_AN5"
                            && geoEvent.GeoscapeEventData.EventID != "PROG_NJ7"
                            && geoEvent.GeoscapeEventData.EventID != "PROG_NJ8")
                        {
                            foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                        {
                            for (int i = 0; i < choice.Outcome.Diplomacy.Count; i++)
                            {
                                if (choice.Outcome.Diplomacy[i].TargetFaction == geoPhoenixFaction && choice.Outcome.Diplomacy[i].Value <= 0)
                                {
                                    OutcomeDiplomacyChange diplomacyChange = choice.Outcome.Diplomacy[i];
                                    diplomacyChange.Value *= 2;
                                    choice.Outcome.Diplomacy[i] = diplomacyChange;
                                }
                            }
                        }
                        }
                    }
                    ApplyChangeDiplomacy = false;
                }

                //Increase diplo penalties in 25, 50 and 75 diplo missions
                GeoscapeEventDef ProgAnuSupportive = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_AN2_WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgNJSupportive = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_NJ1_WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgSynSupportivePoly = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY1_WIN1_GeoscapeEventDef"));
                GeoscapeEventDef ProgSynSupportiveTerra = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY1_WIN2_GeoscapeEventDef"));
                GeoscapeEventDef ProgAnuPact = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_AN4_WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgNJPact = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_NJ2__WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgSynPact = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY3_WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgAnuAlliance1 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_AN6_WIN1_GeoscapeEventDef"));
                GeoscapeEventDef ProgAnuAlliance2 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_AN6_WIN2_GeoscapeEventDef"));
                GeoscapeEventDef ProgNJAlliance = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_NJ3_WIN_GeoscapeEventDef"));
                GeoscapeEventDef ProgSynAlliancePoly = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY4_WIN1_GeoscapeEventDef"));
                GeoscapeEventDef ProgSynAllianceTerra = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY4_WIN2_GeoscapeEventDef"));


                ProgAnuSupportive.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgAnuSupportive.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgAnuPact.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgAnuPact.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgAnuAlliance1.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgAnuAlliance2.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -20
                });

                ProgAnuAlliance2.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgSynSupportivePoly.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgSynSupportivePoly.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "Polyphonic",
                    Value = { Min = 1, Max = 1 },
                    IsSetOperation = false,
                });

                ProgSynSupportivePoly.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });

                ProgSynSupportivePoly.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgSynSupportivePoly.GeoscapeEventData.Choices[1].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "Terraformers",
                    Value = { Min = 1, Max = 1 },
                    IsSetOperation = false,
                });

                ProgSynSupportiveTerra.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgSynSupportiveTerra.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "Terraformers",
                    Value = { Min = 1, Max = 1 },
                    IsSetOperation = false,
                }); ;

                ProgSynSupportiveTerra.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });

                ProgSynSupportiveTerra.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgSynSupportiveTerra.GeoscapeEventData.Choices[1].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "Polyphonic",
                    Value = { Min = 1, Max = 1 },
                    IsSetOperation = false,
                });

                ProgSynPact.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()

                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -18
                });

                ProgSynPact.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -18
                });

                ProgSynPact.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgSynPact.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgSynAlliancePoly.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()

                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -20
                });

                ProgSynAllianceTerra.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()

                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -20
                });

                ProgNJSupportive.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgNJSupportive.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -10
                });

                ProgNJPact.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgNJPact.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -15
                });

                ProgNJAlliance.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -20
                });

                OutcomeDiplomacyChange outcomeDiplomacyChange = ProgNJAlliance.GeoscapeEventData.Choices[0].Outcome.Diplomacy[1];
                outcomeDiplomacyChange.Value = -20;

                //Change Reward introductory mission Synedrion
                GeoscapeEventDef ProgSynIntroWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_SY0_WIN_GeoscapeEventDef"));
                ProgSynIntroWin.GeoscapeEventData.Choices[1].Outcome.Diplomacy[0] = new OutcomeDiplomacyChange
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = 0
                };

                //Testing Less Pandoran Colonies
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                veryhard.NestLimitations.MaxNumber = 3;
                veryhard.NestLimitations.HoursBuildTime = 90;
                veryhard.LairLimitations.MaxNumber = 3;
                veryhard.LairLimitations.MaxConcurrent = 3;
                veryhard.LairLimitations.HoursBuildTime = 100;
                veryhard.CitadelLimitations.HoursBuildTime = 180;

                // KE Story rework - remove missions + Maker
                GeoscapeEventDef geoEventFS9 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_FS9_GeoscapeEventDef"));
                GeoscapeEventDef KE1MissWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE1_WIN_GeoscapeEventDef"));
                GeoscapeEventDef KE2MissWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE2_WIN_GeoscapeEventDef"));
                GeoscapeEventDef KE3MissWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE3_WIN_GeoscapeEventDef"));
                GeoscapeEventDef KE4MissWin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE4_WIN_GeoscapeEventDef"));
                GeoscapeEventDef KE1Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE1_GeoscapeEventDef"));
                GeoscapeEventDef KE2Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE2_GeoscapeEventDef"));
                GeoscapeEventDef KE3Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE3_GeoscapeEventDef"));
                GeoscapeEventDef KE4Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE4_GeoscapeEventDef"));


                KE1Miss.GeoscapeEventData.Choices[0] = KE1MissWin.GeoscapeEventData.Choices[0];
                KE1MissWin.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters.Add("PROG_KE1");
                KE1Miss.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "PROG_KE1_TEXT_OUTCOME_0";
                KE1Miss.GeoscapeEventData.Choices[0].Text.LocalizationKey = "PROG_KE1_CHOICE_0_TEXT";
                KE1Miss.GeoscapeEventData.Choices.Remove(KE1Miss.GeoscapeEventData.Choices[1]);
                //Increase marketplace variable after completing first DLC1 mission
                GeoscapeEventDef dlc1miss1win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU4_WIN_GeoscapeEventDef"));
                dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "NumberOfDLC5MissionsCompletedVariable",
                    Value = KE1MissWin.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0].Value,
                    IsSetOperation = false,

                });
                //Increase marketplace variable after destroying Bionic Fortress
                GeoscapeEventDef dlc1missfinalwin = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU14_WIN_GeoscapeEventDef"));
                dlc1missfinalwin.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(new OutcomeVariableChange()
                {
                    VariableName = "NumberOfDLC5MissionsCompletedVariable",
                    Value = KE2MissWin.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0].Value,
                    IsSetOperation = false,

                });

                TheMarketplaceSettingsDef theMarketplaceSettings = Repo.GetAllDefs<TheMarketplaceSettingsDef>().FirstOrDefault(ged => ged.name.Equals("TheMarketplaceSettingsDef"));
                theMarketplaceSettings.TheMarketplaceItemPriceMultipliers[1].PriceMultiplier = 2.5f;
                theMarketplaceSettings.TheMarketplaceItemPriceMultipliers[2].PriceMultiplier = 2;
                theMarketplaceSettings.TheMarketplaceItemPriceMultipliers[3].PriceMultiplier = 1.5f;
                theMarketplaceSettings.Missions[1] = null; theMarketplaceSettings.Missions[2] = null; theMarketplaceSettings.Missions[3] = null;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[1].MinNumberOfOffers = 10;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[1].MinNumberOfOffers = 12;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[2].MinNumberOfOffers = 13;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[2].MinNumberOfOffers = 15;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[3].MinNumberOfOffers = 16;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[3].MinNumberOfOffers = 20;

                //Replace all LOTA Schemata missions with KE2 mission
                GeoscapeEventDef LE1Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_MISS_GeoscapeEventDef"));
                LE1Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = KE2Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef;
                //Don't generate next Schemata mission
                GeoscapeEventDef LE1Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_WIN_GeoscapeEventDef"));
                //GeoscapeEventDef geoEventFS9 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_FS9_GeoscapeEventDef"));
                LE1Win.GeoscapeEventData.Choices[0].Outcome.SetEvents = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.SetEvents;
                LE1Win.GeoscapeEventData.Choices[0].Outcome.TrackEncounters = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.TrackEncounters;
                //Unlock all ancient weapons research and add hidden variable to unlock final cinematic
                GeoscapeEventDef LE2Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE2_WIN_GeoscapeEventDef"));
                GeoscapeEventDef LE3Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE3_WIN_GeoscapeEventDef"));
                GeoscapeEventDef LE4Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE4_WIN_GeoscapeEventDef"));
                GeoscapeEventDef LE5Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE5_WIN_GeoscapeEventDef"));
                GeoscapeEventDef LE6Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE6_WIN_GeoscapeEventDef"));
                OutcomeVariableChange Schemata2Res = LE2Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0];
                OutcomeVariableChange Schemata3Res = LE3Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0];
                OutcomeVariableChange Schemata4Res = LE4Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0];
                OutcomeVariableChange Schemata5Res = LE5Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0];
                OutcomeVariableChange Schemata6Res = LE6Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[0];
                OutcomeVariableChange var6LE = LE6Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange[1];
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(Schemata2Res);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(Schemata3Res);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(Schemata4Res);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(Schemata5Res);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(Schemata6Res);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange.Add(var6LE);
                //Remove 50 SP
                LE1Win.GeoscapeEventData.Choices[0].Outcome.FactionSkillPoints = 0;
                LE1Win.GeoscapeEventData.Leader = "Jack_Harlson01";
                //Require capturing ancient site for LOTA Schemata missions
                //GeoscapeEventDef LE1Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_GeoscapeEventDef"));
                //GeoscapeEventDef LEFinalEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE_FINAL_GeoscapeEventDef"));
                //GeoLevelConditionDef sourceCondition = Repo.GetAllDefs<GeoLevelConditionDef>().FirstOrDefault(ged => ged.name.Equals("[PROG_LE_FINAL] Condition 1"));
                //GeoLevelConditionDef newCondition = Helper.CreateDefFromClone(sourceCondition, "0358D502-421D-4D9A-9505-491FC80F1C56", "[PROG_LE_1] Condition 2");
                //newCondition.VariableCompareToNumber = 1;
                //LE1Event.GeoscapeEventData.Conditions.Add(newCondition);
                //Add choices for LE1Win

                LE1Win.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("PROG_LE1_WIN_CHOICE_1_TEXT"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        UntrackEncounters = LE1Win.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        VariablesChange = LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_LE1_WIN_CHOICE_1_OUTCOME_GENERAL")
                        },
                        SetEvents = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.SetEvents,
                        TrackEncounters = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.TrackEncounters,
                        FactionSkillPoints = 0
                    }
                });
                LE1Win.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("PROG_LE1_WIN_CHOICE_2_TEXT"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        UntrackEncounters = LE1Win.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        VariablesChange = LE1Win.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_LE1_WIN_CHOICE_2_OUTCOME_GENERAL")
                        },
                        SetEvents = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.SetEvents,
                        TrackEncounters = geoEventFS9.GeoscapeEventData.Choices[0].Outcome.TrackEncounters,
                        FactionSkillPoints = 0
                    }
                });
                LE1Win.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "PROG_LE1_WIN_CHOICE_0_OUTCOME_GENERAL";
                TacCharacterDef armadillo = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Armadillo_CharacterTemplateDef"));
                LE1Win.GeoscapeEventData.Choices[0].Outcome.Units.Add(armadillo);
                LE1Win.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -8

                });
                LE1Win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +8
                });
                LE1Win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -8
                });
                LE1Win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Anu,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -16
                });

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
                dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.Units = an28event.GeoscapeEventData.Choices[0].Outcome.Units;
                dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "PROG_PU4_WIN_CHOICE_0_OUTCOME_GENERAL";

                //Syn choice
                dlc1miss1win.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind ("PROG_PU4_WIN_CHOICE_1_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_PU4_WIN_CHOICE_1_OUTCOME_GENERAL")
                        },
                    VariablesChange = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                    UntrackEncounters = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                    RemoveTimers = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.RemoveTimers,
                    },
                });
                dlc1miss1win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Synedrion,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +3
                });
                dlc1miss1win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = NewJericho,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +3
                });
                dlc1miss1win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -6
                });
                dlc1miss1win.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +2
                });

                //Deny deny deny option
                dlc1miss1win.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = (new LocalizedTextBind("PROG_PU4_WIN_CHOICE_2_TEXT")),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("PROG_PU4_WIN_CHOICE_2_OUTCOME_GENERAL")
                        },
                        VariablesChange = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.VariablesChange,
                        UntrackEncounters = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.UntrackEncounters,
                        RemoveTimers = dlc1miss1win.GeoscapeEventData.Choices[0].Outcome.RemoveTimers,
                    },
                });
                dlc1miss1win.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Anu,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -3
                });
                dlc1miss1win.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = Synedrion,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -3
                });
                dlc1miss1win.GeoscapeEventData.Choices[2].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
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
                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.Resources.Add (new ResourceUnit () 
                {
                    Type = ResourceType.Materials, Value = 750
                });

                newPU12NJOption.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add (new OutcomeDiplomacyChange()
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
                AmbushALN.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushALN.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushALN.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushALN.CratesDeploymentPointsRange.Min = 30;
                AmbushALN.CratesDeploymentPointsRange.Max = 50;
                AmbushALN.CustomObjectives[2]=pickResourceCratesObjective;

                CustomMissionTypeDef AmbushAN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAN_CustomMissionTypeDef"));
                AmbushAN.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushAN.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushAN.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushAN.CratesDeploymentPointsRange.Min = 30;
                AmbushAN.CratesDeploymentPointsRange.Max = 50;
                AmbushAN.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushBandits = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushBandits_CustomMissionTypeDef"));
                AmbushBandits.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushBandits.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushBandits.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushBandits.CratesDeploymentPointsRange.Min = 30;
                AmbushBandits.CratesDeploymentPointsRange.Max = 50;
                AmbushBandits.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushFallen = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushFallen_CustomMissionTypeDef"));
                AmbushFallen.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushFallen.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushFallen.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushFallen.CratesDeploymentPointsRange.Min = 30;
                AmbushFallen.CratesDeploymentPointsRange.Max = 50;
                AmbushFallen.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushNJ = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushNJ_CustomMissionTypeDef"));
                AmbushNJ.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushNJ.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushNJ.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushNJ.CratesDeploymentPointsRange.Min = 30;
                AmbushNJ.CratesDeploymentPointsRange.Max = 50;
                AmbushNJ.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushPure = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushPure_CustomMissionTypeDef"));
                AmbushPure.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushPure.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushPure.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushPure.CratesDeploymentPointsRange.Min = 30;
                AmbushPure.CratesDeploymentPointsRange.Max = 50;
                AmbushPure.CustomObjectives[2] = pickResourceCratesObjective;

                CustomMissionTypeDef AmbushSY = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushSY_CustomMissionTypeDef"));
                AmbushSY.CratesDeploymentPointsRange = SourceScavCratesALN.CratesDeploymentPointsRange;
                AmbushSY.MissionSpecificCrates = SourceScavCratesALN.MissionSpecificCrates;
                AmbushSY.FactionItemsRange = SourceScavCratesALN.FactionItemsRange;
                AmbushSY.CratesDeploymentPointsRange.Min = 30;
                AmbushSY.CratesDeploymentPointsRange.Max = 50;
                AmbushSY.CustomObjectives[2] = pickResourceCratesObjective;

                //Experiment new HD
                var survive3turnsobjective = AmbushALN.CustomObjectives[0];
                CustomMissionTypeDef ALNvsANUHD = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenDefAlienAN_CustomMissionTypeDef"));
                //CustomMissionTypeDef NewALNvsANUHD = Helper.CreateDefFromClone(AmbushALN, "C5BD29BE-2A61-4C5E-A578-F58FCB40BE14", "NewHavenDefAlienAN_CustomMissionTypeDef");
                ALNvsANUHD.ParticipantsData[0].InfiniteReinforcements = true;
                ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Max = 0.3f;
                ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Min = 0.3f;
                ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                ALNvsANUHD.CustomObjectives[0]=survive3turnsobjective;
                           

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

                    // Calculate the percentage of current ODI level, these two variables are globally set by our ODI event patches
                    int odiPerc = CurrentODI_Level * 100 / ODI_EventIDs.Length;

                    // Get max corruption dependent on max WP of the selected actor
                    int maxCorruption = 0;
                    if (odiPerc < 25)
                    {
                        maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax / 3;
                    }
                    else
                    {
                        if (odiPerc < 75)
                        {
                            maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax * 1 / 2;
                        }
                        else // > 75%
                        {
                            maxCorruption = base_TacticalActor.CharacterStats.Willpower.IntMax;
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

        
        // Harmony patch before Geoscape world is created
        [HarmonyPatch(typeof(GeoInitialWorldSetup), "SimulateFactions")]
        internal static class BC_GeoInitialWorldSetup_SimulateFactions_Patch
        {
            public static void Prefix(GeoInitialWorldSetup __instance, GeoLevelController level, IList<GeoSiteSceneDef.SiteInfo> worldSites, TimeSlice timeSlice)
            {
                try

                {
                    // __instance holds all variables of GeoInitialWorldSetup, here the initial amount of all scavenging sites
                    __instance.InitialScavengingSiteCount = 4; // default 16

                    // ScavengingSitesDistribution is an array with the weights for scav, rescue soldier and vehicle
                    foreach (GeoInitialWorldSetup.ScavengingSiteConfiguration scavSiteConf in __instance.ScavengingSitesDistribution)
                    {
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_ResourceCrates_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 2; // default 4
                        }
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_RescueSoldier_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 1;
                        }
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_RescueVehicle_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 1;
                        }
                    }
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
               
            }
        }

        //Harmony patch to increase ambush chance
        [HarmonyPatch(typeof(GeoscapeEventSystem), "OnLevelStart")]
        public static class GeoscapeEventSystem_PhoenixFaction_OnLevelStart_Patch
        {
            public static void Prefix(GeoscapeEventSystem __instance)
            {
                try
                {
                    Logger.Debug($"[GeoscapeEventSystem_PhoenixFaction_OnLevelStart_PREFIX] Increasing ambush chance.");
                    __instance.ExplorationAmbushChance = 100;
                    __instance.AmbushExploredSitesProtection = 1;
                    __instance.StartingAmbushProtection = 1;
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

        

    }
}
