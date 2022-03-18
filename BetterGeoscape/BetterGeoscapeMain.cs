using Harmony;
using PhoenixPoint.Home.View.ViewModules;
using System;

namespace PhoenixRising.BetterGeoscape
{
    public class BetterGeoscapeMain
    {
        // New config field.
        internal static ModConfig Config;
        public static void HomeMod(Func<string, object, object> api)
        {
            // Read config and assign to the local field 'Config'.
            Config = api("config", null) as ModConfig ?? new ModConfig();

            // Only needed if Harmony patches are used
            //HarmonyInstance.Create("PhoenixRising.VolandsPlayground").PatchAll();
            
            // Modnix logging
            api("log verbose", "Mod Initialised.");

            // call Volands playground method
            VolandsPlayground.Apply_Changes();
        }
        public static void GeoscapeOnHide()
        {

        }
        public static void TacticalOnHide()
        {

        }
    }
}
