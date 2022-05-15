using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.Eventus;
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
using PhoenixPoint.Geoscape.Entities.DifficultySystem;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Entities.Sites;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.InterceptionPrototype.UI;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View;
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
                GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));

                //Source for creating new events
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));
                sourceLoseGeoEvent.GeoscapeEventData.Choices[0].Outcome.ReEneableEvent = false;
                sourceLoseGeoEvent.GeoscapeEventData.Choices[0].Outcome.ReactiveEncounters.Clear();

                // PhoenixPoint.UseGlobalStorage = false;

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

                //Anu pissed at player for doing Bionics
                GeoscapeEventDef anuPissedAtBionics = Helper.CreateDefFromClone(sourceLoseGeoEvent, "9B41FE35-6059-46DF-8FC2-0E4E3AB8345E", "Anu_Pissed_Over_Bionics");
                anuPissedAtBionics.GeoscapeEventData.EventID = "Anu_Pissed1";
                anuPissedAtBionics.GeoscapeEventData.Leader = "AN_Synod";
                anuPissedAtBionics.GeoscapeEventData.Description[0].General.LocalizationKey = "ANU_PISSED_BIONICS_TEXT_GENERAL_0";
                anuPissedAtBionics.GeoscapeEventData.Title.LocalizationKey = "ANU_PISSED_BIONICS_TITLE";
                anuPissedAtBionics.GeoscapeEventData.Choices[0].Text.LocalizationKey = "ANU_PISSED_BIONICS_CHOICE_0";
                anuPissedAtBionics.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "ANU_PISSED_BIONICS_CHOICE_0_OUTCOME";
                anuPissedAtBionics.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = phoenixPoint,
                    Value = -8,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                });

                anuPissedAtBionics.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Synedrion,
                    TargetFaction = phoenixPoint,
                    Value = +2,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                });
                anuPissedAtBionics.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("ANU_PISSED_BIONICS_CHOICE_1"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("ANU_PISSED_BIONICS_CHOICE_1_OUTCOME")
                        },
                    }
                });
                anuPissedAtBionics.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = phoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -8

                });
                anuPissedAtBionics.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = phoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +2
                });
                anuPissedAtBionics.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("ANU_PISSED_BIONICS_CHOICE_2"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        VariablesChange = new List<OutcomeVariableChange>
                        { new OutcomeVariableChange
                            {
                                VariableName = "BG_Anu_Pissed_Made_Promise",
                                Value = { Min = 1, Max = 1 },
                                IsSetOperation = true,
                             }
                        },
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("ANU_PISSED_BIONICS_CHOICE_2_OUTCOME")
                        }
                    }
                });

                //Anu really pissed at player for doing Bionics
                GeoscapeEventDef anuReallyPissedAtBionics = Helper.CreateDefFromClone(sourceLoseGeoEvent, "ED58D3D6-BCFC-4288-9BD8-2A7DD1910FA9", "Anu_Really_Pissed_Over_Bionics");
                anuReallyPissedAtBionics.GeoscapeEventData.EventID = "Anu_Pissed2";
                anuReallyPissedAtBionics.GeoscapeEventData.Leader = "AN_Synod";
                anuReallyPissedAtBionics.GeoscapeEventData.Description[0].General.LocalizationKey = "ANU_REALLY_PISSED_BIONICS_TEXT_GENERAL_0";
                anuReallyPissedAtBionics.GeoscapeEventData.Title.LocalizationKey = "ANU_REALLY_PISSED_BIONICS_TITLE";
                anuReallyPissedAtBionics.GeoscapeEventData.Choices[0].Text.LocalizationKey = "ANU_REALLY_PISSED_BIONICS_CHOICE_0";
                //anuReallyPissedAtBionics.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "ANU_REALLY_PISSED_BIONICS_CHOICE_0_OUTCOME";
               // GenerateGeoEventChoice(anuReallyPissedAtBionics, "blah", "blah");
                anuReallyPissedAtBionics.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = phoenixPoint,
                    Value = -10,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                });

                /* 
                 VariablesChange = new List<OutcomeVariableChange>
                     { new OutcomeVariableChange
                         {
                             VariableName = "BG_NJ_Pissed_Made_Promise",
                             Value = { Min = 1, Max = 1 },
                             IsSetOperation = true,
                          }
                     },
                */

                //NJ pissed at player for doing Mutations
                GeoscapeEventDef nJPissedAtMutations = Helper.CreateDefFromClone(sourceLoseGeoEvent, "7FEFAE2A-1544-488D-A5CE-54E57C62ED2C", "NJ_Pissed_Over_Mutations");
                nJPissedAtMutations.GeoscapeEventData.EventID = "NJ_Pissed1";
                nJPissedAtMutations.GeoscapeEventData.Leader = "NJ_TW";
                nJPissedAtMutations.GeoscapeEventData.Description[0].General.LocalizationKey = "NJ_PISSED_MUTATIONS_TEXT_GENERAL_0";
                nJPissedAtMutations.GeoscapeEventData.Title.LocalizationKey = "NJ_PISSED_MUTATIONS_TITLE";
                nJPissedAtMutations.GeoscapeEventData.Choices[0].Text.LocalizationKey = "NJ_PISSED_MUTATIONS_CHOICE_0";
                nJPissedAtMutations.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "NJ_PISSED_MUTATIONS_CHOICE_0_OUTCOME";
                nJPissedAtMutations.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = phoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -5
                });
                nJPissedAtMutations.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("NJ_PISSED_MUTATIONS_CHOICE_1"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("NJ_PISSED_MUTATIONS_CHOICE_1_OUTCOME")
                        }
                    }
                });
                nJPissedAtMutations.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = phoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = -8
                });
                nJPissedAtMutations.GeoscapeEventData.Choices[1].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = Anu,
                    TargetFaction = phoenixPoint,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                    Value = +2
                });
                nJPissedAtMutations.GeoscapeEventData.Choices.Add(new GeoEventChoice()
                {
                    Text = new LocalizedTextBind("NJ_PISSED_MUTATIONS_CHOICE_2"),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        VariablesChange = new List<OutcomeVariableChange>
                        { new OutcomeVariableChange
                            {
                                VariableName = "BG_NJ_Pissed_Made_Promise",
                                Value = { Min = 1, Max = 1 },
                                IsSetOperation = true,
                             }
                        },
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind("NJ_PISSED_MUTATIONS_CHOICE_2_OUTCOME")
                        }
                    }
                });
                //Anu really pissed at player for doing Bionics
                GeoscapeEventDef nJReallyPissedAtMutations = Helper.CreateDefFromClone(sourceLoseGeoEvent, "98F11CA9-F2F5-4B9D-8D93-9823B83A0E3E", "NJ_Really_Pissed_Over_Mutations");
                nJReallyPissedAtMutations.GeoscapeEventData.EventID = "NJ_Pissed2";
                nJReallyPissedAtMutations.GeoscapeEventData.Leader = "NJ_TW";
                nJReallyPissedAtMutations.GeoscapeEventData.Description[0].General.LocalizationKey = "NJ_REALLY_PISSED_MUTATIONS_TEXT_GENERAL_0";
                nJReallyPissedAtMutations.GeoscapeEventData.Title.LocalizationKey = "NJ_REALLY_PISSED_MUTATIONS_TITLE";
                nJReallyPissedAtMutations.GeoscapeEventData.Choices[0].Text.LocalizationKey = "NJ_REALLY_PISSED_MUTATIONS_CHOICE_0";
                //nJReallyPissedAtMutations.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = "NJ_REALLY_PISSED_MUTATIONS_CHOICE_0_OUTCOME";
                
                nJReallyPissedAtMutations.GeoscapeEventData.Choices[0].Outcome.Diplomacy.Add(new OutcomeDiplomacyChange()
                {
                    PartyFaction = NewJericho,
                    TargetFaction = phoenixPoint,
                    Value = -10,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                });

                /* 
                 VariablesChange = new List<OutcomeVariableChange>
                     { new OutcomeVariableChange
                         {
                             VariableName = "BG_NJ_Pissed_Made_Promise",
                             Value = { Min = 1, Max = 1 },
                             IsSetOperation = true,
                          }
                     },
                */


            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

      

      [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        public static class PhoenixStatisticsManager_OnGeoscapeLevelStart_DarkEvents_Patch
        {
            public static void Postfix(GeoFaction __instance)
            {
                try
                {


                    if (__instance.GeoLevel.EventSystem.IsEventTriggered("SDI_02"))
                    {
                        __instance.GeoLevel.EventSystem.SetVariable("LessAmbushes", 1);
                    }
                    if (__instance.GeoLevel.EventSystem.GetVariable("LessAmbushes") == 1)
                    {
                        __instance.GeoLevel.CurrentDifficultyLevel.EvolutionProgressPerDay=150;
                    }
                    else
                    {
                        __instance.GeoLevel.EventSystem.StartingAmbushProtection = 0;
                    }
                    if (__instance.GeoLevel.EventSystem.IsEventTriggered("SDI_03"))
                    {
                        __instance.GeoLevel.CurrentDifficultyLevel.EvolutionProgressPerDay = 50;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
     

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        public static class PhoenixStatisticsManager_UpdateGeoscapeStats_AnuPissedAtBionics_Patch
        {
            public static void Postfix(GeoAlienFaction __instance)
            {
                try
                {
                    int bionics = 0;
                    GeoLevelController geoLevelController = __instance.GeoLevel;
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance, geoLevelController.ViewerFaction);
                    GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                    //check number of bionics player has
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;
                    foreach (GeoCharacter geoCharacter in __instance.GeoLevel.PhoenixFaction.Soldiers)
                    {
                        foreach (GeoItem bionic in geoCharacter.ArmourItems)
                        { if (bionic.ItemDef.Tags.Contains(bionicalTag))

                                bionics += 1;
                        }
                    }
                    if (bionics > 2 && geoLevelController.EventSystem.GetVariable("BG_Anu_Pissed_Over_Bionics") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("Anu_Pissed1", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_Anu_Pissed_Over_Bionics", 1);
                    }

                    if (geoLevelController.EventSystem.GetVariable("BG_Anu_Pissed_Broke_Promise") == 1
                       && geoLevelController.EventSystem.GetVariable("BG_Anu_Really_Pissed_Over_Bionics") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("Anu_Pissed2", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_Anu_Really_Pissed_Over_Bionics", 1);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
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

        /*   public static OutcomeDiplomacyChange GenerateDiplomacyOutcome(GeoFactionDef partyFaction, GeoFactionDef targetFaction, int value)
           {
               try
               {
                   if (partyFaction == null || targetFaction == null)
                   {
                       GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                       GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                       return new OutcomeDiplomacyChange()
                       { PartyFaction=NewJericho, TargetFaction=phoenixPoint, Value=1, PartyType= (OutcomeDiplomacyChange.ChangeTarget)1};
                   }

                   else
                   { 
                       return new OutcomeDiplomacyChange()
                       {
                           PartyFaction = partyFaction,
                           TargetFaction = targetFaction,
                           Value = value,
                           PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                       };
                   }
               }
               catch (Exception e)
               {
                   Logger.Error(e);
               }
           } */

        /*   public static void GenerateVariableChange(string variableName, int minValue, int MaxValue, bool isSet)
           {
               try
               {
                   new OutcomeVariableChange()
                   {
                       VariableName = variableName,
                       Value = { Min = minValue, Max = MaxValue},
                       IsSetOperation = isSet,
                   };

               }
               catch (Exception e)
               {
                   Logger.Error(e);
               }
           }*/

        

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        public static class PhoenixStatisticsManager_UpdateGeoscapeStats_NJPissedAtMutations_Patch
        {
            public static void Postfix(GeoAlienFaction __instance)
            {
                try
                {
                    int mutations = 0;
                    GeoLevelController geoLevelController = __instance.GeoLevel;
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance, geoLevelController.ViewerFaction);
                    GeoFactionDef newJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                    //check number of mutations player has
                    GameTagDef mutationTag = GameUtl.GameComponent<SharedData>().SharedGameTags.AnuMutationTag;
                    foreach (GeoCharacter geoCharacter in __instance.GeoLevel.PhoenixFaction.Soldiers)
                    {
                        foreach (GeoItem mutation in geoCharacter.ArmourItems)
                        {
                            if (mutation.ItemDef.Tags.Contains(mutationTag))
                                mutations += 1;
                        }                      
                    }
                    if (mutations > 2 && geoLevelController.EventSystem.GetVariable("BG_NJ_Pissed_Over_Mutations") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("NJ_Pissed1", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_NJ_Pissed_Over_Mutations", 1);
                    }
                    if (geoLevelController.EventSystem.GetVariable("BG_NJ_Pissed_Broke_Promise") == 1
                       && geoLevelController.EventSystem.GetVariable("BG_NJ_Really_Pissed_Over_Mutations") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("NJ_Pissed2", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_NJ_Really_Pissed_Over_Mutations", 1);
                    }
                    
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(SiteEncountersArtCollectionDef), "GetEventArt")]
        public static class SiteEncountersArtCollectionDef_GetEventArt_InjectArt_patch
        {
         public static void Postfix(ref EncounterEventArt __result, GeoscapeEvent geoEvent)
         {
          try
          {
             /* if (geoEvent.EventID.Equals("Anu_Pissed1"))
              {
                  __result.EventBackground = Helper.CreateSpriteFromImageFile("combat.png");
              }
             */
              if (geoEvent.EventID.Equals("PROG_FS0"))
              {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("FesteringSkiesAfterHamerfall.png");
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

