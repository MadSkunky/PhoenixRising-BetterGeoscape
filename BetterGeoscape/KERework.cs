using Assets.Code.PhoenixPoint.Geoscape.Entities.Sites.TheMarketplace;
using Base.Defs;
using Base.UI;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class KERework
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        
        public static void Apply_Changes()
        {
            try
            {       
                foreach (GeoMarketplaceItemOptionDef geoMarketplaceItemOptionDef in Repo.GetAllDefs<GeoMarketplaceItemOptionDef>())
                {
                    if (!geoMarketplaceItemOptionDef.DisallowDuplicates) 
                    { 
                    geoMarketplaceItemOptionDef.DisallowDuplicates = true;
                    
                    }
                }

                //ID all the factions for later
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));
                /*
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
                theMarketplaceSettings.Missions[1] = null; 
                theMarketplaceSettings.Missions[2] = null; 
                theMarketplaceSettings.Missions[3] = null;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[1].MinNumberOfOffers = 10;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[1].MinNumberOfOffers = 12;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[2].MinNumberOfOffers = 13;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[2].MinNumberOfOffers = 15;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[3].MinNumberOfOffers = 16;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[3].MinNumberOfOffers = 20;
                */

                //Replace all LOTA Schemata missions with KE2 mission
                GeoscapeEventDef geoEventFS9 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_FS9_GeoscapeEventDef"));
                GeoscapeEventDef KE2Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE2_GeoscapeEventDef"));
                GeoscapeEventDef LE1Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_MISS_GeoscapeEventDef"));
                LE1Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = KE2Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef;
                //Don't generate next Schemata mission
                GeoscapeEventDef LE1Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_WIN_GeoscapeEventDef"));
                //GeoscapeEventDef geoEventFS9 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_FS9_GeoscapeEventDef"));
                LE1Win.GeoscapeEventData.Choices[0].Outcome.SetEvents.Clear();
                LE1Win.GeoscapeEventData.Choices[0].Outcome.TrackEncounters.Clear();
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


            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
