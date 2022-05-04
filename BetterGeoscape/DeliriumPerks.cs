
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Effects;
using Base.Entities.Statuses;
using Base.UI;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Effects.DamageTypes;
using PhoenixPoint.Tactical.Entities.Effects;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PhoenixPoint.Tactical.Levels;

namespace PhoenixRising.BetterGeoscape
{
    public class DeliriumPerks
    {

        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        internal static bool doNotLocalize = false;

        public static void Main()
        {
            Create_ShutEye();
            Create_Photophobia();
            Create_AngerIssues();
            Create_Hallucinating();
            Create_OneOfUs();
            Create_FleshEater();
            Create_Nails();
            Create_Immortality();
            Create_Feral();
        }

        public static void Create_ShutEye()
        {
            string skillName = "ShutEye_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("SelfDefenseSpecialist_AbilityDef"));
            PassiveModifierAbilityDef shutEye = Helper.CreateDefFromClone(
                source,
                "95431c82-a525-4975-a8da-9add9799a340",
                skillName);
            shutEye.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "69bbcec5-d491-4e7e-85a2-1063716f4532",
                skillName);
            shutEye.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "28d440ee-c254-427a-b0a9-fe62a25faeac",
                skillName);
            shutEye.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Perception,
                    Modification = StatModificationType.Add,
                    Value = -10
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.HearingRange,
                    Modification = StatModificationType.Add,
                    Value = 10
                },
              };
            shutEye.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            shutEye.ViewElementDef.DisplayName1 = new LocalizedTextBind("SHUT EYE", doNotLocalize);
            shutEye.ViewElementDef.Description = new LocalizedTextBind("<b>-10 Perception, +10 Hearing Range</b>\n<i>Exibiting rare form of paranoia and claustrophobia, the subject often found with his eyes closed, " +
            "claiming to see with his inner eye </i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Volunteered_1-2.png");
            shutEye.ViewElementDef.LargeIcon = icon;
            shutEye.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Hallucinating()
        {
            string skillName = "Hallucinating_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("SelfDefenseSpecialist_AbilityDef"));
            PassiveModifierAbilityDef hallucinating = Helper.CreateDefFromClone(
                source,
                "5d3421cb-9e22-4cdf-bcac-3beac61b2713",
                skillName);
            hallucinating.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "92560850-084c-4d43-8c57-a4f5773e4a26",
                skillName);
            hallucinating.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "b8c58fc2-c56e-4577-a187-c0922cba8468",
                skillName);
            hallucinating.StatModifications = new ItemStatModification[0];
            hallucinating.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            hallucinating.ViewElementDef.DisplayName1 = new LocalizedTextBind("HALLUCINATING", doNotLocalize);
            hallucinating.ViewElementDef.Description = new LocalizedTextBind("<b>Start each mission Disoriented for 2 turns</b>\n<i>So far observation show subject mostly harmless to himself or the surrounding, " +
                "however it is not recommended to deploy on tactical missions</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Paranoid_2-1.png");
            hallucinating.ViewElementDef.LargeIcon = icon;
            hallucinating.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_FleshEater()
        {
            string skillName = "FleshEater_AbilityDef";
            ApplyStatusAbilityDef source = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(p => p.name.Equals("Inspire_AbilityDef"));
            ApplyStatusAbilityDef fleshEater = Helper.CreateDefFromClone(
                source,
                "0319cf53-65d2-4964-98d2-08c1acb54b24",
                skillName);
            fleshEater.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "b101c95b-cd35-4649-9983-2662a454e40f",
                skillName);
            fleshEater.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "ed164c5a-2927-422a-a086-8762137d4c5d",
                skillName);

            OnActorDeathEffectStatusDef fleshEaterStatus = Helper.CreateDefFromClone(
                fleshEater.StatusDef as OnActorDeathEffectStatusDef,
                "ac7195f9-c382-4f79-a956-55d5eb3b6371",
                "E_KillListenerStatus [" + skillName + "]");

            FactionMembersEffectDef fleshEaterEffectDef2 = Helper.CreateDefFromClone(
                fleshEaterStatus.EffectDef as FactionMembersEffectDef,
                "8bd34f58-d452-4f38-975e-4f32b33d283d",
                "E_Effect [" + skillName + "]");

            StatsModifyEffectDef fleshEaterSingleEffectDef2 = Helper.CreateDefFromClone(
                fleshEaterEffectDef2.SingleTargetEffect as StatsModifyEffectDef,
                "ad0891cf-fe7a-443f-acb9-575c3cf23432",
                "E_SingleTargetEffect [" + skillName + "]");


            //(fleshEater.StatusDef as OnActorDeathEffectStatusDef).EffectDef = fleshEaterEffectDef;
            //fleshEaterEffectDef.SingleTargetEffect = fleshEaterSingleTargetEffectDef;
            //fleshEaterSingleTargetEffectDef.StatModifications[0].StatName = "";
            fleshEater.StatusDef = fleshEaterStatus;
            fleshEaterStatus.EffectDef = fleshEaterEffectDef2;
            fleshEaterEffectDef2.SingleTargetEffect = fleshEaterSingleEffectDef2;

            fleshEaterSingleEffectDef2.StatModifications = new List<StatModification>
            {
                new StatModification()
                {
                    Modification = StatModificationType.AddRestrictedToBounds,
                    StatName = "WillPoints",
                    Value = -2,
                }
            };

            fleshEater.ViewElementDef.DisplayName1 = new LocalizedTextBind("FLESH EATER", doNotLocalize);
            fleshEater.ViewElementDef.Description = new LocalizedTextBind("<b>You gain Devour ability. Your allies lose 2 WP each time you kill an enemy.</b>\n<i>It is unclear whether this is some part of pagan ritual or pure insanity, but so far individual " +
                "combat efficiency of the unit has grown by 46%</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_Mutog_Devour.png");
            fleshEater.ViewElementDef.LargeIcon = icon;
            fleshEater.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_AngerIssues()
        {
            string skillName = "AngerIssues_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef angerIssues = Helper.CreateDefFromClone(
                source,
                "c1a545b3-eb5d-47f0-bf59-82710415d559",
                skillName);
            angerIssues.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "561c23c1-ce46-4862-b49f-0fd3656cdefc",
                skillName);
            angerIssues.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "da704d9c-354c-4e2b-a61d-af3b23f47522",
                skillName);
            angerIssues.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Stealth,
                    Modification = StatModificationType.Add,
                    Value = -0.25f
                },
              };
            angerIssues.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            angerIssues.ViewElementDef.DisplayName1 = new LocalizedTextBind("ANGER ISSUES", doNotLocalize);
            angerIssues.ViewElementDef.Description = new LocalizedTextBind("<b>Start each mission Frenzied for 2 turns, -25% Stealth</b>\n<i>Subject shows signs of violent outbursts, it is recommended to keep him isolated unless deployed for combat</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Warcry.png");
            angerIssues.ViewElementDef.LargeIcon = icon;
            angerIssues.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Photophobia()
        {
            string skillName = "Photophobia_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef photophobia = Helper.CreateDefFromClone(
                source,
                "42399bdf-b43b-40f4-a471-89d082a31fde",
                skillName);
            photophobia.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "7e8fff90-a757-4794-81a9-a90cb97cb325",
                skillName);
            photophobia.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "2e4f7cec-80de-423c-914d-865700949a93",
                skillName);
            photophobia.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Speed,
                    Modification = StatModificationType.Add,
                    Value = -2
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Stealth,
                    Modification = StatModificationType.Add,
                    Value = 0.25f
                },
              };
            photophobia.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            photophobia.ViewElementDef.DisplayName1 = new LocalizedTextBind("PHOTOPHOBIA", doNotLocalize);
            photophobia.ViewElementDef.Description = new LocalizedTextBind("<b>Speed reduced -2, Stealth Increased +25%</b>\n<i>Acting erratically the subject seem to always reconsider his next step, trying to stay outside of lit areas</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_NightOwl.png");
            photophobia.ViewElementDef.LargeIcon = icon;
            photophobia.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Nails()
        {
            string skillName = "Nails_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Cautious_AbilityDef"));
            PassiveModifierAbilityDef nails = Helper.CreateDefFromClone(
                source,
                "b3185867-ca87-4e59-af6d-012267a7bd25",
                skillName);
            nails.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "3e57b19b-11e1-42b9-81f4-c9cc9fffc42d",
                skillName);
            nails.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "3f170800-b819-4237-80a3-c9b9daa9dab4",
                skillName);
            nails.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Accuracy,
                    Modification = StatModificationType.Add,
                    Value = -0.2f
                },
              };
            nails.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            nails.ViewElementDef.DisplayName1 = new LocalizedTextBind("NAILS", doNotLocalize);
            nails.ViewElementDef.Description = new LocalizedTextBind("<b>You gain Slashing Strike, but -20% accuracyh</b>\n<i>Demonstrating signs of schizophrenia, this subject refuse to clip his nails and sharpens them to form monster " +
                "claws, which naturally obstructs proper handling of firearm weapons </i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Siren_SlasherHands.png");
            nails.ViewElementDef.LargeIcon = icon;
            nails.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_OneOfUs()
        {
            string skillName = "OneOfUs_AbilityDef";
            DamageMultiplierAbilityDef source = Repo.GetAllDefs<DamageMultiplierAbilityDef>().FirstOrDefault(p => p.name.Equals("VirusResistant_DamageMultiplierAbilityDef"));
            DamageMultiplierAbilityDef oneOfUs = Helper.CreateDefFromClone(
                source,
                "d4f5f9f2-43b6-4c3e-a5db-78a7a9cccd3e",
                skillName);
            oneOfUs.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "569a8f7b-41bf-4a0c-93ce-d96006f4ed27",
                skillName);
            oneOfUs.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "3cc4d8c8-739c-403b-92c9-7a6f5c54abb5",
                skillName);


            oneOfUs.DamageTypeDef = Repo.GetAllDefs<DamageTypeBaseEffectDef>().FirstOrDefault(dtb => dtb.name.Equals("Mist_SpawnVoxelDamageTypeEffectDef"));
            oneOfUs.Multiplier = 0;

            oneOfUs.ViewElementDef.DisplayName1 = new LocalizedTextBind("ONE OF US", doNotLocalize);
            oneOfUs.ViewElementDef.Description = new LocalizedTextBind("<b>Willpower reduced -2, Mist affects you as if you were a Pandoran</b>\n<i>Often the last to leave the mission, wandering ruined landscapes the subject claims the mist " +
                "calls out to him</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Sower_Of_Change_1-2.png");
            oneOfUs.ViewElementDef.LargeIcon = icon;
            oneOfUs.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Immortality()
        {
            string skillName = "Immortality_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef immortality = Helper.CreateDefFromClone(
                source,
                "51ddff8e-49d0-4cca-8f4f-53aa39fcbce9",
                skillName);
            immortality.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "3efc6f6b-8c57-405b-afe4-f20491336bd5",
                skillName);
            immortality.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "604181c6-fd18-46be-a3af-0b756a8200f1",
                skillName);
            immortality.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Endurance,
                    Modification = StatModificationType.Add,
                    Value = -4
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Endurance,
                    Modification = StatModificationType.AddMax,
                    Value = -4
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Armour,
                    Modification = StatModificationType.AddRestrictedToBounds,
                    Value = 10
                },
              };
            immortality.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            immortality.ViewElementDef.DisplayName1 = new LocalizedTextBind("IMMORTALITY", doNotLocalize);
            immortality.ViewElementDef.Description = new LocalizedTextBind("<b>Strength reduced -4, Gain 10 natural Armour</b>\n<i>Self-mutilation is not uncommon to develop throughout Delirium affected subjects," +
                " this one in particular believes he has become Immortal</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Vampire.png");
            immortality.ViewElementDef.LargeIcon = icon;
            immortality.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Feral()
        {
            string skillName = "Feral_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef feral = Helper.CreateDefFromClone(
                source,
                "34612505-8512-4eb3-8429-ef087c07c764",
                skillName);
            feral.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "75660746-2f27-41d1-97e3-f0d6340e96b7",
                skillName);
            feral.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "1135128c-a10d-4285-9d03-d93a4afd6733",
                skillName);
            feral.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Endurance,
                    Modification = StatModificationType.Add,
                    Value = 2
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Endurance,
                    Modification = StatModificationType.AddMax,
                    Value = 2
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Willpower,
                    Modification = StatModificationType.Add,
                    Value = -4
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Willpower,
                    Modification = StatModificationType.AddMax,
                    Value = -4
                },
              };
            feral.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            feral.ViewElementDef.DisplayName1 = new LocalizedTextBind("FERAL", doNotLocalize);
            feral.ViewElementDef.Description = new LocalizedTextBind("<b>Strength Increased +2, Gains Leap  (same as Mutog), Willpower Reduced -4</b>\n<i>Suffering personality disorder, the subject degrade to animalistic behaviour and state of mind</i>", doNotLocalize);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_Mutog_Enrage.png");
            feral.ViewElementDef.LargeIcon = icon;
            feral.ViewElementDef.SmallIcon = icon;
        }
    }

    [HarmonyPatch(typeof(TacticalLevelController), "OnLevelStart")]
    public static class TacticalLevelController_OnLevelStart_Patch
    {
        public static void Postfix(TacticalLevelController __instance)
        {
            DefRepository Repo = GameUtl.GameComponent<DefRepository>();
            try
            {
                foreach (TacticalFaction faction in __instance.Factions)
                {
                    if (faction.IsViewerFaction)
                    {
                        foreach (TacticalActor actor in faction.TacticalActors)
                        {
                            TacticalAbilityDef abilityDef = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("AngerIssues_AbilityDef"));
                            if (actor.GetAbilityWithDef<Ability>(abilityDef) != null)
                            {
                                actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("Frenzy_StatusDef")));
                            }

                            TacticalAbilityDef abilityDef1 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Hallucinating_AbilityDef"));
                            if (actor.GetAbilityWithDef<Ability>(abilityDef1) != null)
                            {
                                actor.Status.ApplyStatus(Repo.GetAllDefs<StatusDef>().FirstOrDefault(sd => sd.name.Equals("E_Status [NeuralDisruption_AbilityDef]")));
                            }

                            TacticalAbilityDef abilityDef2 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("FleshEater_AbilityDef"));
                            if (actor.GetAbilityWithDef<Ability>(abilityDef2) != null)
                            {
                                actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutog_PrimalInstinct_AbilityDef")), actor);
                            }
                            /*
                             TacticalAbilityDef abilityDef3 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("OneOfUs_AbilityDef"));
                             if (actor.GetAbilityWithDef<Ability>(abilityDef3) != null)
                             {
                                 actor.CharacterStats.WillPoints.Add(-2);
                             }                           
                            */
                            TacticalAbilityDef abilityDef5 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Nails_AbilityDef"));
                            if (actor.GetAbilityWithDef<Ability>(abilityDef5) != null)
                            {
                                actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutoid_Adapt_RightArm_Slasher_AbilityDef")), actor);
                            }
                            TacticalAbilityDef abilityDef6 = Repo.GetAllDefs<TacticalAbilityDef>().FirstOrDefault(tad => tad.name.Equals("Nails_AbilityDef"));
                            if (actor.GetAbilityWithDef<Ability>(abilityDef6) != null)
                            {
                                actor.AddAbility(Repo.GetAllDefs<AbilityDef>().FirstOrDefault(sd => sd.name.Equals("Mutog_Leap_AbilityDef")), actor);
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
}
