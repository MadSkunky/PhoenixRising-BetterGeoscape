using AK.Wwise;
using Base.Defs;
using Base.Eventus;
using Base.Eventus.Filters;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Entities.Research.Requirement;
using PhoenixPoint.Geoscape.Entities.Research.Reward;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Conditions;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Events.Eventus.Filters;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    ApplyChangeDiplomacy = false;
                }

                //Testing Less Pandoran Colonies
                GameDifficultyLevelDef veryhard = Repo.GetAllDefs<GameDifficultyLevelDef>().FirstOrDefault(a => a.name.Equals("VeryHard_GameDifficultyLevelDef"));
                veryhard.NestLimitations.MaxNumber = 3;
                veryhard.NestLimitations.HoursBuildTime = 135;
                veryhard.LairLimitations.MaxNumber = 3;
                veryhard.LairLimitations.MaxConcurrent = 3;
                veryhard.LairLimitations.HoursBuildTime = 150;
                veryhard.CitadelLimitations.HoursBuildTime = 240;

                // KE Story rework - remove missions + Maker
                TheMarketplaceSettingsDef theMarketplaceSettings = Repo.GetAllDefs<TheMarketplaceSettingsDef>().FirstOrDefault(ged => ged.name.Equals("TheMarketplaceSettingsDef"));
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[0].MaxNumberOfOffers = theMarketplaceSettings.TheMarketplaceItemOfferAmounts[4].MaxNumberOfOffers;
                theMarketplaceSettings.TheMarketplaceItemOfferAmounts[0].MinNumberOfOffers = theMarketplaceSettings.TheMarketplaceItemOfferAmounts[4].MinNumberOfOffers;
                theMarketplaceSettings.TheMarketplaceItemPriceMultipliers[0].PriceMultiplier = theMarketplaceSettings.TheMarketplaceItemPriceMultipliers[4].PriceMultiplier;
                theMarketplaceSettings.Missions[0] = null;
                theMarketplaceSettings.Missions[1] = null;
                theMarketplaceSettings.Missions[2] = null;
                theMarketplaceSettings.Missions[3] = null;

                //Replace all LOTA Schemata missions with KE2 mission
                GeoscapeEventDef KE2Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_KE2_GeoscapeEventDef"));
                GeoscapeEventDef LE1Miss = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_MISS_GeoscapeEventDef"));
                LE1Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef = KE2Miss.GeoscapeEventData.Choices[0].Outcome.StartMission.MissionTypeDef;
                //Don't generate next Schemata mission
                GeoscapeEventDef LE1Win = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_WIN_GeoscapeEventDef"));
                GeoscapeEventDef geoEventFS9 = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_FS9_GeoscapeEventDef"));
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
                //Require capturing ancient site for LOTA Schemata missions
                //GeoscapeEventDef LE1Event = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE1_GeoscapeEventDef"));
                //GeoscapeEventDef LEFinalEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_LE_FINAL_GeoscapeEventDef"));
                //GeoLevelConditionDef sourceCondition = Repo.GetAllDefs<GeoLevelConditionDef>().FirstOrDefault(ged => ged.name.Equals("[PROG_LE_FINAL] Condition 1"));
                //GeoLevelConditionDef newCondition = Helper.CreateDefFromClone(sourceCondition, "0358D502-421D-4D9A-9505-491FC80F1C56", "[PROG_LE_1] Condition 2");
                //newCondition.VariableCompareToNumber = 1;
                //LE1Event.GeoscapeEventData.Conditions.Add(newCondition);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
