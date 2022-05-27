using Base.Defs;
using Base.UI;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using System;
using System.Linq;

namespace PhoenixRising.BetterGeoscape
{
    internal class CommonMethods
    {
       // private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

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
    }

}
