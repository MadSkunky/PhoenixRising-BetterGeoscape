﻿
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
            Create_HallucinatingStatus();
            Create_NailsPassive();
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
            shutEye.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_INNER_SIGHT_NAME";
            shutEye.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_INNER_SIGHT_DESCRIPTION";
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
            hallucinating.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_ANXIETY_NAME";
            hallucinating.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_ANXIETY_DESCRIPTION";
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

            fleshEater.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_BLOODTHIRSTY_NAME";
            fleshEater.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_BLOODTHIRSTY_DESCRIPTION";
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
            angerIssues.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_FASTER_SYNAPSES_NAME";
            angerIssues.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_FASTER_SYNAPSES_DESCRIPTION";
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
            photophobia.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_TERROR_NAME";
            photophobia.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_TERROR_DESCRIPTION";
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
            nailsPassive.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_WOLVERINE_NAME";
            nailsPassive.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_WOLVERINE_DESCRIPTION";
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

            nails.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_WOLVERINE_NAME";
            nails.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_WOLVERINE_DESCRIPTION";
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

            oneOfUs.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_ONE_OF_THEM_NAME";
            oneOfUs.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_ONE_OF_THEM_DESCRIPTION";
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
                "BA1CF3A2-2175-4A2D-BC1A-59439722CD81",
                skillName);
            ofuPassive.ViewElementDef = Helper.CreateDefFromClone(
                source.ViewElementDef,
                "76E1161E-FAB9-4E7C-A039-B2945FC4D4FD",
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
            ofuPassive.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_ONE_OF_THEM_NAME";
            ofuPassive.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_ONE_OF_THEM_DESCRIPTION";
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
            immortality.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_DEREALIZATION_NAME";
            immortality.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_DEREALIZATION_DESCRIPTION";
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

            feral.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_FERAL_NAME";
            feral.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_FERAL_DESCRIPTION";
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

            solipsism.ViewElementDef.DisplayName1.LocalizationKey = "DELIRIUM_PERK_HYPERALGESIA_NAME";
            solipsism.ViewElementDef.Description.LocalizationKey = "DELIRIUM_PERK_HYPERALGESIA_DESCRIPTION";
            Sprite icon = Helper.CreateSpriteFromImageFile("UI_AbilitiesIcon_PersonalTrack_Privileged_1-2.png");
            solipsism.ViewElementDef.LargeIcon = icon;
            solipsism.ViewElementDef.SmallIcon = icon;
        }

        public static void Create_HallucinatingStatus()
        {
            string skillName = "Hallucinating_StatusDef";
            SilencedStatusDef source = Repo.GetAllDefs<SilencedStatusDef>().FirstOrDefault(p => p.name.Equals("ActorSilenced_StatusDef"));
            SilencedStatusDef hallucinatingStatus = Helper.CreateDefFromClone(
                source,
                "2d5ed7eb-f4f3-42bf-8589-1d50ec99fa8b",
                skillName);

            hallucinatingStatus.DurationTurns = 2;
        }

    }
}


