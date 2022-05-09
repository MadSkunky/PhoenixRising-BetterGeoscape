using Harmony;
using PhoenixPoint.Geoscape.View.ViewModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class AugmentationWOCH
    {
        //When getting an augment, the character's Stamina is set to 0 and each augment reduces corruption by a 1/3
        [HarmonyPatch(typeof(UIModuleBionics), "OnAugmentApplied")]
        public static class UIModuleBionics_OnAugmentApplied_SetStaminaTo0_patch
        {
            public static void Postfix(UIModuleBionics __instance)
            {
                try
                {
                    //set Stamina to zero after installing a bionic
                    __instance.CurrentCharacter.Fatigue.Stamina.SetToMin();
                }

                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }

    }
}
