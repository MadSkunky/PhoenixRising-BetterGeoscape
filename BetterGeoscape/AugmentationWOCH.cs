using Base.Defs;
using Harmony;
using PhoenixPoint.Geoscape.Levels;
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
            private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
            public static void Postfix(UIModuleBionics __instance)
            {
                try
                {
                    
                    //set Stamina to zero after installing a bionic
                    __instance.CurrentCharacter.Fatigue.Stamina.SetToMin();
                    //check if player made promise to Anu not to apply more bionics
                    if (__instance.Context.Level.EventSystem.GetVariable("BG_Anu_Pissed_Made_Promise")==1) 
                    {
                        __instance.Context.Level.EventSystem.SetVariable("BG_Anu_Pissed_Broke_Promise", 1);
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
