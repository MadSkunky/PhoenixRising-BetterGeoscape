using Base.Defs;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Levels.Mist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{

    /* {"KEY_SNIPER_QUICK_AIM_NAME", "KEY_RAPID_CLEARANCE_NAME", "AIMED BURST", "KEY_RALLY_NAME", "Dash", "AdreanlineRush", "GunKata_AbilityDef",
         "Exertion_AbilityDef", "BC_ARTargeting_AbilityDef", "Rage Burst", "JetpackControl_AbilityDef", "KEY_HEAVY_BOOM_BLAST_NAME", "Deploy Drone Pack", "LayWaste_AbilityDef", "KEY_ARMOUR_BREAK_NAME",
         "BC_Gunslinger_AbilityDef", "ElectricReinforcement", "AmplifyPain_AbilityDef", "KEY_SNIPER_MARK_FOR_DEATH_NAME", "KEY_INDUCE_PANIC_NAME", "KEY_PSYCHIC_BLAST_NAME", "KEY_TECHNICIAN_REMOTE_DEPLOYMENT_NAME", "KEY_TECHNICIAN_MANUAL_CONTROL_NAME", "KEY_TECHNICIAN_FIELD_MEDIC_NAME",
         "KEY_DECOY_NAME", "KEY_VANISH_NAME"}*/

    //This looks like the method for AI to evaluate how much damage it will do with an attack. May be useful for implementing attack for Umbra
    /*
        public void CalculateDamageResultForAiTarget(TacticalActorBase targetActorBase, IDamageReceiver receiver, ref DamageResult damageResult)
        {
            foreach (DamageKeywordPair item in DamageKeywords.Where((DamageKeywordPair k) => k.DamageKeywordDef.UseForAiVulnerabilityCheck))
            {
                float keywordValue = item.Value * receiver.GetDamageMultiplierFor(item.DamageKeywordDef.DamageTypeDef, this);
                float num = 0f;
                TacticalActor tacticalActor;
                if ((object)(tacticalActor = (receiver as TacticalActor)) != null)
                {
                    foreach (ItemSlot healthSlot in tacticalActor.BodyState.GetHealthSlots())
                    {
                        num = Mathf.Max(num, healthSlot.GetArmor());
                    }
                }
                else
                {
                    num = receiver.GetArmor();
                }

                num -= GenerateArmourPiercingAmount();
                item.DamageKeywordDef.ApplyAiEvaluationEffect(targetActorBase, keywordValue, num, ref damageResult);
            }
        }  */


    internal class DifferentThings
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public void CheckAugmentationsForDelirium(GeoCharacter character, ItemDef augment1, ItemDef augment2, ItemDef augment3)
       {
           

            ItemDef aN_Berserker_Heavy_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Heavy_Legs_ItemDef"));
            ItemDef aN_Berserker_Heavy_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Heavy_Torso_BodyPartDef"));
            ItemDef aN_Berserker_Heavy_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Heavy_Helmet_BodyPartDef"));
            ItemDef aN_Berserker_Shooter_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Shooter_Legs_ItemDef"));
            ItemDef aN_Berserker_Shooter_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Shooter_Torso_BodyPartDef"));
            ItemDef aN_Berserker_Shooter_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Shooter_Helmet_BodyPartDef"));
            ItemDef aN_Berserker_Watcher_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Watcher_Legs_ItemDef"));
            ItemDef aN_Berserker_Watcher_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Watcher_Torso_BodyPartDef"));
            ItemDef aN_Berserker_Watcher_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Berserker_Watcher_Helmet_BodyPartDef"));

            ItemDef firstUpgrade_aN_Berserker_Heavy_Legs = Helper.CreateDefFromClone(aN_Berserker_Heavy_Legs, "72899D84-E5E6-4CD7-815C-A78858A0B485", "FirstUpgrade_aN_Berserker_Heavy_Legs_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Heavy_Torso = Helper.CreateDefFromClone(aN_Berserker_Heavy_Torso, "28F2DB04-C954-482B-914C-ED6290658A82", "FirstUpgrade_aN_Berserker_Heavy_Torso_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Heavy_Helmet = Helper.CreateDefFromClone(aN_Berserker_Heavy_Helmet, "D4C47C9D-3740-40C5-B9A6-E8EDBFBAD794", "FirstUpgrade_aN_Berserker_Heavy_Helmet_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Shooter_Legs = Helper.CreateDefFromClone(aN_Berserker_Shooter_Legs, "596827F8-33EB-4C4E-9FC4-AC8BF0814461", "FirstUpgrade_aN_Berserker_Shooter_Legs_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Shooter_Torso = Helper.CreateDefFromClone(aN_Berserker_Shooter_Torso, "F1126E5A-A6A9-47F9-9D4A-090DE4552FE2", "FirstUpgrade_aN_Berserker_Shooter_Torso_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Shooter_Helmet = Helper.CreateDefFromClone(aN_Berserker_Shooter_Helmet, "BFBAF2AD-79B3-4099-9656-161359C5547A", "FirstUpgrade_aN_Berserker_Shooter_Helmet_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Watcher_Legs = Helper.CreateDefFromClone(aN_Berserker_Watcher_Legs, "EE455D11-0EF7-4E69-95CD-0BD4E6F7C106", "FirstUpgrade_aN_Berserker_Watcher_Legs_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Watcher_Torso = Helper.CreateDefFromClone(aN_Berserker_Watcher_Torso, "6B7414B5-9576-4340-A335-ABBFD5CDE4EB", "FirstUpgrade_aN_Berserker_Watcher_Torso_ItemDef");
            ItemDef firstUpgrade_aN_Berserker_Watcher_Helmet = Helper.CreateDefFromClone(aN_Berserker_Watcher_Helmet, "2B359C50-2742-4A91-B931-6F62FBA1BF6C", "FirstUpgrade_aN_Berserker_Watcher_Helmet_ItemDef");

            ItemDef secondUpgrade_aN_Berserker_Heavy_Legs = Helper.CreateDefFromClone(aN_Berserker_Heavy_Legs, "{B182DC13-44A9-4745-B4F7-E8E31DB4EFBE}", "SecondUpgrade_aN_Berserker_Heavy_Legs_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Heavy_Torso = Helper.CreateDefFromClone(aN_Berserker_Heavy_Torso, "C3D24E19-4771-41C7-ABF6-DE6A1247F944", "SecondUpgrade_aN_Berserker_Heavy_Torso_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Heavy_Helmet = Helper.CreateDefFromClone(aN_Berserker_Heavy_Helmet, "D636D638-3AFE-4645-95D4-586623AAC342", "SecondUpgrade_aN_Berserker_Heavy_Helmet_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Shooter_Legs = Helper.CreateDefFromClone(aN_Berserker_Shooter_Legs, "AD1FFEF7-C041-4B80-A942-C67543EA72D1", "SecondUpgrade_aN_Berserker_Shooter_Legs_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Shooter_Torso = Helper.CreateDefFromClone(aN_Berserker_Shooter_Torso, "C07C35C7-2513-4E6F-B0C9-4F096E5AF33B", "SecondUpgrade_aN_Berserker_Shooter_Torso_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Shooter_Helmet = Helper.CreateDefFromClone(aN_Berserker_Shooter_Helmet, "86921905-938C-4AE5-AA10-B8034C377C73", "SecondUpgrade_aN_Berserker_Shooter_Helmet_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Watcher_Legs = Helper.CreateDefFromClone(aN_Berserker_Watcher_Legs, "92774BEF-4C45-4307-8916-C4603CF8C37B", "SecondUpgrade_aN_Berserker_Watcher_Legs_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Watcher_Torso = Helper.CreateDefFromClone(aN_Berserker_Watcher_Torso, "FEC815D6-6AEA-437F-B51D-F7E24801C653", "SecondUpgrade_aN_Berserker_Watcher_Torso_ItemDef");
            ItemDef secondUpgrade_aN_Berserker_Watcher_Helmet = Helper.CreateDefFromClone(aN_Berserker_Watcher_Helmet, "C71DD85E-8859-4FCC-A707-8C686D946634", "SecondUpgrade_aN_Berserker_Watcher_Helmet_ItemDef");

            ItemDef aN_Priest_Head01 = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Priest_Head01_BodyPartDef"));
            ItemDef aN_Priest_Head02 = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Priest_Head02_BodyPartDef"));
            ItemDef aN_Priest_Head03 = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("AN_Priest_Head03_BodyPartDef"));

            ItemDef firstUpgrade_aN_Priest_Head01 = Helper.CreateDefFromClone(aN_Priest_Head01, "B4E1E295-302D-43AA-9CAF-D45642A5D968", "FirstUpgrade_aN_Priest_Head01_Legs_ItemDef");
            ItemDef firstUpgrade_aN_Priest_Head02 = Helper.CreateDefFromClone(aN_Priest_Head02, "212CD5C4-65BE-4D12-A1FD-AC49D5604095", "FirstUpgrade_aN_Priest_Head02_Torso_ItemDef");
            ItemDef firstUpgrade_aN_Priest_Head03 = Helper.CreateDefFromClone(aN_Priest_Head03, "E56BD0C0-D5F2-491F-A518-AF9BFFBBCADB", "FirstUpgrade_aN_Priest_Head03_Helmet_ItemDef");

            ItemDef secondUpgrade_aN_Priest_Head01 = Helper.CreateDefFromClone(aN_Priest_Head01, "0EF83E01-D79A-45C8-810E-6E3BE7CD2807", "SecondUpgrade_aN_Priest_Head01_Legs_ItemDef");
            ItemDef secondUpgrade_aN_Priest_Head02 = Helper.CreateDefFromClone(aN_Priest_Head02, "ABAFF1AA-9EC9-4116-B398-DE9BB9EF4769", "SecondUpgrade_aN_Priest_Head02_Torso_ItemDef");
            ItemDef secondUpgrade_aN_Priest_Head03 = Helper.CreateDefFromClone(aN_Priest_Head03, "9B0FEE93-44FA-4B61-9DC3-538C2C51198D", "SecondUpgrade_aN_Priest_Head03_Helmet_ItemDef");


            ItemDef NJ_Jugg_BIO_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Jugg_BIO_Legs_ItemDef"));
            ItemDef NJ_Jugg_BIO_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Jugg_BIO_Torso_BodyPartDef"));
            ItemDef NJ_Jugg_BIO_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Jugg_BIO_Helmet_BodyPartDef"));

            ItemDef NJ_Exo_BIO_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Exo_BIO_Legs_ItemDef"));
            ItemDef NJ_Exo_BIO_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Exo_BIO_Torso_BodyPartDef"));
            ItemDef NJ_Exo_BIO_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Exo_BIO_Helmet_BodyPartDef"));

            ItemDef SY_Shinobi_BIO_Legs = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("SY_Shinobi_BIO_Legs_ItemDef"));
            ItemDef SY_Shinobi_BIO_Torso = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("SY_Shinobi_BIO_Torso_BodyPartDef"));
            ItemDef SY_Shinobi_BIO_Helmet = Repo.GetAllDefs<ItemDef>().FirstOrDefault(ged => ged.name.Equals("SY_Shinobi_BIO_Helmet_BodyPartDef"));

            //All mutation defs:
            //AN_Berserker_Shooter_Helmet_BodyPartDef
            //AN_Berserker_Shooter_Torso_BodyPartDef
            //AN_Berserker_Shooter_Legs_ItemDef
            //AN_Berserker_Watcher_Helmet_BodyPartDef
            //AN_Berserker_Watcher_Torso_BodyPartDef
            //AN_Berserker_Watcher_Legs_ItemDef
            //AN_Berserker_Heavy_Helmet_BodyPartDef
            //AN_Berserker_Heavy_Torso_BodyPartDef
            //AN_Berserker_Heavy_Legs_ItemDef
            //AN_Priest_Head01_BodyPartDef
            //AN_Priest_Head02_BodyPartDef
            //AN_Priest_Head03_BodyPartDef

            //All bionics defs:
            //NJ_Jugg_BIO_Helmet_BodyPartDef
            //NJ_Jugg_BIO_Torso_BodyPartDef
            //NJ_Jugg_BIO_Legs_ItemDef
            //NJ_Exo_BIO_Helmet_BodyPartDef
            //NJ_Exo_BIO_Torso_BodyPartDef
            //NJ_Exo_BIO_Legs_ItemDef
            //SY_Shinobi_BIO_Helmet_BodyPartDef
            //SY_Shinobi_BIO_Torso_BodyPartDef
            //SY_Shinobi_BIO_Legs_ItemDef

            // check if mutations are present
            if (character.GetBodyParts().Contains(augment1))
            {

                if (character.GetBodyParts().Contains(augment2) && augment1 !=augment2) 
                { 
                
                    if(character.GetBodyParts().Contains(augment3) && augment3 != augment2) 
                    { 
                    
                    }

                }
            }
            
        }

    }
}
