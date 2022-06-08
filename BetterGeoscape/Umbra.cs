using Base;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Effects;
using Base.Entities.Effects.ApplicationConditions;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTagsTypes;
using PhoenixPoint.Common.Entities.Items.SkinData;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Effects.ApplicationConditions;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Eventus;
using PhoenixPoint.Tactical.Levels;
using PhoenixPoint.Tactical.Levels.FactionEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    internal class Umbra
    {
        
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
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
