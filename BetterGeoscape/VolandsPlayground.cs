using Assets.Code.PhoenixPoint.Geoscape.Entities.Sites.TheMarketplace;
using Base.Core;
using Base.Defs;
using Base.UI;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewStates;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Levels.Mist;
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
            // @Voland: play down from here
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

                GeoscapeEventDef darkEvent = Helper.CreateDefFromClone(sourceLoseGeoEvent, "4585B351-3403-4798-B45A-B9DAD77361ED", "DarkEventDef");
                darkEvent.GeoscapeEventData.EventID = "DarkEvent";
                GeoscapeEventDef voidOmen = Helper.CreateDefFromClone(sourceLoseGeoEvent, "396AE9AA-3E2F-440A-83D0-C89255DCB92D", "VoidOmenEventDef");
                voidOmen.GeoscapeEventData.EventID = "VoidOmen";


                Intro.CreateIntro();
                VoidOmens.SetUmbraRandomValue(0);



            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        /*
        [HarmonyPatch(typeof(PhoenixStatisticsManager),"OnResearchCompleted")]
        public static class PhoenixStatisticsManager_OnResearchCompleted_ResearchBasedEvolutionTest_Patch
        {

            public static void Postfix(GeoFaction faction,  ResearchElement research)
            {
                try
                {                   
                    if(research.ResearchID== "SYN_LaserWeapons_ResearchDef") 
                    {
                        string playerCompletedLaserWResearch = "PlayerHasLw";
                        faction.GeoLevel.EventSystem.SetVariable(playerCompletedLaserWResearch, 1);
                        Logger.Always("The variable " + playerCompletedLaserWResearch + "is set to " + faction.GeoLevel.EventSystem.GetVariable(playerCompletedLaserWResearch));
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        public static void TerribleScyllaRoars()
        {
            try
            {
                ResearchDef terribleScyllaResearch = 
                    Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("ALN_Acheron1_ResearchDef"));
                terribleScyllaResearch.Priority = 100;
                terribleScyllaResearch.SecodaryPriority = 100;
                EncounterVariableResearchRequirementDef sourceVarResReq =
                    Repo.GetAllDefs<EncounterVariableResearchRequirementDef>().
                    FirstOrDefault(ged => ged.name.Equals("NJ_Bionics1_ResearchDef_EncounterVariableResearchRequirementDef_0"));
                EncounterVariableResearchRequirementDef variableResReqTerribleScylla = 
                    Helper.CreateDefFromClone(sourceVarResReq, "9622885E-5012-497F-8EC2-9CC690D65612", "TerribleScyllaResReqDef");
                variableResReqTerribleScylla.VariableName = "TerribleScyllaResReqDef";

                terribleScyllaResearch.RevealRequirements.Operation = ResearchContainerOperation.ANY;
                terribleScyllaResearch.RevealRequirements.Container.Add(new ReseachRequirementDefOpContainer
                {
                    Requirements = new ResearchRequirementDef[] { variableResReqTerribleScylla },
                    Operation = ResearchContainerOperation.ANY
                });
                Logger.Always("Terrible Scylla is ready!");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        */

        [HarmonyPatch(typeof(PhoenixStatisticsManager), "OnGeoscapeLevelStart")]
        public static class PhoenixStatisticsManager_OnGeoscapeLevelStart_VoidOmens_Patch
        {

            public static void Postfix(GeoLevelController level)
            {
                try
                {
                    VoidOmens.CreateVoidOmens(level);
                    VoidOmens.CheckForRemovedVoidOmens(level);
                    Umbra.SetUmbraEvolution(level);

                    if (level.EventSystem.GetVariable("BG_Intro_Played") == 0)
                    {
                        GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(level.PhoenixFaction, level.ViewerFaction);
                        level.EventSystem.TriggerGeoscapeEvent("IntroBetterGeo_0", geoscapeEventContext);
                        level.EventSystem.SetVariable("BG_Intro_Played", 1);
                    }
                    if (level.EventSystem.GetVariable("BG_Intro_Played") == 1)
                    {
                        GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(level.PhoenixFaction, level.ViewerFaction);
                        level.EventSystem.TriggerGeoscapeEvent("IntroBetterGeo_1", geoscapeEventContext);
                        level.EventSystem.SetVariable("BG_Intro_Played", 2);
                    }
                    if (level.EventSystem.GetVariable("BG_Intro_Played") == 2)
                    {
                        GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(level.PhoenixFaction, level.ViewerFaction);
                        level.EventSystem.TriggerGeoscapeEvent("IntroBetterGeo_2", geoscapeEventContext);
                        level.EventSystem.SetVariable("BG_Intro_Played", 3);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(PhoenixStatisticsManager), "OnGeoscapeLevelEnd")]
        public static class PhoenixStatisticsManager_OnGeoscapeLevelEnd_VoidOmens_Patch
        {

            public static void Postfix(GeoLevelController level)
            {
                try
                {
                    Umbra.SetUmbraEvolution(level);
                    VoidOmens.CheckForVoidOmensRequiringTacticalPatching(level);
                    if (VoidOmens.VoidOmen16Active && VoidOmens.VoidOmen15Active)
                    {
                        VoidOmens.SetUmbraRandomValue(0.32f);
                    }
                    if (VoidOmens.VoidOmen16Active && !VoidOmens.VoidOmen15Active)
                    {
                        VoidOmens.SetUmbraRandomValue(0.16f);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }


        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionHourly")]
        public static class GeoAlienFaction_UpdateFactionHourly_DarkEvents_Patch
        {
            public static void Postfix(GeoFaction __instance)
            {
                try
                {
                    VoidOmens.CreateVoidOmens(__instance.GeoLevel);
                    VoidOmens.CheckForRemovedVoidOmens(__instance.GeoLevel);
                    Umbra.SetUmbraEvolution(__instance.GeoLevel);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(TacticalAbility), "get_WillPointCost")]
        public static class TacticalAbility_get_WillPointCost_VoidOmenExtraWPCost_Patch
        {
            public static void Postfix(ref float __result, TacticalAbility __instance)
            {
                try
                {
                    if (__result > 0)
                    {
                        if (VoidOmens.VoidOmen3Active && __instance.TacticalActor.IsControlledByPlayer)
                        {
                            __result += Mathf.RoundToInt(__result * 0.5f);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(TacticalVoxelMatrix), "SpawnAndPropagateMist")]
        public static class TacticalVoxelMatrix_SpawnAndPropagateMist_VoidOmenMoreMistOnTactical_Patch
        {
            public static void Prefix(TacticalVoxelMatrix __instance)
            {
                try
                {
                    if (VoidOmens.VoidOmen7Active)
                    {
                        __instance.VoxelMatrixData.InitialMistEntitiesToSpawn.Min = 30;
                        __instance.VoxelMatrixData.InitialMistEntitiesToSpawn.Max = 40;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(UIStateRosterDeployment), "get__squadMaxDeployment")]
        public static class UIStateRosterDeployment_get_SquadMaxDeployment_VoidOmenLimitedDeployment_Patch
        {
            public static void Postfix(ref int __result)
            {
                try
                {
                    if (VoidOmens.VoidOmen4Active)
                    {
                        __result -= 2;
                    }
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }

            }
        }

        [HarmonyPatch(typeof(GeoBehemothActor), "get_DisruptionMax")]
        public static class GeoBehemothActor_get_DisruptionMax_VoidOmenBehemothRoamsMore_Patch
        {
            public static void Postfix(ref int __result, GeoBehemothActor __instance)
            {
                try
                {
                    int[] voidOmensInEffect = VoidOmens.CheckForAlreadyRolledVoidOmens(__instance.GeoLevel);
                    if (voidOmensInEffect.Contains(11))
                    {
                        __result += 3 * __instance.GeoLevel.CurrentDifficultyLevel.Order;
                    }
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }

            }
        }





        /* [HarmonyPatch(typeof(GeoSite), "DestroySite")]
          public static class GeoSite_DestroySite_DestroyedHavenGenerateScav_patch
          {
              internal static bool flag = false;

              public static void Postfix(GeoSite __instance, GeoSiteType ____type)
              {
                  try
                  {

                      if (__instance.Type == GeoSiteType.Haven && !flag)
                      {
                         flag = true;
                      }
                      else if (flag)
                      {
                         __instance.ActiveMission = null;
                          __instance.CreateScavengingMission();
                         flag = false;
                      }
                  }
                  catch (Exception e)
                  {
                      Logger.Error(e);
                  }
              }
          }*/



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
                    if (geoEvent.EventID.Contains("SDI"))
                    {
                        __result.EventLeader = Helper.CreateSpriteFromImageFile("BG_alistair_small.png");
                    }

                    if (geoEvent.EventID.Equals("PROG_FS0"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("FesteringSkiesAfterHamerfall.png");
                    }

                    if (geoEvent.EventID.Equals("DarkEvent"))
                    {
                        __result.EventLeader = Helper.CreateSpriteFromImageFile("BG_alistair_small.png");
                    }

                    if (geoEvent.EventID.Equals("DarkEvent") && (geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_02"
                        || geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_05" || geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_08"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("VO_05.jpg");
                    }

                    if (geoEvent.EventID.Equals("DarkEvent") && geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_11")
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("VO_11.jpg");
                    }

                    if (geoEvent.EventID.Equals("DarkEvent") && (geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_12" || geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_09"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("VO_12.jpg");
                    }
                    if (geoEvent.EventID.Equals("DarkEvent") && geoEvent.EventData.Title.LocalizationKey == "DARK_EVENT_TITLE_13")
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("VO_13.jpg");
                    }


                    if (geoEvent.EventID.Equals("IntroBetterGeo_2"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("BG_Intro_1.jpg");
                        __result.EventLeader = Helper.CreateSpriteFromImageFile("BG_Olena.jpg");
                    }
                    if (geoEvent.EventID.Equals("IntroBetterGeo_1"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("BG_Intro_1.jpg");
                        __result.EventLeader = Helper.CreateSpriteFromImageFile("BG_alistair_small.png");
                    }
                    if (geoEvent.EventID.Equals("IntroBetterGeo_0"))
                    {
                        __result.EventBackground = Helper.CreateSpriteFromImageFile("BG_Intro_0.jpg");
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        /*  [HarmonyPatch(typeof(GeoHavenDefenseMission), "OnUpdateMission")]

          public static class GeoHavenDefenseMission_UpdateGeoscapeMissionState_ChangeToInfestation_patch
          {
              public static void Postfix()
              {
                  try
                  {
                      SharedData sharedData = GameUtl.GameComponent<SharedData>();

                      Logger.Always("Update Geoscape Mission was invoked");
                      // __instance.AttackerFaction == sharedData.AlienFactionDef &&

                      if (DefenseMission.AttackerDeployment >= 1 && DefenseMission.DefenderDeployment <= 2)
                      {
                          Logger.Always("First check was passed");
                          DefenseMission.CompleteUpdateableMission();
                          //  __instance.Site.ActiveMission = null;
                          DefenseMission.Site.GeoLevel.AlienFaction.InfestHaven(DefenseMission.Site);
                          //  
                          Logger.Always("Infestation should have happened");

                      }

                       if (__instance.AttackerDeployment>=1 && __instance.DefenderDeployment <= 2) 
                      {

                          Logger.Always("First check was passed");
                          __instance.CompleteUpdateableMission();
                          //  __instance.Site.ActiveMission = null;
                          __instance.Site.GeoLevel.AlienFaction.InfestHaven(__instance.Site);
                        //  
                          Logger.Always("Infestation should have happened");
                      }

                  }
                  catch (Exception e)
                  {
                      Logger.Error(e);
                  }

              }
          }*/

        [HarmonyPatch(typeof(GeoSite), "CreateHavenDefenseMission")]
        public static class GeoSite_CreateHavenDefenseMission_IncreaseAttackHavenVoidOmen_patch
        {
            public static void Prefix(ref HavenAttacker attacker)
            {
                try
                {
                    if (VoidOmens.VoidOmen12Active)
                    {
                        SharedData sharedData = GameUtl.GameComponent<SharedData>();
                        if (attacker.Faction.PPFactionDef == sharedData.AlienFactionDef)
                        {
                            Logger.Always("Alien deployment was " + attacker.Deployment);
                            attacker.Deployment = Mathf.RoundToInt(attacker.Deployment * 1.5f);
                            Logger.Always("Alien deployment is now " + attacker.Deployment);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        

        [HarmonyPatch(typeof(GeoAlienFaction), "AlienBaseDestroyed")]
        public static class GeoAlienFaction_AlienBaseDestroyed_RemoveVoidOmenDestroyedPC_patch
        {
            public static void Prefix(GeoAlienBase alienBase, GeoAlienFaction __instance)
            {
                try
                {
                    Logger.Always("Lair or Citadal destroyed");
                    if (alienBase.AlienBaseTypeDef.Keyword == "lair" || alienBase.AlienBaseTypeDef.Keyword == "citadel"
                        || (alienBase.AlienBaseTypeDef.Keyword == "nest" && __instance.GeoLevel.CurrentDifficultyLevel.Order == 1))
                    {
                        Logger.Always("Lair or Citadal destroyed, Void Omen should be removed");
                        VoidOmens.RemoveEarliestVoidOmen(__instance.GeoLevel);

                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoMarketplace), "OnSiteVisited")]

        public static class GeoMarketplace_OnSiteVisited_MarketPlace_patch
        {
            public static void Prefix(GeoMarketplace __instance, GeoLevelController ____level, TheMarketplaceSettingsDef ____settings)
            {
                try
                {
                    if (____level.EventSystem.GetVariable(____settings.NumberOfDLC5MissionsCompletedVariable) == 0)
                    {
                        ____level.EventSystem.SetVariable(____settings.NumberOfDLC5MissionsCompletedVariable, 4);
                        ____level.EventSystem.SetVariable(____settings.DLC5IntroCompletedVariable, 1);
                        ____level.EventSystem.SetVariable(____settings.DLC5FinalMovieCompletedVariable, 1);
                        __instance.UpdateOptions(____level.Timing);
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


