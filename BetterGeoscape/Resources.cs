using Base.Defs;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class Resources
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        private static bool ApplyChangeReduceResources = true;
        public static void Apply_Changes()
        {
            try
            {
                if (ApplyChangeReduceResources)
                {

                    foreach (GeoscapeEventDef geoEvent in Repo.GetAllDefs<GeoscapeEventDef>())
                    {
                        foreach (GeoEventChoice choice in geoEvent.GeoscapeEventData.Choices)
                        {
                            if (choice.Outcome.Resources != null && !choice.Outcome.Resources.IsEmpty)
                            {
                                choice.Outcome.Resources *= (BetterGeoscapeMain.Config.ResourceMultiplier);
                            }
                        }
                    }
                    ApplyChangeReduceResources = false;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
 
