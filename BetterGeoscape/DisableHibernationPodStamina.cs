using Base.Defs;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class DisableHibernationPodStamina

    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static void Apply_Changes()
        {

            try
            {
                GeoVehicleModuleDef hibernationmodule = Repo.GetAllDefs<GeoVehicleModuleDef>().FirstOrDefault(ged => ged.name.Equals("SY_HibernationPods_GeoVehicleModuleDef"));
                hibernationmodule.GeoVehicleModuleBonusValue = 0;

            }


            catch (Exception e)
            {
                Logger.Error(e);
            }

        }

    }
}