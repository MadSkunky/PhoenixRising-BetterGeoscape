
using Base.Utils;
using Harmony;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Levels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoenixRising.BetterGeoscape
{
    internal class ScavengingSettings
    {

        // Harmony patch before Geoscape world is created
        [HarmonyPatch(typeof(GeoInitialWorldSetup), "SimulateFactions")]
        internal static class BC_GeoInitialWorldSetup_SimulateFactions_Patch
        {
            public static void Prefix(GeoInitialWorldSetup __instance, GeoLevelController level, IList<GeoSiteSceneDef.SiteInfo> worldSites, TimeSlice timeSlice)
            {
                try

                {
                    // __instance holds all variables of GeoInitialWorldSetup, here the initial amount of all scavenging sites
                    __instance.InitialScavengingSiteCount = 8; // default 16

                    // ScavengingSitesDistribution is an array with the weights for scav, rescue soldier and vehicle
                    foreach (GeoInitialWorldSetup.ScavengingSiteConfiguration scavSiteConf in __instance.ScavengingSitesDistribution)
                    {
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_ResourceCrates_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 2; // default 4
                        }
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_RescueSoldier_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 1;
                        }
                        if (scavSiteConf.MissionTags.Any(mt => mt.name.Equals("Contains_RescueVehicle_MissionTagDef")))
                        {
                            scavSiteConf.Weight = 1;
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

    



