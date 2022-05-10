using Base.Defs;
using Harmony;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class MutationStamina
    {
        [HarmonyPatch(typeof(UIModuleMutationSection), "ApplyMutation")]
        public static class UIModuleMutationSection_ApplyMutation_SetStaminaTo0_patch
        {
            private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
            public static void Postfix(IAugmentationUIModule ____parentModule)
            {
                try
                {
                    ____parentModule.CurrentCharacter.Fatigue.Stamina.SetToMin();

                    GeoFactionDef newJerico = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                    
                    //check if player made promise to New Jericho not to apply more mutations
                    if (____parentModule.Context.Level.EventSystem.GetVariable("BG_NJ_Pissed_Made_Promise") == 1 && ____parentModule.CurrentCharacter.OriginalFactionDef == newJerico)
                    {
                        ____parentModule.Context.Level.EventSystem.SetVariable("BG_NJ_Pissed_Broke_Promise", 1);
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
