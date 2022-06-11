using Base.Defs;
using Base.UI;
using PhoenixPoint.Geoscape.Events.Eventus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixRising.BetterGeoscape
{
    internal class Intro
    {
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;
        public static void CreateIntro()            
        {
            try             
            {
                string introEvent_0 = "IntroBetterGeo_0";
                string introEvent_1 = "IntroBetterGeo_1";
                string introEvent_2 = "IntroBetterGeo_2";
                GeoscapeEventDef intro0 = CommonMethods.CreateNewEvent("9D08B1EE-54D2-4039-B01E-B52716CB0D4F",
                    introEvent_0, "BG_INTRO_0_TITLE", "BG_INTRO_0_DESCRIPTION", null);                
                GeoscapeEventDef intro1 = CommonMethods.CreateNewEvent("298DA57E-1230-4B5D-A5E1-B55A043958BC",
                    introEvent_1, "BG_INTRO_1_TITLE", "BG_INTRO_1_DESCRIPTION", null);
               // intro1.GeoscapeEventData.Choices[0].Outcome.TriggerEncounterID = introEvent_0;
                GeoscapeEventDef intro2 = CommonMethods.CreateNewEvent("75F3C93D-E9E9-4BF6-86D9-D77A4D842831",
                    introEvent_2, "BG_INTRO_2_TITLE", "BG_INTRO_2_DESCRIPTION", null);
               // intro2.GeoscapeEventData.Choices[0].Outcome.TriggerEncounterID = introEvent_1;
            }
            catch (Exception e)
            {
            Logger.Error(e);
            }
        }
    }
}
