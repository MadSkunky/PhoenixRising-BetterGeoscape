using System;
using System.IO;
using System.Reflection;
using Base.Core;
using Base.Defs;
using Harmony;

namespace PhoenixRising.BetterGeoscape
{
    public class BetterGeoscapeMain
    {
        // New config field.
        internal static ModConfig Config;

        internal static string LogPath;
        internal static string ModDirectory;
        internal static string LocalizationDirectory;
        internal static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        
        public static void HomeMod(Func<string, object, object> api)
        {
            InitBetterGeoscape(api);
            // Read config and assign to the local field 'Config'.
            Config = api("config", null) as ModConfig ?? new ModConfig();
            

            // Only needed if Harmony patches are used
            HarmonyInstance.Create("PhoenixRising.VolandsPlayground").PatchAll();

            // Modnix logging
            api("log verbose", "Mod Initialised.");

            // call Volands playground method
            VolandsPlayground.Apply_Changes();
            

            // Apply story rework changes (Voland)
            if (Config.ActivateCHRework)
            {
                CHReworkMain.Apply_Changes();
            }

            if (Config.ActivateReverseEngineeringResearch)
            {
                ReverseEgineeringResearch.Apply_Changes();
            }


        }
        public static void GeoscapeOnHide()
        {

        }
        public static void TacticalOnHide()
        {

        }

        public static void InitBetterGeoscape(Func<string, object, object> api)
        {
            // Read config and assign to config field.
            Config = api("config", null) as ModConfig ?? new ModConfig();
            // Path for own logging
            ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Path to localization CSVs
            LocalizationDirectory = Path.Combine(ModDirectory, "Assets", "Localization");
            
            // Initialize Logger
            LogPath = Path.Combine(ModDirectory, "BetterGeoscape.log");
            Logger.Initialize(LogPath, Config.Debug, ModDirectory, nameof(BetterGeoscapeMain));

            // Initialize Helper
            Helper.Initialize();
        }
    }
}
