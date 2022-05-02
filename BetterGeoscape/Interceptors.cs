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
                GeoVehicleDef manticoreMaskedNew = Helper.CreateDefFromClone(manticore, "19B82FD8-67EE-4277-B982-F352A53ADE72", "PP_ManticoreMasked_Def_8_Slots");
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
                //Change Fuel Tanmk module
                GeoVehicleModuleDef fueltankmodule = Repo.GetAllDefs<GeoVehicleModuleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_FuelTanks_GeoVehicleModuleDef"));
                //Increase cost to 50% of Vanilla Manti
                cruisecontrolmodule.ManufactureMaterials = 600;
                cruisecontrolmodule.ManufactureTech = 75;
                cruisecontrolmodule.ManufacturePointsCost = 505;


                //Make Hibernation module available for manufacture from start of game - doesn't work because HM is not an ItemDef
                //GeoPhoenixFactionDef phoenixFactionDef = Repo.GetAllDefs<GeoPhoenixFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                //EntitlementDef festeringSkiesEntitlementDef = Repo.GetAllDefs<EntitlementDef>().FirstOrDefault(ged => ged.name.Equals("FesteringSkiesEntitlementDef"));
                // phoenixFactionDef.AdditionalDLCItems.Add(new GeoFactionDef.DLCStartItems { DLC = festeringSkiesEntitlementDef, StartingManufacturableItems = hibernationmodule };               
                //Change cost of Manti to 50% of Vanilla
                VehicleItemDef mantiVehicle = Repo.GetAllDefs<VehicleItemDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_VehicleItemDef"));
                mantiVehicle.ManufactureMaterials = 600;
                mantiVehicle.ManufactureTech = 75;
                mantiVehicle.ManufacturePointsCost = 505;
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


        private static void emptyAircraft(GeoVehicle aircraft)
        {
            if (aircraft.CurrentSite != null && aircraft.CurrentSite.Type == GeoSiteType.PhoenixBase)
            {
                List<GeoCharacter> list = new List<GeoCharacter>(from u in aircraft.Units orderby u.OccupingSpace descending select u);
                foreach (GeoCharacter character in list)
                {
                    if (aircraft.FreeCharacterSpace >= 0)
                    {
                        break;
                    }
                    aircraft.RemoveCharacter(character);
                    aircraft.CurrentSite.AddCharacter(character);
                }
            }
        }


        [HarmonyPatch(typeof(GeoVehicle), "UpdateVehicleBonusCache")]
        internal static class BG_HybernationModuleIncreaseSpaceForUnits_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            
                      
            private static void Postfix(GeoVehicle __instance)
            {
                try
                {
                  
                    GeoVehicleEquipment hybenationPods = __instance.Modules?.FirstOrDefault(gve => gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Recuperation);
                    GeoVehicleEquipment fuelTank = __instance.Modules?.FirstOrDefault(gve => gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Range);
                    GeoVehicleEquipment cruiseControl = __instance.Modules?.FirstOrDefault(gve => gve.ModuleDef.BonusType == GeoVehicleModuleDef.GeoVehicleModuleBonusType.Speed);
                    string geoVehicle = __instance.VehicleDef.ViewElement.Name;

                    //if hybernation pods are present, take the stats of the new defs with increased capacity
                    if (geoVehicle == "Geoscape Manticore")
                    {

                        if (hybenationPods != null || cruiseControl != null || fuelTank != null)

                        {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_Def_6_Slots"));
                        }

                        else
                        {

                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_Manticore_Def"));
                           emptyAircraft(__instance);

                        }
                    }
                if (geoVehicle == "Geoscape Helios")

                  {

                  if (hybenationPods != null || cruiseControl != null || fuelTank != null)

                  {
                   
                        __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_Def_5_Slots"));
                  }

                  else
                  {
                            __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("SYN_Helios_Def"));
                            emptyAircraft(__instance);

                        }
                }

                if (geoVehicle == "Geoscape Thunderbird")

                {

                if (hybenationPods != null || cruiseControl != null || fuelTank != null)

                   {
                    __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_Def_7_Slots"));
                   }

                else
                   
                  {

                   __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("NJ_Thunderbird_Def"));
                   emptyAircraft(__instance);
                   }
                }

                if (geoVehicle == "Geoscape Blimp")

                {

                  if (hybenationPods != null || cruiseControl != null || fuelTank != null)

                  {
                   __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("ANU_Blimp_Def_12_Slots"));
                  
                  }

                   else
                   
                   {

                   __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("ANU_Blimp_Def"));
                   emptyAircraft(__instance);

                      }
                    }

                if (geoVehicle == "Geoscape Masked Manticore")

                {

                 if (hybenationPods != null || cruiseControl != null || fuelTank != null)

                    {
                     __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_ManticoreMasked_Def_8_Slots"));
                     }

                 else

                  {

                    __instance.BaseDef = Repo.GetAllDefs<GeoVehicleDef>().FirstOrDefault(ged => ged.name.Equals("PP_ManticoreMasked_Def"));
                        emptyAircraft(__instance);

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
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }

            public static void Postfix(GeoLevelController __instance)
            {
                try
                {
                    foreach (GeoVehicle geoVehicle in __instance.PhoenixFaction.Vehicles)
                    {
                        if (geoVehicle.Name.Equals("Aurora"))
                        {
                            clonedAircraft.TeleportToSite(geoVehicle.CurrentSite);
                            clonedAircraft.ReloadAllEquipments();
                            return;
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


