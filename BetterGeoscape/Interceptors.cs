using Base.Core;
using Base.Defs;
using Base.Platforms;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhoenixPoint.Geoscape.Entities.GeoVehicleStatModifier;

namespace PhoenixRising.BetterGeoscape
{
    internal class Interceptors
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static void Apply_Changes()
        {

            try
            {
                //ID all the factions for later
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));

                //ID all craft for later
                GeoVehicleDef manticore = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_Def"));
                GeoVehicleDef helios = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_Def"));
                GeoVehicleDef thunderbird = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_Def"));
                GeoVehicleDef blimp = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("ANU_Blimp_Def"));
                GeoVehicleDef manticoreMasked = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_MaskedManticore_Def"));
      
                //Reduce all craft seating (except blimp) by 4 and create clones with previous seating

                GeoVehicleDef manticoreNew = Helper.CreateDefFromClone(manticore, "83A7FD03-DB85-4CEE-BAED-251F5415B82B", "PP_Manticore_Def_6_Slots");
                manticore.BaseStats.SpaceForUnits = 2;
                GeoVehicleDef heliosNew = Helper.CreateDefFromClone(helios, "4F9026CB-EF42-44B8-B9C3-21181EC4E2AB", "SYN_Helios_Def_5_Slots");
                helios.BaseStats.SpaceForUnits = 1;
                GeoVehicleDef thunderbirdNew = Helper.CreateDefFromClone(thunderbird, "FDE7F0C2-8BA7-4046-92EB-F3462F204B2B", "NJ_Thunderbird_Def_7_Slots");
                thunderbird.BaseStats.SpaceForUnits = 3;
                GeoVehicleDef blimpNew = Helper.CreateDefFromClone(blimp, "B857B76D-BDDB-4CA9-A1CA-895A540B17C8", "ANU_Blimp_Def_12_Slots");
                blimpNew.BaseStats.SpaceForUnits = 12;
                GeoVehicleDef manticoreMaskedNew = Helper.CreateDefFromClone(manticoreMasked, "19B82FD8-67EE-4277-B982-F352A53ADE72", "PP_ManticoreMasked_Def_8_Slots");
                manticoreMasked.BaseStats.SpaceForUnits = 4;

                //Change Hibernation module
                GeoVehicleModuleDef hibernationmodule = Repo.GetAllDefs<GeoVehicleModuleDef>().FirstOrDefault(ged => ged.name.Equals("SY_HibernationPods_GeoVehicleModuleDef"));
                //Increase cost to 50% of Vanilla Manti
                hibernationmodule.ManufactureMaterials = 600;
                hibernationmodule.ManufactureTech = 75;
                hibernationmodule.ManufacturePointsCost = 505;
                //Change Cruise Control module
                GeoVehicleModuleDef cruisecontrolmodule = Repo.GetAllDefs<GeoVehicleModuleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_CruiseControl_GeoVehicleModuleDef"));
                //Increase cost to 50% of Vanilla Manti
                cruisecontrolmodule.ManufactureMaterials = 600;
                cruisecontrolmodule.ManufactureTech = 75;
                cruisecontrolmodule.ManufacturePointsCost = 505;
                //Change Fuel Tank module
                GeoVehicleModuleDef fueltankmodule = Repo.GetAllDefs<GeoVehicleModuleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_FuelTanks_GeoVehicleModuleDef"));
                //Increase cost to 50% of Vanilla Manti
                fueltankmodule.ManufactureMaterials = 600;
                fueltankmodule.ManufactureTech = 75;
                fueltankmodule.ManufacturePointsCost = 505;


                //Make Hibernation module available for manufacture from start of game - doesn't work because HM is not an ItemDef
                //GeoPhoenixFactionDef phoenixFactionDef = Repo.GetAllDefs<GeoPhoenixFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                //EntitlementDef festeringSkiesEntitlementDef = Repo.GetAllDefs<EntitlementDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesEntitlementDef"));
                // phoenixFactionDef.AdditionalDLCItems.Add(new GeoFactionDef.DLCStartItems { DLC = festeringSkiesEntitlementDef, StartingManufacturableItems = hibernationmodule };               
                //Change cost of Manti to 50% of Vanilla
                VehicleItemDef mantiVehicle = Repo.GetAllDefs<VehicleItemDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_VehicleItemDef"));
                mantiVehicle.ManufactureMaterials = 600;
                mantiVehicle.ManufactureTech = 75;
                mantiVehicle.ManufacturePointsCost = 505;
                //Change cost of Helios to Vanilla minus cost of passenger module
                VehicleItemDef heliosVehicle = Repo.GetAllDefs<VehicleItemDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_VehicleItemDef"));
                heliosVehicle.ManufactureMaterials = 555;
                heliosVehicle.ManufactureTech = 173;
                heliosVehicle.ManufacturePointsCost = 510;
                //Change cost of Thunderbird to Vanilla minus cost of passenger module
                VehicleItemDef thunderbirdVehicle = Repo.GetAllDefs<VehicleItemDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_VehicleItemDef"));
                thunderbirdVehicle.ManufactureMaterials = 900;
                thunderbirdVehicle.ManufactureTech = 113;
                thunderbirdVehicle.ManufacturePointsCost = 660;

                //Make HM research for PX, available after completing Phoenix Archives
                ResearchDef hibernationModuleResearch = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Aircraft_HybernationPods_ResearchDef"));
                ResearchDef sourcePX_SDI_ResearchDef = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("PX_SDI_ResearchDef"));
                hibernationModuleResearch.Faction = PhoenixPoint;
                hibernationModuleResearch.RevealRequirements = sourcePX_SDI_ResearchDef.RevealRequirements;
                hibernationModuleResearch.ResearchCost = 100;
                hibernationmodule.GeoVehicleModuleBonusValue = 0.35f;      

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            
        }

        [HarmonyPatch(typeof(GeoPhoenixFaction), "CreateInitialSquad")]
        internal static class BG_GeoPhoenixFaction_CreateInitialSquad_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Prefix(GeoPhoenixFaction __instance)
            {
                try
                {
                    GeoVehicle geoVehicle = __instance.Vehicles.First();
                    geoVehicle.AddEquipment(Repo.GetAllDefs<GeoVehicleEquipmentDef>().FirstOrDefault(gve => gve.name.Equals("SY_HibernationPods_GeoVehicleModuleDef")));
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoVehicle), "ReplaceEquipments")]
        internal static class BG_GeoVehicle_ReplaceEquipments_RemoveExcessPassengers_patch
        {
            private static void Postfix(GeoVehicle __instance)
            {
                try
                {
                    if (__instance.CurrentSite != null && __instance.CurrentSite.Type == GeoSiteType.PhoenixBase)
                    {
                        if (!__instance.HasModuleBonusTo(GeoVehicleModuleDef.GeoVehicleModuleBonusType.Recuperation)
                            || !__instance.HasModuleBonusTo(GeoVehicleModuleDef.GeoVehicleModuleBonusType.Speed)
                            || !__instance.HasModuleBonusTo(GeoVehicleModuleDef.GeoVehicleModuleBonusType.Range))
                        {
                            if (__instance.UsedCharacterSpace > __instance.MaxCharacterSpace)
                            {
                                List<GeoCharacter> list = new List<GeoCharacter>(from u in __instance.Units orderby u.OccupingSpace descending select u);
                                foreach (GeoCharacter character in list)
                                {
                                    if (__instance.FreeCharacterSpace >= 0)
                                    {
                                        break;
                                    }
                                    __instance.RemoveCharacter(character);
                                    __instance.CurrentSite.AddCharacter(character);
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

        [HarmonyPatch(typeof(GeoVehicle), "UpdateVehicleBonusCache")]
        internal static class BG_GeoVehicle_UpdateVehicleBonusCache_PassengerModulesIncreaseSpaceForUnits_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
                            
            private static void Postfix(GeoVehicle __instance)
            {
                try
                {
                  
                    GeoVehicleEquipment hybernationPods = __instance.Modules?.FirstOrDefault(gve => gve != null && gve.ModuleDef != null && gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Recuperation);
                    GeoVehicleEquipment fuelTank = __instance.Modules?.FirstOrDefault(gve => gve != null && gve.ModuleDef != null && gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Range);
                    GeoVehicleEquipment cruiseControl = __instance.Modules?.FirstOrDefault(gve => gve != null && gve.ModuleDef != null && gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Speed);
                    string geoVehicle = __instance.VehicleDef.ViewElement.Name;

                    //if hybernation pods are present, take the stats of the new defs with increased capacity
                    if (geoVehicle == "Geoscape Manticore")
                    {
                        if (hybernationPods != null || cruiseControl != null || fuelTank != null)
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_Def_6_Slots"));
                        }
                        else
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_Def"));
                            
                        }
                    }
                    if (geoVehicle == "Geoscape Helios")
                    {
                        if (hybernationPods != null || cruiseControl != null || fuelTank != null)
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_Def_5_Slots"));
                        }
                        else
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_Def"));
                            
                        }
                    }
                    if (geoVehicle == "Geoscape Thunderbird")
                    {
                        if (hybernationPods != null || cruiseControl != null || fuelTank != null)
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_Def_7_Slots"));
                        }
                        else
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_Def"));
                            
                        }
                    }
                    if (geoVehicle == "Geoscape Blimp")
                    {
                        if (hybernationPods != null || cruiseControl != null || fuelTank != null)
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("ANU_Blimp_Def_12_Slots"));
                        }
                        else
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("ANU_Blimp_Def"));
                            
                        }
                    }
                    if (geoVehicle == "Geoscape Masked Manticore")
                    {
                        if (hybernationPods != null || cruiseControl != null || fuelTank != null)
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_ManticoreMasked_Def_8_Slots"));
                        }
                        else
                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_MaskedManticore_Def"));
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoLevelController), "RunInterceptionTutorial")]
        public static class GeoLevelController_DontDestroyAircraft_Gift
        {
            private static GeoVehicle clonedAircraft;

            public static void Prefix(GeoLevelController __instance)
            {
                try
                {

                    GeoVehicle geoVehicle = __instance.View.SelectedActor as GeoVehicle;
                    string componentName = "PP_Manticore";
                    if (geoVehicle.VehicleDef.name.Contains("ANU_Blimp"))
                    {
                        componentName = "ANU_Blimp";
                    }
                    else if (geoVehicle.VehicleDef.name.Contains("NJ_Thunderbird"))
                    {
                        componentName = "NJ_Thunderbird";
                    }
                    else if (geoVehicle.VehicleDef.name.Contains("SYN_Helios"))
                    {
                        componentName = "SYN_Helios";
                    }
                    ComponentSetDef sourceAircraftComponentDef = Repo.GetAllDefs<ComponentSetDef>().FirstOrDefault(csd => csd.name.Equals(componentName));
                    clonedAircraft = __instance.PhoenixFaction.CreateVehicle(geoVehicle.CurrentSite, sourceAircraftComponentDef);
                    clonedAircraft.RenameVehicle(geoVehicle.Name);
                    foreach (GeoVehicleEquipment equipment in geoVehicle.Equipments)
                    {
                        clonedAircraft.AddEquipment(equipment);
                    }

                    GeoSite geoSite = (from p in __instance.Map.SitesByType[GeoSiteType.PhoenixBase]
                                       where p.Owner == __instance.PhoenixFaction && p.State == GeoSiteState.Functioning
                                       select p into d
                                       orderby GeoMap.Distance(d, clonedAircraft.CurrentSite)
                                       select d).FirstOrDefault();
                    clonedAircraft.TeleportToSite(geoSite);
                    clonedAircraft.ReloadAllEquipments();


                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
    }
}


