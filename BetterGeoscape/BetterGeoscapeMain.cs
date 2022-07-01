using System;
using System.IO;
using System.Reflection;
using Base.Core;
using Base.Defs;
using Harmony;
using PhoenixPoint.Common.Core;

namespace PhoenixRising.BetterGeoscape
{
    public class BetterGeoscapeMain
    {
        // New config field.
        internal static ModConfig Config;

        internal static string LogPath;
        internal static string ModDirectory;
        internal static string LocalizationDirectory;
        internal static string TexturesDirectory;
        internal static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        internal static readonly SharedData Shared = GameUtl.GameComponent<SharedData>(); 
        
        internal static bool doNotLocalize = false;

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

            PandoranProgress.Apply_Changes();

            AirCombat.ModifyAirCombatDefs();

            Umbra.ChangeUmbra();

            if (Config.ActivateReverseEngineeringResearch) 
            { 
            ReverseEgineeringResearch.Apply_Changes();
            }

            if (Config.MoreAmbushes)
            {
                ChangesToAmbushes.Apply_Changes();
              //  HarmonyInstance.Create("PhoenixRising.ChangesToAmbushes").PatchAll();
            }

            if (Config.ActivateChangesToDLC1andDLC2) 
            {
                ChangesToEvents.Apply_Changes();
            }

            if (Config.ActivateFSRework) 
            {
                FesteringSkiesRework.ApplyChanges();
            }
           
            Resources.Apply_Changes();
            InfestationOptionMission.Apply_Infestation_Changes();

            if (Config.ActivateKERework)
            {
                KERework.Apply_Changes();
            }

            if (Config.DiplomaticPenalties)
            {
                DiplomacyPenalties.Apply_Changes();
            }
            
            // Apply story rework changes (Voland)
            if (Config.ActivateCHRework)
            {
                CHReworkMain.Apply_Changes();
           //     HarmonyInstance.Create("PhoenixRising.CHReworkMain").PatchAll();
                DeliriumPerks.Main();
            }

            if (Config.ActivateReverseEngineeringResearch)
            {
                ReverseEgineeringResearch.Apply_Changes();
            }

            if (Config.ActivateInterceptors)
            {
                Interceptors.Apply_Changes();
              //  HarmonyInstance.Create("PhoenixRising.Interceptors").PatchAll();
            }

            if (Config.DisableStaminaRecuperatonModule)
            {
                DisableHibernationPodStamina.Apply_Changes();
            }    

            if (Config.StaminaPenaltyFromInjury) 
            {
           //     HarmonyInstance.Create("PhoenixRising.InjuryStamina").PatchAll(); 
            }

            if (Config.StaminaPenaltyFromBionics)
            {
            //    HarmonyInstance.Create("PhoenixRising.AugmentationWOCH").PatchAll();
            }

            if (Config.StaminaPenaltyFromMutation) 
            {
            //    HarmonyInstance.Create("PhoenixRising.MutationStamina").PatchAll();
            }

        }
        public static void GeoscapeOnHide()
        {
           DeliriumPerks.Main();
        }
        public static void TacticalOnHide()
        {
           DeliriumPerks.Main();
        }

        public static void InitBetterGeoscape(Func<string, object, object> api)
        {
            // Read config and assign to config field.
            Config = api("config", null) as ModConfig ?? new ModConfig();
            // Path for own logging
            ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Path to localization CSVs
            LocalizationDirectory = Path.Combine(ModDirectory, "Assets", "Localization");
            //Texture Directory (for Dtony's DeliriumPerks)
            TexturesDirectory = Path.Combine(ModDirectory, "Assets", "Textures");
            // Initialize Logger
            LogPath = Path.Combine(ModDirectory, "BetterGeoscape.log");
            Logger.Initialize(LogPath, Config.Debug, ModDirectory, nameof(BetterGeoscapeMain));

            // Initialize Helper
            Helper.Initialize();
        }
    }
}
