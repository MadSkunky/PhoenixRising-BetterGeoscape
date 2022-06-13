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
                GeoscapeEventDef intro0 = CommonMethods.CreateNewEvent(
                    introEvent_0, "BG_INTRO_0_TITLE", "BG_INTRO_0_DESCRIPTION", null);                
                GeoscapeEventDef intro1 = CommonMethods.CreateNewEvent(
                    introEvent_1, "BG_INTRO_1_TITLE", "BG_INTRO_1_DESCRIPTION", null);
               // intro1.GeoscapeEventData.Choices[0].Outcome.TriggerEncounterID = introEvent_0;
                GeoscapeEventDef intro2 = CommonMethods.CreateNewEvent(
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
