using Base.Defs;
using Base.Entities.Effects;
using Base.Entities.Effects.ApplicationConditions;
using Base.UI;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.Missions.Outcomes;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Entities.Sites;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Tactical.AI;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Tactical.Levels.FactionEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class VoidOmens
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static bool[] darkEventsCheck = new bool[17];
        public static bool VoidOmen3Active = false;
        public static bool VoidOmen3Activated = false;
        public static bool VoidOmen7Active = false;
        public static bool VoidOmen15Active = false;
        public static bool VoidOmen16Active = false;
        public static bool VoidOmen12Active = false;
        public static bool VoidOmen14Active = false;



        public static void CreateVoidOmens(GeoLevelController level)
        {
            try
            {
                int difficulty = level.CurrentDifficultyLevel.Order;
                string voidOmen = "VoidOmen_";

                for (int i = 1; i < difficulty + 1; i++)
                {
                    for (int j = 1; j < darkEventsCheck.Count()-1; j++)
                    {
                        if (level.EventSystem.GetVariable(voidOmen + i) == j)
                        {
                            if (j == 1 && darkEventsCheck[j] == false)
                            {
                                level.EventSystem.ExplorationAmbushChance = 100;
                                CustomMissionTypeDef AmbushALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAlien_CustomMissionTypeDef"));
                                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                                AmbushALN.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                                AmbushALN.CratesDeploymentPointsRange.Min = 50;
                                AmbushALN.CratesDeploymentPointsRange.Max = 70;
                                Logger.Always("Exploration ambush chance is now " + level.EventSystem.ExplorationAmbushChance);
                                Logger.Always("Alien ambushes can now have a max of  " + AmbushALN.CratesDeploymentPointsRange.Max / 10 + " crates");
                                darkEventsCheck[j] = true;
                            }
                            if (j == 2 && darkEventsCheck[j] == false)
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
                                darkEventsCheck[j] = true;

                            }
                            if (j == 3 && darkEventsCheck[j] == false)
                            {
                                darkEventsCheck[j] = true;
                            }
                            if (j == 4 && darkEventsCheck[j] == false) //pending adding extra XP gain
                            {
                                foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                                {
                                    missionTypeDef.MaxPlayerUnits = 6;
                                }
                                darkEventsCheck[j] = true;
                            }
                            if (j == 5 && darkEventsCheck[j] == false)
                            {
                                foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                                {
                                    if (missionTypeDef.name.Contains("Haven") && !missionTypeDef.name.Contains("Infestation"))
                                    {
                                        TacCrateDataDef cratesNotResources = Repo.GetAllDefs<TacCrateDataDef>().FirstOrDefault(ged => ged.name.Equals("Default_TacCrateDataDef"));
                                        missionTypeDef.ParticipantsRelations[2].MutualRelation = FactionRelation.Enemy;
                                        missionTypeDef.ParticipantsData[1].PredeterminedFactionEffects = missionTypeDef.ParticipantsData[0].PredeterminedFactionEffects;
                                        missionTypeDef.MissionSpecificCrates = cratesNotResources;
                                        missionTypeDef.FactionItemsRange.Min = 2;
                                        missionTypeDef.FactionItemsRange.Max = 7;
                                        missionTypeDef.CratesDeploymentPointsRange.Min = 20;
                                        missionTypeDef.CratesDeploymentPointsRange.Max = 30;
                                    }
                                }
                                darkEventsCheck[j] = true;
                            }
                            if (j == 6 && darkEventsCheck[j] == false)
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
                                darkEventsCheck[j] = true;

                            }
                            if (j == 7 && darkEventsCheck[j] == false)
                            {
                                darkEventsCheck[j] = true;
                            }

                            if (j == 8 && darkEventsCheck[j] == false)
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
                                darkEventsCheck[j] = true;
                            }
                          
                            if (j == 9 && darkEventsCheck[j] == false)
                            {
                                FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                                festeringSkiesSettingsDef.HavenAttackCounterModifier = 0.66f;
                                darkEventsCheck[j] = true;
                            }
                            
                            if (j == 10 && darkEventsCheck[j] == false)
                            {
                                darkEventsCheck[j] = true;
                            }
                            if (j == 11 && darkEventsCheck[j] == false)
                            {
                                FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                                festeringSkiesSettingsDef.DisruptionThreshholdBaseValue = 15;
                                darkEventsCheck[j] = true;
                                
                            }
                            if (j == 12 && darkEventsCheck[j] == false)
                            {
                                VoidOmen14Active = true;
                                darkEventsCheck[j] = true;

                            }
                            if (j == 13 && darkEventsCheck[j] == false)
                            {
                                foreach (GameDifficultyLevelDef difficultyLevelDef in Repo.GetAllDefs<GameDifficultyLevelDef>()) 
                                {
                                    difficultyLevelDef.NestLimitations.MaxConcurrent = 1000;
                                    difficultyLevelDef.LairLimitations.MaxConcurrent = 1000;
                                    difficultyLevelDef.CitadelLimitations.MaxConcurrent = 1000;
                                }
                               
                                darkEventsCheck[j] = true;
                            }
                            if (j == 14 && darkEventsCheck[j] == false)
                            {
                                TacticalPerceptionDef tacticalPerceptionDef = Repo.GetAllDefs<TacticalPerceptionDef>().FirstOrDefault((TacticalPerceptionDef a) => a.name.Equals("Soldier_PerceptionDef"));
                                tacticalPerceptionDef.PerceptionRange = 20;
                                darkEventsCheck[j] = true;
                            }
                            if (j == 15 && darkEventsCheck[j] == false)
                            {
                                darkEventsCheck[j] = true;
                            }
                            if (j == 16 && darkEventsCheck[j] == false)
                            {
                                if (!darkEventsCheck[15])
                                {
                                    RandomValueEffectConditionDef randomValueCrabUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                                    FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralCrabmen_FactionEffectDef]"));
                                    randomValueCrabUmbra.ThresholdValue = 0.25f;
                                    RandomValueEffectConditionDef randomValueFishUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                                    FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralFishmen_FactionEffectDef]"));
                                    randomValueFishUmbra.ThresholdValue = 0.25f;
                                }
                                if (darkEventsCheck[15])
                                {
                                    RandomValueEffectConditionDef randomValueCrabUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                                    FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralCrabmen_FactionEffectDef]"));
                                    randomValueCrabUmbra.ThresholdValue = 0.5f;
                                    RandomValueEffectConditionDef randomValueFishUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                                    FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralFishmen_FactionEffectDef]"));
                                    randomValueFishUmbra.ThresholdValue = 0.5f;
                                }
                                darkEventsCheck[j] = true;
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
                string voidOmen = "VoidOmen_";
                int[] voidOmensInPlay = new int[difficulty];

                for (int i = 1; i < difficulty + 1; i++)
                {
                    if (level.EventSystem.GetVariable(voidOmen + i) != 0)
                    {
                        voidOmensInPlay[i - 1] = level.EventSystem.GetVariable(voidOmen + i);
                    }
                }

                for (int i = 1; i < darkEventsCheck.Count()-1; i++)
                {
                    if (!voidOmensInPlay.Contains(i) && darkEventsCheck[i])
                    {
                        if (darkEventsCheck[1])
                        {
                            level.EventSystem.ExplorationAmbushChance = 70;
                            CustomMissionTypeDef AmbushALN = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("AmbushAlien_CustomMissionTypeDef"));
                            AmbushALN.ParticipantsData[0].ReinforcementsTurns.Max = 2;
                            AmbushALN.ParticipantsData[0].ReinforcementsTurns.Min = 2;
                            AmbushALN.CratesDeploymentPointsRange.Min = 30;
                            AmbushALN.CratesDeploymentPointsRange.Max = 50;
                            darkEventsCheck[1] = false;
                            Logger.Always("Exploration ambush chance is now " + level.EventSystem.ExplorationAmbushChance);
                            Logger.Always("Alien ambushes can now have a max of  " + AmbushALN.CratesDeploymentPointsRange.Max / 10 + " crates");
                            Logger.Always("The check for VO#1 went ok");
                        }
                        if (darkEventsCheck[2])
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
                            darkEventsCheck[2] = false;
                            Logger.Always("The check for VO#2 went ok");
                        }
                        if (darkEventsCheck[3])
                        {
                        
                            darkEventsCheck[3] = false;
                            Logger.Always("The check for VO#3 went ok");
                        }

                        if (darkEventsCheck[4])
                        {
                            foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                            {
                                missionTypeDef.MaxPlayerUnits = 8;
                            }
                            darkEventsCheck[4] = false;
                            Logger.Always("The check for VO#4 went ok");
                        }

                        if (darkEventsCheck[5])
                        {
                            foreach (CustomMissionTypeDef missionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>())
                            {
                                if (missionTypeDef.name.Contains("Haven") && !missionTypeDef.name.Contains("Infestation"))
                                {
                                    TacticalFactionEffectDef defendersCanBeRecruited = Repo.GetAllDefs<TacticalFactionEffectDef>().FirstOrDefault(ged => ged.name.Equals("CanBeRecruitedByPhoenix_FactionEffectDef"));                                   
                                    missionTypeDef.ParticipantsRelations[2].MutualRelation = FactionRelation.Friend;
                                    EffectDef[] predeterminedFactionEffects = new EffectDef[1] { defendersCanBeRecruited };
                                    missionTypeDef.ParticipantsData[1].PredeterminedFactionEffects = predeterminedFactionEffects;
                                    missionTypeDef.FactionItemsRange.Min = 0;
                                    missionTypeDef.FactionItemsRange.Max = 0;
                                    missionTypeDef.CratesDeploymentPointsRange.Min = 0;
                                    missionTypeDef.CratesDeploymentPointsRange.Max = 0;
                                }
                            }
                            darkEventsCheck[5] = false;
                            Logger.Always("The check for VO#5 went ok");
                        }

                        if (darkEventsCheck[6])
                        {
                            level.CurrentDifficultyLevel.EvolutionPointsGainOnMissionLoss = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[0].EvolutionPerDestroyedBase = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[1].EvolutionPerDestroyedBase = 0;
                            level.CurrentDifficultyLevel.AlienBaseTypeEvolutionParams[2].EvolutionPerDestroyedBase = 0;
                            foreach (ResourceGeneratorFacilityComponentDef lab in Repo.GetAllDefs<ResourceGeneratorFacilityComponentDef>())
                            {
                                if (lab.name == "E_ResourceGenerator [ResearchLab_PhoenixFacilityDef]" 
                                    || lab.name == "E_ResourceGenerator [BionicsLab_PhoenixFacilityDef]" 
                                    && lab.BaseResourcesOutput.Values[1]!=null)
                                {
                                    lab.BaseResourcesOutput.Values.Remove(lab.BaseResourcesOutput.Values[1]);
                                }
                            }
                            darkEventsCheck[6] = false;
                            Logger.Always("The check for VO#6 went ok");
                        }

                        if (darkEventsCheck[7])
                        {
                            darkEventsCheck[7] = false;
                            Logger.Always("The check for VO#7 went ok");
                        }

                        if (darkEventsCheck[8])
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
                            darkEventsCheck[8] = false;
                        }

                        if (darkEventsCheck[9])
                        {
                            FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                            festeringSkiesSettingsDef.HavenAttackCounterModifier = 1.33f;
                            darkEventsCheck[9] = false;
                            Logger.Always("The check for VO#9 went ok");
                        }     
                        
                        if (darkEventsCheck[10])
                        {
                            darkEventsCheck[10] = false;
                            Logger.Always("The check for VO#10 went ok");
                        }

                        if (darkEventsCheck[11])
                        {
                            FesteringSkiesSettingsDef festeringSkiesSettingsDef = Repo.GetAllDefs<FesteringSkiesSettingsDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesSettingsDef"));
                            festeringSkiesSettingsDef.DisruptionThreshholdBaseValue = 3;
                            darkEventsCheck[11] = false;
                            Logger.Always("The check for VO#11 went ok");
                        }

                        if (darkEventsCheck[12])
                        {
                           VoidOmen14Active=false;
                            darkEventsCheck[12] = false;
                            Logger.Always("The check for VO#12 went ok");
                        }

                        if (darkEventsCheck[13])
                        {
                            foreach (GameDifficultyLevelDef difficultyLevelDef in Repo.GetAllDefs<GameDifficultyLevelDef>())
                            {
                                if (difficultyLevelDef.Order == 1)
                                {
                                    difficultyLevelDef.NestLimitations.MaxConcurrent = 2;
                                    difficultyLevelDef.LairLimitations.MaxConcurrent = 2;
                                    difficultyLevelDef.CitadelLimitations.MaxConcurrent = 2;
                                }
                                else
                                {
                                    difficultyLevelDef.NestLimitations.MaxConcurrent = 3;
                                    difficultyLevelDef.LairLimitations.MaxConcurrent = 3;
                                    difficultyLevelDef.CitadelLimitations.MaxConcurrent = 3;
                                }
                            }
                            darkEventsCheck[13] = false;
                            Logger.Always("The check for VO#13 went ok");
                        }

                        if (darkEventsCheck[14])
                        {
                            TacticalPerceptionDef tacticalPerceptionDef = Repo.GetAllDefs<TacticalPerceptionDef>().FirstOrDefault((TacticalPerceptionDef a) => a.name.Equals("Soldier_PerceptionDef"));
                            tacticalPerceptionDef.PerceptionRange = 35;
                            darkEventsCheck[14] = false;
                            Logger.Always("The check for VO#14 went ok");
                        }
                        
                        if (darkEventsCheck[15])
                        {
                            darkEventsCheck[15] = false;
                            Logger.Always("The check for VO#15 went ok");
                        }

                        if (darkEventsCheck[16])
                        {
                            RandomValueEffectConditionDef randomValueCrabUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                            FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralCrabmen_FactionEffectDef]"));
                            randomValueCrabUmbra.ThresholdValue = 0;
                            RandomValueEffectConditionDef randomValueFishUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                            FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralFishmen_FactionEffectDef]"));
                            randomValueFishUmbra.ThresholdValue = 0;

                            darkEventsCheck[16] = false;
                            Logger.Always("The check for VO#16 went ok");
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

                int difficulty = level.CurrentDifficultyLevel.Order;
                string voidOmen = "VoidOmen_";

                for (int i = 1; i < difficulty + 1; i++)
                {
                    if (level.EventSystem.GetVariable(voidOmen + i) == 3)
                    {
                        VoidOmen3Active = true;
                    }
                    if (level.EventSystem.GetVariable(voidOmen + i) == 7)
                    {
                        VoidOmen7Active = true;
                    }
                    if (level.EventSystem.GetVariable(voidOmen + i) == 9)
                    {
                        VoidOmen15Active = true;
                    }
                    if (level.EventSystem.GetVariable(voidOmen + i) == 11)
                    {
                        VoidOmen16Active = true;
                    }
                    if (level.EventSystem.GetVariable(voidOmen + i) == 12)
                    {
                        VoidOmen12Active = true;
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }  

        public static void CheckForAlreadyRolledVoidOmens(GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                string triggeredVoidOmens = "TriggeredVoidOmen_";
                string voidOmen = "VoidOmen_";

                int[] allTheDarkEventsAlreadyRolled = new int[19];
                // And an array to record which variables hold which Dark Events
                int[] allTheDarkEventsVariables = new int[difficulty];

                for (int x = 1; x < 20; x++)
                {
                    if (geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x) != 0)
                    {
                        allTheDarkEventsAlreadyRolled[x] = geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x);
                        Logger.Always("Check Triggered Void Omens " + allTheDarkEventsAlreadyRolled[x]);
                    }
                }
                int countVoidOmensY = 0;
                int countVoidOmensX = 0;
                int variablesUsed = 0;
                // Then we check our Dark Event variables to see which one has the earliest Dark Event already rolled                                
                for (int x = 1; x < 20; x++)
                {
                    // We will look through the DarkEvents variables in the order in which they were filled
                    for (int y = 0; y < difficulty; y++)
                    {
                        // And record which variable holds which Dark Event
                        if (geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)) == allTheDarkEventsAlreadyRolled[x])
                        {
                            allTheDarkEventsVariables[difficulty - y - 1] = allTheDarkEventsAlreadyRolled[x];
                            Logger.Always("Check Variable " + (difficulty - y) + " holding Void Omen " + allTheDarkEventsVariables[difficulty - y - 1]);
                            variablesUsed++;
                        }
                        countVoidOmensY++;
                    }
                    Logger.Always("the count of variables used is " + variablesUsed);
                    if (variablesUsed == difficulty)
                    {
                        x = 20;
                    }
                    countVoidOmensX++;
                }
                Logger.Always("The y loop was done " + countVoidOmensY);
                Logger.Always("The x loop was done " + countVoidOmensX);
                // Then we try to find in the array of the Dark Variables which one appeared the earliest
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void RemoveEarliestVoidOmen
        (GeoLevelController geoLevelController)
        {
            try
            {
                int difficulty = geoLevelController.CurrentDifficultyLevel.Order;
                string triggeredVoidOmens = "TriggeredVoidOmen_";
                string voidOmen = "VoidOmen_";

                int[] allTheDarkEventsAlreadyRolled = new int[19];
                // And an array to record which variables hold which Dark Events
                int[] allTheDarkEventsVariables = new int[difficulty];

                CheckForAlreadyRolledVoidOmens(geoLevelController);

                int xCounter = 0;
                int replacedVoidOmen = 0;

                for (int x = 1; x < 20; x++)
                {
                    xCounter++;
                    // We check, starting from the earliest, which Dark Event is still in play
                    if (allTheDarkEventsVariables.Contains(geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x)))
                    {
                        // Then we locate in which Variable it is recorded
                        for (int y = 0; y < difficulty; y++)
                        {
                            // Once we find it, we want to remove it
                            if (allTheDarkEventsVariables[difficulty - y - 1] == geoLevelController.EventSystem.GetVariable(triggeredVoidOmens + x))
                            {
                                replacedVoidOmen = geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y));
                                geoLevelController.EventSystem.SetVariable(voidOmen + (difficulty - y), 0);

                                Logger.Always("The Void Omen Variable " + voidOmen + (difficulty - y) +
                                    " is now " + geoLevelController.EventSystem.GetVariable(voidOmen + (difficulty - y)));
                            }
                        }
                    }
                }               

                if (replacedVoidOmen != 0)
                {
                    string objectiveToBeReplaced = (string)CHReworkMain.DarkEvents_Title.GetValue(replacedVoidOmen - 1);
                    Logger.Always("The target event that will be replaced is " + objectiveToBeReplaced);
                    CHReworkMain.RemoveDarkEventObjective(objectiveToBeReplaced, geoLevelController);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }          
        }
    }
}


