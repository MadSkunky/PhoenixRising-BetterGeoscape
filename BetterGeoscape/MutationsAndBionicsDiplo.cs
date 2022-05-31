using Base.Core;
using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class MutationsAndBionicsDiplo
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        public static class PhoenixStatisticsManager_UpdateGeoscapeStats_AnuPissedAtBionics_Patch
        {
            public static void Postfix(GeoAlienFaction __instance)
            {
                try
                {
                    int bionics = 0;
                    GeoLevelController geoLevelController = __instance.GeoLevel;
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance, geoLevelController.ViewerFaction);
                    GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                    //check number of bionics player has
                    GameTagDef bionicalTag = GameUtl.GameComponent<SharedData>().SharedGameTags.BionicalTag;
                    foreach (GeoCharacter geoCharacter in __instance.GeoLevel.PhoenixFaction.Soldiers)
                    {
                        foreach (GeoItem bionic in geoCharacter.ArmourItems)
                        {
                            if (bionic.ItemDef.Tags.Contains(bionicalTag))

                                bionics += 1;
                        }
                    }
                    if (bionics > 2 && geoLevelController.EventSystem.GetVariable("BG_Anu_Pissed_Over_Bionics") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("Anu_Pissed1", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_Anu_Pissed_Over_Bionics", 1);
                    }

                    if (geoLevelController.EventSystem.GetVariable("BG_Anu_Pissed_Broke_Promise") == 1
                       && geoLevelController.EventSystem.GetVariable("BG_Anu_Really_Pissed_Over_Bionics") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("Anu_Pissed2", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_Anu_Really_Pissed_Over_Bionics", 1);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

        [HarmonyPatch(typeof(GeoAlienFaction), "UpdateFactionDaily")]
        public static class PhoenixStatisticsManager_UpdateGeoscapeStats_NJPissedAtMutations_Patch
        {
            public static void Postfix(GeoAlienFaction __instance)
            {
                try
                {
                    int mutations = 0;
                    GeoLevelController geoLevelController = __instance.GeoLevel;
                    GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance, geoLevelController.ViewerFaction);
                    GeoFactionDef newJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                    //check number of mutations player has
                    GameTagDef mutationTag = GameUtl.GameComponent<SharedData>().SharedGameTags.AnuMutationTag;
                    foreach (GeoCharacter geoCharacter in __instance.GeoLevel.PhoenixFaction.Soldiers)
                    {
                        foreach (GeoItem mutation in geoCharacter.ArmourItems)
                        {
                            if (mutation.ItemDef.Tags.Contains(mutationTag))
                                mutations += 1;
                        }
                    }
                    if (mutations > 2 && geoLevelController.EventSystem.GetVariable("BG_NJ_Pissed_Over_Mutations") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("NJ_Pissed1", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_NJ_Pissed_Over_Mutations", 1);
                    }
                    if (geoLevelController.EventSystem.GetVariable("BG_NJ_Pissed_Broke_Promise") == 1
                       && geoLevelController.EventSystem.GetVariable("BG_NJ_Really_Pissed_Over_Mutations") == 0)
                    {
                        geoLevelController.EventSystem.TriggerGeoscapeEvent("NJ_Pissed2", geoscapeEventContext);
                        geoLevelController.EventSystem.SetVariable("BG_NJ_Really_Pissed_Over_Mutations", 1);
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
