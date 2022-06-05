
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
using PhoenixPoint.Common.UI;

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
            Create_OneOfUsPassive();
            Create_FleshEater();
            Create_Nails();
            Create_Immortality();
            Create_Feral();
            Create_Solipsism();
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
            shutEye.ViewElementDef.DisplayName1 = new LocalizedTextBind("SHUT EYE", true);
            shutEye.ViewElementDef.Description = new LocalizedTextBind("<b>-10 Perception, +10 Hearing Range</b>\n<i>Exibiting rare form of paranoia and claustrophobia, the subject often found with his eyes closed, " +
            "claiming to see with his inner eye </i>", true);
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
            hallucinating.ViewElementDef.DisplayName1 = new LocalizedTextBind("HALLUCINATING", true);
            hallucinating.ViewElementDef.Description = new LocalizedTextBind("<b>Start each mission Disoriented for 2 turns</b>\n<i>So far observation show subject mostly harmless to himself or the surrounding, " +
                "however it is not recommended to deploy on tactical missions</i>", true);
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

            fleshEater.ViewElementDef.DisplayName1 = new LocalizedTextBind("FLESH EATER", true);
            fleshEater.ViewElementDef.Description = new LocalizedTextBind("<b>You gain Devour ability. Your allies lose 2 WP each time you kill an enemy.</b>\n<i>It is unclear whether this is some part of pagan ritual or pure insanity, but so far individual " +
                "combat efficiency of the unit has grown by 46%</i>", true);
            Sprite icon = Repo.GetAllDefs<TacticalAbilityViewElementDef>().FirstOrDefault(tav => tav.name.Equals("E_ViewElement [Mutog_Devour_AbilityDef]")).LargeIcon;
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
            angerIssues.ViewElementDef.DisplayName1 = new LocalizedTextBind("ANGER ISSUES", true);
            angerIssues.ViewElementDef.Description = new LocalizedTextBind("<b>Start each mission Frenzied for 2 turns, -25% Stealth</b>\n<i>Subject shows signs of violent outbursts, it is recommended to keep him isolated unless deployed for combat</i>", true);
            Sprite icon = Repo.GetAllDefs<TacticalAbilityViewElementDef>().FirstOrDefault(tav => tav.name.Equals("E_View [WarCry_AbilityDef]")).LargeIcon;
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
            photophobia.ViewElementDef.DisplayName1 = new LocalizedTextBind("PHOTOPHOBIA", true);
            photophobia.ViewElementDef.Description = new LocalizedTextBind("<b>Speed reduced -2, Stealth Increased +25%</b>\n<i>Acting erratically the subject seem to always reconsider his next step, trying to stay outside of lit areas</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_NightOwl.png");
            photophobia.ViewElementDef.LargeIcon = icon;
            photophobia.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_NailsPassive()
        {
            string skillName = "NailsPassive_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Cautious_AbilityDef"));
            PassiveModifierAbilityDef nailsPassive = Helper.CreateDefFromClone(
                source,
                "b3185867-ca87-4e59-af6d-012267a7bd25",
                skillName);
            nailsPassive.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "3e57b19b-11e1-42b9-81f4-c9cc9fffc42d",
                skillName);
            nailsPassive.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "3f170800-b819-4237-80a3-c9b9daa9dab4",
                skillName);
            nailsPassive.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Accuracy,
                    Modification = StatModificationType.Add,
                    Value = -0.2f
                },
              };
            nailsPassive.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            nailsPassive.ViewElementDef.DisplayName1 = new LocalizedTextBind("NAILS", true);
            nailsPassive.ViewElementDef.Description = new LocalizedTextBind("<b>You gain Slashing Strike, but -20% accuracyh</b>\n<i>Demonstrating signs of schizophrenia, this subject refuse to clip his nails and sharpens them to form monster " +
                "claws, which naturally obstructs proper handling of firearm weapons </i>", true);
            Sprite icon = Repo.GetAllDefs<TacticalAbilityViewElementDef>().FirstOrDefault(tav => tav.name.Equals("E_ViewElement [Mutoid_SlashingStrike_AbilityDef]")).SmallIcon;
            nailsPassive.ViewElementDef.LargeIcon = icon;
            nailsPassive.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Nails()
        {
            string skillName = "Nails_AbilityDef";
            ApplyStatusAbilityDef source = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(p => p.name.Equals("Mutoid_Adapt_RightArm_Slasher_AbilityDef"));
            ApplyStatusAbilityDef nails = Helper.CreateDefFromClone(
                source,
                "bb65ab9c-94ae-4878-b999-e04946f720aa",
                skillName);
            nails.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "c050760d-1fb7-4b25-9295-00d98aedad19",
                skillName);
            nails.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "e9bd7acb-6955-414b-a2de-7544c38b7b6e",
                skillName);

            nails.ViewElementDef.DisplayName1 = new LocalizedTextBind("NAILS", true);
            nails.ViewElementDef.Description = new LocalizedTextBind("<b>You gain Slashing Strike, but -20% accuracyh</b>\n<i>Demonstrating signs of schizophrenia, this subject refuse to clip his nails and sharpens them to form monster " +
                "claws, which naturally obstructs proper handling of firearm weapons </i>", true);
            Sprite icon = Repo.GetAllDefs<TacticalAbilityViewElementDef>().FirstOrDefault(tav => tav.name.Equals("E_ViewElement [Mutoid_SlashingStrike_AbilityDef]")).SmallIcon;
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

            oneOfUs.ViewElementDef.DisplayName1 = new LocalizedTextBind("ONE OF US", true);
            oneOfUs.ViewElementDef.Description = new LocalizedTextBind("<b>Willpower reduced -2, Mist affects you as if you were a Pandoran</b>\n<i>Often the last to leave the mission, wandering ruined landscapes the subject claims the mist " +
                "calls out to him</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Sower_Of_Change_1-2.png");
            oneOfUs.ViewElementDef.LargeIcon = icon;
            oneOfUs.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_OneOfUsPassive()
        {
            string skillName = "OneOfUsPassive_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef ofuPassive = Helper.CreateDefFromClone(
                source,
                "ff35f9ef-ad67-42ff-9dcd-0288dba4d636",
                skillName);
            ofuPassive.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "61e44215-fc05-4383-b9e4-17f384e3d003",
                skillName);
            ofuPassive.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "aaead24e-9dba-4ef7-ba2d-8df142cb9105",
                skillName);

            ofuPassive.StatModifications = new ItemStatModification[]
              {
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Willpower,
                    Modification = StatModificationType.Add,
                    Value = -2
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Willpower,
                    Modification = StatModificationType.AddMax,
                    Value = -2
                },
              };

            DamageMultiplierStatusDef mistResistance = Repo.GetAllDefs<DamageMultiplierStatusDef>().FirstOrDefault(a => a.name.Contains("MistResistance_StatusDef"));
            mistResistance.Multiplier = 0.0f;
            ofuPassive.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            ofuPassive.ViewElementDef.DisplayName1 = new LocalizedTextBind("ONE OF US", true);
            ofuPassive.ViewElementDef.Description = new LocalizedTextBind("<b>Willpower reduced -2, Mist affects you as if you were a Pandoran</b>\n<i>Often the last to leave the mission, wandering ruined landscapes the subject claims the mist " +
                "calls out to him</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Sower_Of_Change_1-2.png");
            ofuPassive.ViewElementDef.LargeIcon = icon;
            ofuPassive.ViewElementDef.SmallIcon = icon;
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
                    Value = -5,
                },
                new ItemStatModification()
                {
                    TargetStat = StatModificationTarget.Endurance,
                    Modification = StatModificationType.AddMax,
                    Value = -5,
                },
              };
            immortality.ItemTagStatModifications = new EquipmentItemTagStatModification[0];
            immortality.ViewElementDef.DisplayName1 = new LocalizedTextBind("IMMORTALITY", true);
            immortality.ViewElementDef.Description = new LocalizedTextBind("<b>Strength reduced -5, Disabled limbs remain functional</b>\n<i>Self-mutilation is not uncommon to develop throughout Delirium affected subjects," +
                " this one in particular believes he has become Immortal</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Vampire.png");
            immortality.ViewElementDef.LargeIcon = icon;
            immortality.ViewElementDef.SmallIcon = icon;
        }
        /*
        public static void Create_Immortality2()
        {
            string skillName = "Immortality2_AbilityDef";
            ApplyStatusAbilityDef source = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(p => p.name.Equals("IgnorePain_AbilityDef"));
            ApplyStatusAbilityDef immortality = Helper.CreateDefFromClone(
                source,
                "eea26659-d54f-48d8-8025-cb7ca53c1749",
                skillName);
            immortality.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "d99c2d2f-0cff-412c-ad99-218b39158c88",
                skillName);
            immortality.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "3f8b13e1-70ff-4964-923d-1e2c73f66f4f",
                skillName);

            immortality.ViewElementDef.DisplayName1 = new LocalizedTextBind("IMMORTALITY", true);
            immortality.ViewElementDef.Description = new LocalizedTextBind("<b>Strength reduced -4, Gain 10 natural Armour</b>\n<i>Self-mutilation is not uncommon to develop throughout Delirium affected subjects," +
                " this one in particular believes he has become Immortal</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Vampire.png");
            immortality.ViewElementDef.LargeIcon = icon;
            immortality.ViewElementDef.SmallIcon = icon;
        }
        */
        public static void Create_Feral()
        {
            string skillName = "Feral_AbilityDef";
            ApplyStatusAbilityDef source = Repo.GetAllDefs<ApplyStatusAbilityDef>().FirstOrDefault(p => p.name.Equals("RapidClearance_AbilityDef"));
            ApplyStatusAbilityDef feral = Helper.CreateDefFromClone(
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
            OnActorDeathEffectStatusDef feralStatusDef = Helper.CreateDefFromClone(
                Repo.GetAllDefs<OnActorDeathEffectStatusDef>().FirstOrDefault(a => a.name.Equals("E_RapidClearanceStatus [RapidClearance_AbilityDef]")),
                "9510c7e3-bef7-4b89-b20a-3bb57a7e664b",
                "E_FeralStatus [Feral_AbilityDef]");
            ProcessDeathReportEffectDef feralEffectDef = Helper.CreateDefFromClone(
                Repo.GetAllDefs<ProcessDeathReportEffectDef>().FirstOrDefault(a => a.name.Equals("E_Effect [RapidClearance_AbilityDef]")),
                "d0f71701-4255-4b57-a387-0f3c936ed29e",
                "E_Effect [Feral_AbilityDef]");

            feral.StatusApplicationTrigger = StatusApplicationTrigger.ActorEnterPlay;
            feral.Active = false;
            feral.WillPointCost = 0;

            feral.StatusDef = feralStatusDef;
            feralStatusDef.EffectDef = feralEffectDef;
            feralEffectDef.RestoreActionPointsFraction = 0.25f;

            feral.ViewElementDef.DisplayName1 = new LocalizedTextBind("FERAL", true);
            feral.ViewElementDef.Description = new LocalizedTextBind("<b>Your attacks have 10% chance to fumble but you recover 1 AP when you kill an enemy</b>\n<i>Suffering personality disorder, the subject degrade to animalistic behaviour and state of mind</i>", true);
            Sprite icon = Repo.GetAllDefs<TacticalAbilityViewElementDef>().FirstOrDefault(tav => tav.name.Equals("E_ViewElement [Mutog_PrimalInstinct_AbilityDef]")).LargeIcon;
            feral.ViewElementDef.LargeIcon = icon;
            feral.ViewElementDef.SmallIcon = icon;
        }
        public static void Create_Solipsism()
        {
            string skillName = "Solipsism_AbilityDef";
            PassiveModifierAbilityDef source = Repo.GetAllDefs<PassiveModifierAbilityDef>().FirstOrDefault(p => p.name.Equals("Thief_AbilityDef"));
            PassiveModifierAbilityDef solipsism = Helper.CreateDefFromClone(
                source,
                "ccd66e53-6258-4fa6-a185-66ba0f5bc4b7",
                skillName);
            solipsism.CharacterProgressionData = Helper.CreateDefFromClone(
                source.CharacterProgressionData,
                "1aef5152-c6d6-435f-959e-0ac368dcf248",
                skillName);
            solipsism.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "ff72f143-8f3e-4988-a5fd-566faa5cb281",
                skillName);


            solipsism.StatModifications = new ItemStatModification[0];
            solipsism.ItemTagStatModifications = new EquipmentItemTagStatModification[0];

            solipsism.ViewElementDef.DisplayName1 = new LocalizedTextBind("SOLIPSISM", true);
            solipsism.ViewElementDef.Description = new LocalizedTextBind("<b>You no longer suffer WP loss when allies die, but you lose 1 WP each time you take damage</b>\n<i>Subject exibits delusions of grandeur and self-obsession," +
                " causing insubordination and disregard for human life</i>", true);
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Privileged_1-2.png");
            solipsism.ViewElementDef.LargeIcon = icon;
            solipsism.ViewElementDef.SmallIcon = icon;
        }
    }

}


