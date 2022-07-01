using Base.Defs;
using Base.Entities.Effects;
using Base.Entities.Effects.ApplicationConditions;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Entities.Missions.Outcomes;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Entities.Sites;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Levels.FactionEffects;
using System;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class VoidOmens
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static bool[] voidOmensCheck = new bool[18];
        //VO#3 is WP cost +50%
        public static bool VoidOmen3Active = false;
        public static bool VoidOmen3Activated = false;
        public static bool VoidOmen4Active = false;
        //VO#7 is more mist in missions
        public static bool VoidOmen7Active = false;
        //VO#10 is no limit to Delirium
        public static bool VoidOmen10Active = false;
        //VO#12 is +50% strength of alien attacks on Havens
        public static bool VoidOmen12Active = false;
        //VO#15 is more Umbra
        public static bool VoidOmen15Active = false;
        //VO#16 is Umbras can appear anywhere and attack anyone
        public static bool VoidOmen16Active = false;



        public static void CreateVoidOmens(GeoLevelController level)
        {
            try
            {
                int difficulty = level.CurrentDifficultyLevel.Order;
                string voidOmen = "VoidOmen_";

                for (int i = 1; i < difficulty + 1; i++)
                {
                    for (int j = 1; j < voidOmensCheck.Count() - 1; j++)
                    {
                        if (level.EventSystem.GetVariable(voidOmen + i) == j)
                        {
                            if (j == 1 && voidOmensCheck[j] == false)
                            {
                                level.EventSystem.ExplorationAmbushChance = 100;
                                CustomMissionTypeDef AmbushALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAlien_CustomMissionTypeDef"));
                                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                                AmbushALN.CratesDeploymentPointsRange.Min = 50;
                                AmbushALN.CratesDeploymentPointsRange.Max = 70;
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable "+voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 2 && voidOmensCheck[j] == false)
                            {
                                foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                                {
                                    foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                                    {
                                        for (int t = 0; t < choice.Outcome.Diplomacy.Count; t++)
                                        {
                                            if (choice.Outcome.Diplomacy[t].Value != 0)
                                            {
                                                OutcomeDiplomacyChange diplomacyChange = choice.Outcome.Diplomacy[t];
                                                diplomacyChange.Value = Mathf.RoundToInt(diplomacyChange.Value * 0.5f);
                                                choice.Outcome.Diplomacy[t] = diplomacyChange;
                                            }
                                        }
                                    }
                                }
                                foreach (DiplomacyMissionOutcomeDef diplomacyMissionOutcomeDef in Repo.GetAllDefs<DiplomacyMissionOutcomeDef>())
                                {
                                    diplomacyMissionOutcomeDef.DiplomacyToFaction.Max = Mathf.RoundToInt(diplomacyMissionOutcomeDef.DiplomacyToFaction.Max * 0.5f);
                                    diplomacyMissionOutcomeDef.DiplomacyToFaction.Min = Mathf.RoundToInt(diplomacyMissionOutcomeDef.DiplomacyToFaction.Min * 0.5f);
                                }
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;

                            }
                            if (j == 3 && voidOmensCheck[j] == false)
                            {
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 4 && voidOmensCheck[j] == false) //pending adding extra XP gain
                            {
                                VoidOmen4Active = true;
                                voidOmensCheck[j] = true;
                            }
                            if (j == 5 && voidOmensCheck[j] == false)
                            {
                                foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                                {

                                    if (missionTypeDef.name.Contains("Haven") && !missionTypeDef.name.Contains("Infestation"))
                                    {
                                        TacCrateDataDef cratesNotResources = Repo.GetAllDefs<TacCrateDataDef>().FirstOrDefault(ged => ged.name.Equals("Default_TacCrateDataDef"));
                                        if (missionTypeDef.name.Contains("Civ"))
                                        {
                                            missionTypeDef.ParticipantsRelations[1].MutualRelation = FactionRelation.Enemy;
                                        }
                                        else if (!missionTypeDef.name.Contains("Civ"))
                                        {
                                            missionTypeDef.ParticipantsRelations[2].MutualRelation = FactionRelation.Enemy;
                                        }
                                        missionTypeDef.ParticipantsData[1].PredeterminedFactionEffects = missionTypeDef.ParticipantsData[0].PredeterminedFactionEffects;
                                        missionTypeDef.MissionSpecificCrates = cratesNotResources;
                                        missionTypeDef.FactionItemsRange.Min = 2;
                                        missionTypeDef.FactionItemsRange.Max = 7;
                                        missionTypeDef.CratesDeploymentPointsRange.Min = 20;
                                        missionTypeDef.CratesDeploymentPointsRange.Max = 30;
                                    }
                                }
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 6 && voidOmensCheck[j] == false)
                            {
                                level.CurrentDifficultyLevel.EvolutionPointsGainOnMissionLoss = 20;
                                level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 30;
                                level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 60;
                                level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 90;
                                foreach (ResourceGeneratorFacilityComponentDef lab in Repo.GetAllDefs<ResourceGeneratorFacilityComponentDef>())
                                {
                                    if (lab.name == "E_ResourceGenerator [ResearchLab_PhoenixFacilityDef]" || lab.name == "E_ResourceGenerator [BionicsLab_PhoenixFacilityDef]")
                                        lab.BaseResourcesOutput.Values.Add(new ResourceUnit { Type = ResourceType.Research, Value = 2 });
                                }
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;

                            }
                            if (j == 7 && voidOmensCheck[j] == false)
                            {
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }

                            if (j == 8 && voidOmensCheck[j] == false)
                            {
                                GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                                foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                                {
                                    foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                                    {
                                        for (int t = 0; t < choice.Outcome.Diplomacy.Count; t++)
                                        {
                                            if (choice.Outcome.Diplomacy[t].Value <= 0 && choice.Outcome.Diplomacy[t].TargetFaction != phoenixPoint)
                                            {
                                                OutcomeDiplomacyChange diplomacyChange = choice.Outcome.Diplomacy[t];
                                                diplomacyChange.Value = Mathf.RoundToInt(diplomacyChange.Value * 0.5f);
                                                choice.Outcome.Diplomacy[t] = diplomacyChange;
                                            }
                                        }
                                    }
                                }
                                GeoHavenZoneDef havenLab = Repo.GetAllDefs<GeoHavenZoneDef>().FirstOrDefault(ged => ged.name.Equals("Research_GeoHavenZoneDef"));
                                havenLab.ProvidesResearch = 2;
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }

                            if (j == 9 && voidOmensCheck[j] == false)
                            {
                                FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                                festeringSkiesSettingsDef.HavenAttackCounterModifier = 0.66f;
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }

                            if (j == 10 && voidOmensCheck[j] == false)
                            {
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 11 && voidOmensCheck[j] == false)
                            {
                                /*   FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                                   festeringSkiesSettingsDef.DisruptionThreshholdBaseValue = 15;*/
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;

                            }
                            if (j == 12 && voidOmensCheck[j] == false)
                            {
                                VoidOmen12Active = true;
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;

                            }
                            if (j == 13 && voidOmensCheck[j] == false)
                            {
                                level.CurrentDifficultyLevel.NestLimitations.HoursBuildTime /= 2;
                                level.CurrentDifficultyLevel.LairLimitations.HoursBuildTime /= 2;
                                level.CurrentDifficultyLevel.CitadelLimitations.HoursBuildTime /= 2;

                                Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i + ", so Pandoran nests take " + level.CurrentDifficultyLevel.NestLimitations.HoursBuildTime + " hours");
                                voidOmensCheck[j] = true;
                            }
                            if (j == 14 && voidOmensCheck[j] == false)
                            {
                                TacticalPerceptionDef tacticalPerceptionDef = Repo.GetAllDefs<TacticalPerceptionDef>().FirstOrDefault((TacticalPerceptionDef a) => a.name.Equals("Soldier_PerceptionDef"));
                                tacticalPerceptionDef.PerceptionRange = 20;
                                // Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 15 && voidOmensCheck[j] == false)
                            {
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 16 && voidOmensCheck[j] == false)
                            {
                                if (!voidOmensCheck[15])
                                {
                                    SetUmbraRandomValue(0.16f);
                                }
                                if (voidOmensCheck[15])
                                {
                                    SetUmbraRandomValue(0.32f);
                                }
                                //  Logger.Always(voidOmen + j + " is now in effect, held in variable " + voidOmen + i);
                                voidOmensCheck[j] = true;
                            }
                            if (j == 17 && voidOmensCheck[j] == false)
                            {
                                voidOmensCheck[j] = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void CheckForRemovedVoidOmens(GeoLevelController level)
        {
            try
            {
                int difficulty = level.CurrentDifficultyLevel.Order;
                int[] voidOmensInPlay = CheckFordVoidOmensInPlay(level);
                Logger.Always("Checking if method invocation is working, these are the Void Omens in play " + voidOmensInPlay[0] + " "
                    + voidOmensInPlay[1] + " " + voidOmensInPlay[2] + " " + voidOmensInPlay[3]);

                for (int i = 1; i < voidOmensCheck.Count() - 1; i++)
                {
                    if (!voidOmensInPlay.Contains(i) && voidOmensCheck[i])
                    {
                        if (voidOmensCheck[1])
                        {
                            level.EventSystem.ExplorationAmbushChance = 70;
                            CustomMissionTypeDef AmbushALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAlien_CustomMissionTypeDef"));
                            AmbushALN.ParticipantsData[0].ReinforcementsTurns.Max = 2;
                            AmbushALN.ParticipantsData[0].ReinforcementsTurns.Min = 2;
                            AmbushALN.CratesDeploymentPointsRange.Min = 30;
                            AmbushALN.CratesDeploymentPointsRange.Max = 50;
                            voidOmensCheck[1] = false;
                            //   Logger.Always("Exploration ambush chance is now " + level.EventSystem.ExplorationAmbushChance);
                            //  Logger.Always("Alien ambushes can now have a max of  " + AmbushALN.CratesDeploymentPointsRange.Max / 10 + " crates");
                            Logger.Always("The check for VO#1 went ok");
                        }
                        else if (voidOmensCheck[2])
                        {
                            foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                            {
                                foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                                {
                                    for (int t = 0; t < choice.Outcome.Diplomacy.Count; t++)
                                    {
                                        if (choice.Outcome.Diplomacy[t].Value != 0)
                                        {
                                            OutcomeDiplomacyChange diplomacyChange = choice.Outcome.Diplomacy[t];
                                            diplomacyChange.Value = Mathf.RoundToInt(diplomacyChange.Value * 2f);
                                            choice.Outcome.Diplomacy[t] = diplomacyChange;
                                        }
                                    }
                                }
                            }
                            foreach (DiplomacyMissionOutcomeDef diplomacyMissionOutcomeDef in Repo.GetAllDefs<DiplomacyMissionOutcomeDef>())
                            {
                                diplomacyMissionOutcomeDef.DiplomacyToFaction.Max = Mathf.RoundToInt(diplomacyMissionOutcomeDef.DiplomacyToFaction.Max * 2f);
                                diplomacyMissionOutcomeDef.DiplomacyToFaction.Min = Mathf.RoundToInt(diplomacyMissionOutcomeDef.DiplomacyToFaction.Min * 2f);
                            }
                            voidOmensCheck[2] = false;
                            Logger.Always("The check for VO#2 went ok");
                        }
                        else if (voidOmensCheck[3])
                        {

                            voidOmensCheck[3] = false;
                            Logger.Always("The check for VO#3 went ok");
                        }

                        else if (voidOmensCheck[4])
                        {
                            VoidOmen4Active = false;
                            voidOmensCheck[4] = false;
                            Logger.Always("The check for VO#4 went ok");
                        }

                        else if (voidOmensCheck[5])
                        {
                            foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                            {
                                if (missionTypeDef.name.Contains("Haven") && !missionTypeDef.name.Contains("Infestation"))
                                {
                                    TacticalFactionEffectDef defendersCanBeRecruited = Repo.GetAllDefs<TacticalFactionEffectDef>().FirstOrDefault(ged => ged.name.Equals("CanBeRecruitedByPhoenix_FactionEffectDef"));

                                    if (missionTypeDef.name.Contains("Civ"))
                                    {
                                        missionTypeDef.ParticipantsRelations[1].MutualRelation = FactionRelation.Friend;
                                    }
                                    else if (!missionTypeDef.name.Contains("Civ"))
                                    {
                                        missionTypeDef.ParticipantsRelations[2].MutualRelation = FactionRelation.Friend;
                                    }
                                    EffectDef[] predeterminedFactionEffects = new EffectDef[1] { defendersCanBeRecruited };
                                    missionTypeDef.ParticipantsData[1].PredeterminedFactionEffects = predeterminedFactionEffects;
                                    missionTypeDef.FactionItemsRange.Min = 0;
                                    missionTypeDef.FactionItemsRange.Max = 0;
                                    missionTypeDef.CratesDeploymentPointsRange.Min = 0;
                                    missionTypeDef.CratesDeploymentPointsRange.Max = 0;
                                }
                            }
                            voidOmensCheck[5] = false;
                            Logger.Always("The check for VO#5 went ok");
                        }

                        else if (voidOmensCheck[6])
                        {
                            level.CurrentDifficultyLevel.EvolutionPointsGainOnMissionLoss = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0;
                            foreach (ResourceGeneratorFacilityComponentDef lab in Repo.GetAllDefs<ResourceGeneratorFacilityComponentDef>())
                            {
                                if (lab.name == "E_ResourceGenerator [ResearchLab_PhoenixFacilityDef]"
                                    || lab.name == "E_ResourceGenerator [BionicsLab_PhoenixFacilityDef]"
                                    && lab.BaseResourcesOutput.Values[1] != null)
                                {
                                    lab.BaseResourcesOutput.Values.Remove(lab.BaseResourcesOutput.Values[1]);
                                }
                            }
                            voidOmensCheck[6] = false;
                            Logger.Always("The check for VO#6 went ok");
                        }

                        else if (voidOmensCheck[7])
                        {
                            voidOmensCheck[7] = false;
                            Logger.Always("The check for VO#7 went ok");
                        }

                        else if (voidOmensCheck[8])
                        {
                            GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                            foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                            {
                                foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                                {
                                    for (int t = 0; t < choice.Outcome.Diplomacy.Count; t++)
                                    {
                                        if (choice.Outcome.Diplomacy[t].Value <= 0 && choice.Outcome.Diplomacy[t].TargetFaction != phoenixPoint)
                                        {
                                            OutcomeDiplomacyChange diplomacyChange = choice.Outcome.Diplomacy[t];
                                            diplomacyChange.Value = Mathf.RoundToInt(diplomacyChange.Value * 2f);
                                            choice.Outcome.Diplomacy[t] = diplomacyChange;
                                        }
                                    }
                                }
                            }
                            GeoHavenZoneDef havenLab = Repo.GetAllDefs<GeoHavenZoneDef>().FirstOrDefault(ged => ged.name.Equals("Research_GeoHavenZoneDef"));
                            havenLab.ProvidesResearch = 1;
                            Logger.Always("The check for VO#8 went ok");
                            voidOmensCheck[8] = false;
                        }

                        else if (voidOmensCheck[9])
                        {
                            FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                            festeringSkiesSettingsDef.HavenAttackCounterModifier = 1.33f;
                            voidOmensCheck[9] = false;
                            Logger.Always("The check for VO#9 went ok");
                        }

                        else if (voidOmensCheck[10])
                        {
                            voidOmensCheck[10] = false;
                            Logger.Always("The check for VO#10 went ok");
                        }

                        else if (voidOmensCheck[11])
                        {
                            /*   FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                               festeringSkiesSettingsDef.DisruptionThreshholdBaseValue = 3;*/
                            voidOmensCheck[11] = false;
                            Logger.Always("The check for VO#11 went ok");
                        }

                        else if (voidOmensCheck[12])
                        {
                            VoidOmen12Active = false;
                            voidOmensCheck[12] = false;
                            Logger.Always("The check for VO#12 went ok");
                        }

                        else if (voidOmensCheck[13])
                        {

                            level.CurrentDifficultyLevel.NestLimitations.HoursBuildTime *= 2;
                            level.CurrentDifficultyLevel.LairLimitations.HoursBuildTime *= 2;
                            level.CurrentDifficultyLevel.CitadelLimitations.HoursBuildTime *= 2;

                            voidOmensCheck[13] = false;

                            Logger.Always("The check for VO#13 went ok" + " so Pandoran nests take " + level.CurrentDifficultyLevel.NestLimitations.HoursBuildTime + " hours");
                        }

                        else if (voidOmensCheck[14])
                        {
                            TacticalPerceptionDef tacticalPerceptionDef = Repo.GetAllDefs<TacticalPerceptionDef>().FirstOrDefault((TacticalPerceptionDef a) => a.name.Equals("Soldier_PerceptionDef"));
                            tacticalPerceptionDef.PerceptionRange = 30;
                            voidOmensCheck[14] = false;
                            Logger.Always("The check for VO#14 went ok");
                        }

                        else if (voidOmensCheck[15])
                        {
                            voidOmensCheck[15] = false;
                            Logger.Always("The check for VO#15 went ok");
                        }

                        else if (voidOmensCheck[16])
                        {
                            SetUmbraRandomValue(0);

                            voidOmensCheck[16] = false;
                            Logger.Always("The check for VO#16 went ok");
                        }

                        else if (voidOmensCheck[17])
                        {
                            voidOmensCheck[17] = false;
                            Logger.Always("The check for VO#17 went ok");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void CheckForVoidOmensRequiringTacticalPatching(GeoLevelController level)
        {
            try
            {

                VoidOmen3Active = false;
                VoidOmen7Active = false;
                VoidOmen10Active = false;
                VoidOmen15Active = false;
                VoidOmen16Active = false;

                int[] rolledVoidOmens = CheckFordVoidOmensInPlay(level);

                if (rolledVoidOmens.Contains(3))
                {
                    VoidOmen3Active = true;
                }
                if (rolledVoidOmens.Contains(7))
                {
                    VoidOmen7Active = true;
                }
                if (rolledVoidOmens.Contains(10))
                {
                    VoidOmen10Active = true;
                }
                if (rolledVoidOmens.Contains(15))
                {
                    VoidOmen15Active = true;
                }
                if (rolledVoidOmens.Contains(16))
                {
                    VoidOmen16Active = true;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static int[] CheckForAlreadyRolledVoidOmens(GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                int[] allVoidOmensAlreadyRolled = new int[20];
                string triggeredVoidOmens = "TriggeredVoidOmen_";

                for (int x = 1; x < 20; x++)
                {
                    if (geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x) != 0)
                    {
                        allVoidOmensAlreadyRolled[x] = geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x);
                    }
                }
                return allVoidOmensAlreadyRolled;
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }

            throw new InvalidOperationException();
        }


        public static int[] CheckFordVoidOmensInPlay(GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                string voidOmen = "VoidOmen_";

                // An array to record all the Void Omens rolled so far
                int[] allVoidOmensAlreadyRolled = CheckForAlreadyRolledVoidOmens(geoLevelController);
                // An array to record which variables hold which Void Omens
                int[] voidOmensInPlay = new int[difficulty];

                // This is a variable to close the loop below when the array of Void Omens in play is full               
                int variablesUsed = 0;

                // We will check our Void Omen variables to see which one has the earliest Void Omen already rolled                                
                for (int x = 1; x < 20; x++)
                {
                    // We will look through the Void Omen variables in the order in which they were filled
                    for (int y = 0; y < difficulty; y++)
                    {
                        // And record which variable holds which Void Omen, by checking it against the array of already rolled Void Omens
                        if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)) == allVoidOmensAlreadyRolled[x])
                        {
                            voidOmensInPlay[difficulty - y - 1] = allVoidOmensAlreadyRolled[x];
                            //  Logger.Always("Check Variable " + (difficulty - y) + " holding Void Omen " + voidOmensInPlay[difficulty - y - 1]);
                            variablesUsed++;
                        }
                        // We also have to record which variables are empty
                        else if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)) == 0)
                        {
                            voidOmensInPlay[difficulty - y - 1] = 0;
                            //  Logger.Always("Check Variable " + (difficulty - y) + " holding Void Omen " + voidOmensInPlay[difficulty - y - 1]);
                            variablesUsed++;
                        }
                    }
                    //  Logger.Always("the count of variables used is " + variablesUsed);
                    if (variablesUsed == difficulty)
                    {
                        x = 20;
                    }

                }
                Logger.Always("The Void Omens already in play are " + voidOmensInPlay[0] + " " + voidOmensInPlay[1] + " " + voidOmensInPlay[2] + " " + voidOmensInPlay[3]);
                return voidOmensInPlay;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static void RemoveEarliestVoidOmen
        (GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                string triggeredVoidOmens = "TriggeredVoidOmen_";
                string voidOmen = "VoidOmen_";

                // And an array to record which variables hold which Dark Events
                int[] voidOmensinPlay = CheckFordVoidOmensInPlay(geoLevelController);
                Logger.Always("Checking if method invocation is working, these are all the Void Omens in play " + voidOmensinPlay[0] + " "
                    + voidOmensinPlay[1] + " " + voidOmensinPlay[2] + " " + voidOmensinPlay[3]);

                int replacedVoidOmen = 0;

                for (int x = 1; x < 20; x++)
                {
                    // We check, starting from the earliest, which Void Omen is still in play
                    if (voidOmensinPlay.Contains(geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x)))
                    {
                        // Then we locate in which Variable it is recorded
                        for (int y = 0; y < difficulty; y++)
                        {
                            // Once we find it, we want to remove it
                            // Added the check to skip empty Void Omen variables, to hopefully make this method work even when list is not full
                            if (voidOmensinPlay[difficulty - y - 1] == geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x) && voidOmensinPlay[difficulty - y - 1] != 0)
                            {
                                Logger.Always("The Void Omen Variable to be replaced is " + voidOmen + (difficulty - y) +
                                   " now holds VO " + geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)));
                                replacedVoidOmen = geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y));
                                geoLevelController.EventSystem.SetVariable(voidOmen + (difficulty - y), 0);
                                Logger.Always("The Void Omen Variable " + voidOmen + (difficulty - y) +
                                    " is now " + geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)));
                                y = difficulty;
                                x = 20;
                            }
                        }
                    }
                }

                if (replacedVoidOmen != 0)
                {
                    string objectiveToBeReplaced = (string)CHReworkMain.VoidOmens_Title.GetValue(replacedVoidOmen - 1);
                    Logger.Always("The target event that will be replaced is " + objectiveToBeReplaced);
                    CHReworkMain.RemoveVoidOmenObjective(objectiveToBeReplaced, geoLevelController);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void RemoveAllVoidOmens(GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                string triggeredVoidOmens = "TriggeredVoidOmen_";
                string voidOmen = "VoidOmen_";

                // And an array to record which variables hold which Dark Events
                int[] voidOmensinPlay = CheckFordVoidOmensInPlay(geoLevelController);
                Logger.Always("Checking if method invocation is working, these are all the Void Omens in play " + voidOmensinPlay[0] + " "
                    + voidOmensinPlay[1] + " " + voidOmensinPlay[2] + " " + voidOmensinPlay[3]);

                int replacedVoidOmen = 0;

                for (int x = 1; x < 20; x++)
                {
                    // We check, starting from the earliest, which Void Omen is still in play
                    if (voidOmensinPlay.Contains(geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x)))
                    {
                        // Then we locate in which Variable it is recorded
                        for (int y = 0; y < difficulty; y++)
                        {
                            // Once we find it, we want to remove it
                            if (voidOmensinPlay[difficulty - y - 1] == geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x))
                            {
                                Logger.Always("The Void Omen Variable to be replaced is " + voidOmen + (difficulty - y) +
                                   " now holds VO " + geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)));
                                replacedVoidOmen = geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y));
                                geoLevelController.EventSystem.SetVariable(voidOmen + (difficulty - y), 0);
                                Logger.Always("The Void Omen Variable " + voidOmen + (difficulty - y) +
                                    " is now " + geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)));
                            }
                        }
                    }
                    if (replacedVoidOmen != 0)
                    {
                        string objectiveToBeReplaced = (string)CHReworkMain.VoidOmens_Title.GetValue(replacedVoidOmen - 1);
                        Logger.Always("The target event that will be replaced is " + objectiveToBeReplaced);
                        CHReworkMain.RemoveVoidOmenObjective(objectiveToBeReplaced, geoLevelController);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        public static void SetUmbraRandomValue(float value)
        {
            try
            {
                RandomValueEffectConditionDef randomValueFishUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralFishmen_FactionEffectDef]"));
                randomValueFishUmbra.ThresholdValue = value;
                RandomValueEffectConditionDef randomValueCrabUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralCrabmen_FactionEffectDef]"));
                randomValueCrabUmbra.ThresholdValue = value;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }



    }
}


