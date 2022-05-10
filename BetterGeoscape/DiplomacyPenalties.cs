﻿using Base.Defs;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class DiplomacyPenalties
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        private static bool ApplyChangeDiplomacy = true;

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
                    PartyFaction = Anu,
                    TargetFaction = PhoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -18
                });

                ProgSynPact.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
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

            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}