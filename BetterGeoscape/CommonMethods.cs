using Base.Defs;
using Base.UI;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities.Abilities;
using System;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class CommonMethods
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
        public static GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
        public static GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
        public static GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));


        public static void SetStaminaToZero(GeoCharacter __instance)
        {
            try
            {
                if (__instance.Fatigue.Stamina != null && __instance.Fatigue.Stamina > 0)
                {
                    __instance.Fatigue.Stamina.SetToMin();
                }
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }

        }
        public static void GenerateGeoEventChoice(GeoscapeEventDef geoEvent, string choice, string outcome)
        {
            try
            {
                geoEvent.GeoscapeEventData.Choices.Add(new GeoEventChoice()

                {
                    Text = new LocalizedTextBind(choice),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind(outcome)
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static OutcomeDiplomacyChange GenerateDiplomacyOutcome(GeoFactionDef partyFaction, GeoFactionDef targetFaction, int value)
          {
              try
              {
                 return new OutcomeDiplomacyChange()
                 {
                    PartyFaction = partyFaction,
                    TargetFaction = targetFaction,
                    Value = value,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                 };     
              }           
              catch (Exception e)
              {
                  Logger.Error(e);
              }
            throw new InvalidOperationException();
        }

        public static OutcomeVariableChange GenerateVariableChange(string variableName, int value, bool isSet)
           {
               try
               {
                   return new OutcomeVariableChange()
                   {
                       VariableName = variableName,
                       Value = { Min = value, Max = value},
                       IsSetOperation = isSet,
                   };
               }
               catch (Exception e)
               {
                   Logger.Error(e);
               }
            throw new InvalidOperationException();
        }

        public static GeoscapeEventDef CreateNewEvent(string name, string title, string description, string outcome)        
        {
            try
            {

                string gUID = Guid.NewGuid().ToString();
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));
                sourceLoseGeoEvent.GeoscapeEventData.Choices[0].Outcome.ReEneableEvent = false;
                sourceLoseGeoEvent.GeoscapeEventData.Choices[0].Outcome.ReactiveEncounters.Clear();
                GeoscapeEventDef newEvent = Helper.CreateDefFromClone(sourceLoseGeoEvent, gUID, name);
                newEvent.GeoscapeEventData.EventID = name;
                newEvent.GeoscapeEventData.Title.LocalizationKey = title;
                newEvent.GeoscapeEventData.Description[0].General.LocalizationKey = description;
                if(outcome != null) 
                { 
                newEvent.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey=outcome;
                }
                return newEvent;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static void ApplyRepPenaltyForRevealDiploMission(string eventIDRevealMission, GeoFactionDef partyFaction, int value)
        {
            try
            {
                GeoscapeEventDef eventDef = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals(eventIDRevealMission));
                eventDef.GeoscapeEventData.Choices[0].Outcome.Diplomacy[0] = GenerateDiplomacyOutcome(partyFaction, phoenixPoint, value);

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }



        }


    }

}
