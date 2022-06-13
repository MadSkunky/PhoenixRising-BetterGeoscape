using Base.Defs;
using Base.Entities.Effects.ApplicationConditions;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTagsTypes;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Entities.Research.Requirement;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Tactical.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class Umbra
    {

        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static string variableUmbraALNResReq = "Umbra_Encounter_Variable";

        public static void ChangeUmbra()

        {
            try
            {
                EncounterVariableResearchRequirementDef sourceVarResReq =
                   Repo.GetAllDefs<EncounterVariableResearchRequirementDef>().
                   FirstOrDefault(ged => ged.name.Equals("NJ_Bionics1_ResearchDef_EncounterVariableResearchRequirementDef_0"));
                //Changing Umbra Crab and Triton to appear after SDI event 3;
                ResearchDef umbraCrabResearch = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("ALN_CrabmanUmbra_ResearchDef"));

                //Creating new Research Requirement, requiring a variable to be triggered  
                EncounterVariableResearchRequirementDef variableResReqUmbra = Helper.CreateDefFromClone(sourceVarResReq, "0CCC30E0-4DB1-44CD-9A60-C1C8F6588C8A", "UmbraResReqDef");
                variableResReqUmbra.VariableName = variableUmbraALNResReq;
                // This changes the Umbra reserach so that 2 conditions have to be fulfilled: 1) a) nest has to be researched, or b) exotic material has to be found
                // (because 1)a) is fufilled at start of the game, b)) is redundant but harmless), and 2) a special variable has to be triggered, assigned to event sdi3
                umbraCrabResearch.RevealRequirements.Operation = ResearchContainerOperation.ALL;
                umbraCrabResearch.RevealRequirements.Container[0].Operation = ResearchContainerOperation.ANY;
                umbraCrabResearch.RevealRequirements.Container[1].Requirements[0] = variableResReqUmbra;
                //Now same thing for Triton Umbra, but it will use same variable because we want them to appear at the same time
                ResearchDef umbraFishResearch = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("ALN_FishmanUmbra_ResearchDef"));
                umbraFishResearch.RevealRequirements.Operation = ResearchContainerOperation.ALL;
                umbraFishResearch.RevealRequirements.Container[0].Operation = ResearchContainerOperation.ANY;
                umbraFishResearch.RevealRequirements.Container[1].Requirements[0] = variableResReqUmbra;
                //Because Triton research has 2 requirements in the second container, we set them to any
                umbraFishResearch.RevealRequirements.Container[1].Operation = ResearchContainerOperation.ANY;

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }


        }

        public static void AddArthronUmbraDeathBelcherAbility(TacticalActor tacticalActor)

        {
            try
            {
                AddAbilityStatusDef oilCrabAbility =
                      Repo.GetAllDefs<AddAbilityStatusDef>().FirstOrDefault
                      (ged => ged.name.Equals("OilCrab_AddAbilityStatusDef"));
                tacticalActor.Status.ApplyStatus(oilCrabAbility);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

        public static void AddTritonUmbraDeathBelcherAbility(TacticalActor tacticalActor)

        {
            try
            {
                AddAbilityStatusDef oilTritonAbility =
                     Repo.GetAllDefs<AddAbilityStatusDef>().FirstOrDefault
                     (ged => ged.name.Equals("OilFish_AddAbilityStatusDef"));
                tacticalActor.Status.ApplyStatus(oilTritonAbility);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static void UmbraEvolution(int healthPoints, int standardDamageAttack, int pierceDamageAttack)
        {
            WeaponDef umbraCrab = Repo.GetAllDefs<WeaponDef>().
            FirstOrDefault(ged => ged.name.Equals("Oilcrab_Torso_BodyPartDef"));
            umbraCrab.HitPoints = healthPoints;
            umbraCrab.DamagePayload.DamageKeywords[0].Value = standardDamageAttack;
            umbraCrab.DamagePayload.DamageKeywords[1].Value = pierceDamageAttack;
            BodyPartAspectDef umbraCrabBodyAspect = Repo.GetAllDefs<BodyPartAspectDef>().
            FirstOrDefault(ged => ged.name.Equals("E_BodyPartAspect [Oilcrab_Torso_BodyPartDef]"));
            umbraCrabBodyAspect.Endurance = (healthPoints / 10);
            WeaponDef umbraFish = Repo.GetAllDefs<WeaponDef>().
            FirstOrDefault(ged => ged.name.Equals("Oilfish_Torso_BodyPartDef"));
            umbraFish.HitPoints = healthPoints;
            umbraFish.DamagePayload.DamageKeywords[0].Value = standardDamageAttack;
            umbraFish.DamagePayload.DamageKeywords[1].Value = pierceDamageAttack;
            BodyPartAspectDef umbraFishBodyAspect = Repo.GetAllDefs<BodyPartAspectDef>().
            FirstOrDefault(ged => ged.name.Equals("E_BodyPartAspect [Oilfish_Torso_BodyPartDef]"));
            umbraFishBodyAspect.Endurance = (healthPoints / 10);
        }

        public static void SetUmbraEvolution(GeoLevelController level) 
        {

            if (level.EventSystem.GetVariable(variableUmbraALNResReq) == 2)
            {
                UmbraEvolution(125 * level.CurrentDifficultyLevel.Order, 20 * level.CurrentDifficultyLevel.Order, 20);
            }
            else if (level.EventSystem.GetVariable(variableUmbraALNResReq) == 1)
            {
                UmbraEvolution(80 * level.CurrentDifficultyLevel.Order, 20 * level.CurrentDifficultyLevel.Order, 0);
            }
        }

        public static int totalCharactersWithDelirium;
        public static int totalDeliriumOnMission;

        [HarmonyPatch(typeof(PhoenixStatisticsManager), "NewTurnEvent")]
        public static class PhoenixStatisticsManager_NewTurnEvent_CalculateDelirium_Patch
        {
            public static void Postfix(TacticalFaction prevFaction, TacticalFaction nextFaction)
            {

                try
                {
                    if (!VoidOmens.VoidOmen16Active)
                    {
                        ClassTagDef crabTag = Repo.GetAllDefs<ClassTagDef>().FirstOrDefault
                       (ged => ged.name.Equals("Crabman_ClassTagDef"));
                        ClassTagDef fishTag = Repo.GetAllDefs<ClassTagDef>().FirstOrDefault
                       (ged => ged.name.Equals("Fishman_ClassTagDef"));

                        if (prevFaction.IsControlledByPlayer)
                        {
                            totalCharactersWithDelirium = 0;
                            totalDeliriumOnMission = 0;

                            foreach (TacticalActor actor in nextFaction.TacticalActors)
                            {
                                if (actor.CharacterStats.Corruption.Value > 0)
                                {
                                    totalCharactersWithDelirium++;
                                    totalDeliriumOnMission += (int)actor.CharacterStats.Corruption.Value.BaseValue;

                                }
                            }
                        }
                        Logger.Always("Total Delirium on mission is " + totalDeliriumOnMission);
                        Logger.Always("Number of characters with Delirium is " + totalCharactersWithDelirium);
                        if (!prevFaction.IsControlledByPlayer)
                        {
                            Logger.Always("The prevFaction is " + prevFaction.Faction.FactionDef.name);
                            Logger.Always("Total Delirium on mission is " + totalDeliriumOnMission);
                            Logger.Always("Number of characters with Delirium is " + totalCharactersWithDelirium);
                            if (totalDeliriumOnMission >= 10 || totalCharactersWithDelirium >= 5)
                            {

                                DeathBelcherAbilityDef oilcrabDeathBelcherAbility =
                               Repo.GetAllDefs<DeathBelcherAbilityDef>().FirstOrDefault
                               (ged => ged.name.Equals("Oilcrab_Die_DeathBelcher_AbilityDef"));

                                DeathBelcherAbilityDef oilfishDeathBelcherAbility =
                               Repo.GetAllDefs<DeathBelcherAbilityDef>().FirstOrDefault
                               (ged => ged.name.Equals("Oilfish_Die_DeathBelcher_AbilityDef"));

                                foreach (TacticalActor actor in nextFaction.TacticalActors)
                                {
                                    Logger.Always("The next faction is " + nextFaction.Faction.FactionDef.name);
                                    Logger.Always("The actor is " + actor.name);
                                    if (actor.GameTags.Contains(crabTag) && actor.GetAbilityWithDef<DeathBelcherAbility>(oilcrabDeathBelcherAbility) == null
                                        && !actor.name.Contains("Oilcrab"))

                                    {
                                        int roll = UnityEngine.Random.Range(0, 100);
                                        if (VoidOmens.VoidOmen15Active && roll >= 68)
                                        {
                                            Logger.Always("This Arthron here " + actor + ", got past the crabtag and the blecher ability check!");
                                            AddArthronUmbraDeathBelcherAbility(actor);
                                        }
                                        else if (!VoidOmens.VoidOmen15Active && roll >= 84)
                                        {
                                            Logger.Always("This Arthron here " + actor + ", got past the crabtag and the blecher ability check!");
                                            AddArthronUmbraDeathBelcherAbility(actor);
                                        }

                                    }
                                    if (actor.GameTags.Contains(fishTag) && actor.GetAbilityWithDef<DeathBelcherAbility>(oilfishDeathBelcherAbility) == null
                                        && !actor.name.Contains("Oilfish"))
                                    {
                                        int roll = UnityEngine.Random.Range(0, 100);
                                        if (VoidOmens.VoidOmen15Active && roll >= 68)
                                        {
                                            Logger.Always("This Triton here " + actor + ", got past the crabtag and the blecher ability check!");
                                            AddTritonUmbraDeathBelcherAbility(actor);
                                        }
                                        else if (!VoidOmens.VoidOmen15Active && roll >= 84)
                                        {
                                            Logger.Always("This Triton here " + actor + ", got past the crabtag and the blecher ability check!");
                                            AddTritonUmbraDeathBelcherAbility(actor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        RandomValueEffectConditionDef randomValueCrabUmbra = Repo.GetAllDefs<RandomValueEffectConditionDef>().
                        FirstOrDefault(ged => ged.name.Equals("E_RandomValue [UmbralCrabmen_FactionEffectDef]"));
                        Logger.Always("The randon Crab Umbra value is " + randomValueCrabUmbra.ThresholdValue);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        //Patch to prevent Umbras from attacking characters without Delirium
        [HarmonyPatch(typeof(TacticalAbility), "GetTargetActors", new Type[] { typeof(TacticalTargetData), typeof(TacticalActorBase), typeof(Vector3) })]
        public static class TacticalAbility_GetTargetActors_Patch
        {
            public static void Postfix(ref IEnumerable<TacticalAbilityTarget> __result, TacticalActorBase sourceActor)
            {
                try
                {
                    if (!VoidOmens.VoidOmen16Active)
                    {
                        if (sourceActor.ActorDef.name.Equals("Oilcrab_ActorDef") || sourceActor.ActorDef.name.Equals("Oilfish_ActorDef"))
                        {
                            List<TacticalAbilityTarget> list = new List<TacticalAbilityTarget>(); // = __result.ToList();
                                                                                                  //list.RemoveWhere(adilityTarget => (adilityTarget.Actor as TacticalActor)?.CharacterStats.Corruption <= 0);
                            foreach (TacticalAbilityTarget source in __result)
                            {
                                if (source.Actor is TacticalActor && (source.Actor as TacticalActor).CharacterStats.Corruption > 0)
                                {
                                    list.Add(source);
                                }
                            }
                            __result = list;
                        }
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
