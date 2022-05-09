using Harmony;
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
            public static void Postfix(IAugmentationUIModule ____parentModule)
            {
                try
                {
                    ____parentModule.CurrentCharacter.Fatigue.Stamina.SetToMin();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
    }
}
