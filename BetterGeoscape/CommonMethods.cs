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

        public static string[] tacticalAbilitiesDefNames = new string[26] {"BC_QuickAim_AbilityDef",
            "RapidClearance_AbilityDef", "AimedBurst_AbilityDef", "Dash_AbilityDef", "AdrenalineRush_AbilityDef",
            "GunKata_AbilityDef", "Exertion_AbilityDef", "BC_ARTargeting_AbilityDef", "RageBurst_RageBurstInConeAbilityDef",
            "JetpackControl_AbilityDef","BigBooms_AbilityDef", "DeployDronePack_ShootAbilityDef", "LayWaste_AbilityDef",
            "ArmourBreak_AbilityDef", "BC_Gunslinger_AbilityDef", "ElectricReinforcement_AbilityDef", "AmplifyPain_AbilityDef","MarkedForDeath_AbilityDef", "InducePanic_AbilityDef",
            "MindCrush_AbilityDef", "RemoteDeployment_AbilityDef", "ManualControl_AbilityDef", "FieldMedic_AbilityDef", "Decoy_AbilityDef", "Vanish_AbilityDef", "JetJump_AbilityDef"};
        
        

        public static string[] tacticalAbilitiesDisplayName = new string[26] {"QuickAim", "Rapid Clearence", "AimedBurst_AbilityDef", "Rally", "Dash", "AdreanlineRush", "GunKata_AbilityDef",
        "Exertion_AbilityDef", "BC_ARTargeting_AbilityDef", "Rage Burst", "JetpackControl_AbilityDef", "BigBooms", "Deploy Drone Pack", "LayWaste_AbilityDef", "ArmourBreak",
        "BC_Gunslinger_AbilityDef", "ElectricReinforcement", "AmplifyPain_AbilityDef", "MarkedForDeath", "InducePanic", "MindCrush", "Remote Deployment", "ManualControl", "FieldMedic",
        "Decoy", "Vanish"};

        public static void AdjustWPCostAbility(string abilityDef, float costChange) 
        {
            try
            {
               TacticalAbilityDef ability = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(ged => ged.name.Equals(abilityDef));
                ability.WillPointCost += costChange;                    
            }
            catch (Exception e)
            {
                Logger.Error(e);
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
