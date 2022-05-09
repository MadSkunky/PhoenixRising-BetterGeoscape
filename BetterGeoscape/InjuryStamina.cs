using Harmony;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Tactical.Entities.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class InjuryStamina
    {

        // Setting Stamina to zero if character suffered a disabled limb during tactical

        //A list of operatives that get disabled limbs. This list is cleared when the game is exited, so saving a game in tactical, exiting the game and reloading will probably make the game "forget" the character was ever injured.
        public static List<int> charactersWithBrokenLimbs = new List<int>();

        // This first patch is to "register" the injury in the above list
        [HarmonyPatch(typeof(BodyPartAspect), "OnSetToDisabled")]
        internal static class BodyPartAspect_OnSetToDisabled_patch
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051")]
            private static void Postfix(BodyPartAspect __instance)
            {
                // The way to get access to base.OwnerItem
                // 'base' it the class this object is derived from and with Harmony we can't directly access these base classes
                // looking in dnSpy we can see, that 'base' is of type 'TacticalItemAspectBase' and we want to access it's property 'OwnerItem' that is of type 'TacticalItem'
                // 'AccessTools.Property' are tools from Harmony to make such an access easier, the usual way through Reflections is a bit more complicated.
                TacticalItem base_OwnerItem = (TacticalItem)AccessTools.Property(typeof(TacticalItemAspectBase), "OwnerItem").GetValue(__instance, null);

                if (!charactersWithBrokenLimbs.Contains(base_OwnerItem.TacticalActorBase.GeoUnitId))
                {
                    charactersWithBrokenLimbs.Add(base_OwnerItem.TacticalActorBase.GeoUnitId);
                }
            }
        }

        // This second patch reads from the list and drains Stamina from everyone who is in it. 
        [HarmonyPatch(typeof(GeoCharacter), "Init")]
        internal static class GeoCharacter_Init_StaminaToZeroIfBodyPartDisabled_patch
        {
            public static void Postfix(GeoCharacter __instance)
            {
                try
                {

                    if (charactersWithBrokenLimbs.Contains(__instance.Id))
                    {
                        CommonMethods.SetStaminaToZero(__instance);
                        charactersWithBrokenLimbs.Remove(__instance.Id);
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
