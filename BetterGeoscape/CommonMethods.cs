using PhoenixPoint.Geoscape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class CommonMethods
    {

        public static void SetStaminaToZero(GeoCharacter __instance)
        {
            try
            {
                __instance.Fatigue.Stamina.SetToMin();
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

    }
}
