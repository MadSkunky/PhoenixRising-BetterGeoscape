namespace PhoenixRising.BetterGeoscape
{
    // New config class.
    internal class ModConfig
    {  
       
        // These settings determine amount of resources player can acquire:
       
        // Determines amount of scavenging missions available and type of mission (crates, vehicles, or soldiers)
        // Is setup at start of new game, so game in progress will not be affected by change in settings
        public int InitialScavSites = 8; // 16 on Vanilla
        public int ChancesScavCrates = 4; // 4 on Vanilla
        public int ChancesScavSoldiers = 1; // 1 on Vanilla
        public int ChancesScavGroundVehicleRescue = 1; // 1 on Vanilla 
        
        // Determines amount of resources gained in Events. 1f = 100% if Azazoth level.
        // 0.8f = 80% of Azazoth = Pre-Azazoth level (default on BetterGeoscape).
        // For example, to double amount of resources from current Vanilla (Azazoth level), change to 2f 
        // Can be applied to game in progress
        public float ResourceMultiplier = 0.8f;
       

        // Changing the settings below will make the game easier:
        
        // Determines if diplomatic penalties are applied when cozying up to one of the factions by the two other factions
        // Can be applied to game in progress
        public bool DiplomaticPenalties = true;
       
        // If set to false, a disabled limb in tactical will not set character's Stamina to zero in geo
        public bool StaminaPenaltyFromInjury = true;
       
        // If set to false, applying a mutation will not set character's Stamina to zero
        public bool StaminaPenaltyFromMutation = true;
        
        // If set to false, adding a bionic will not set character's Stamina to zero
        public bool StaminaPenaltyFromBionics = true;

        // If set to false, ambushes will happen as rarely as in Vanilla, and will not have crates in them
        public bool MoreAmbushes = true;

       
        // Changing the settings below will make the game harder:
        
        // If set to true, the passenger module FAR-M will no longer regenerate Stamina in flight
        // For players who prefer having to come back to base more often
        public bool DisableStaminaRecuperatonModule = false;
        
        // If set to true reversing engineering an item allows to research the faction technology that allows manufacturing the item 
        public bool ActivateReverseEngineeringResearch = true;


        // Below are advanced settings. The mod was designed to be played with all of them turned on
        
        // If set to true changes some DLC1 and DLC2 events to give player more options and different rewards
        // Also replaces all LOTA Schemata mission with a sigle mission
        public bool ActivateChangesToDLC1andDLC2 = true;
        
        // If set to true activates DLC3 Festering Skies story rework
        public bool ActivateFSRework = true;

        // If set to true changes all aircraft (except Anu Blimp) so that they cost less but seat 4 less.
        // There are 3 different passenger modules that give 4 extra seats and give one additional bonus  
        public bool ActivateInterceptors = true;
       
        // If set to true activates DLC4 Corrupted Horizons story rework
        public bool ActivateCHRework = true;
       
        // If set to true activates DLC5 Kaos Engines story rework (in progress)
        public bool ActivateKERework = true;


        //If set to 1, shows when any error ocurrs. Do not change unless you know what you are doing.
        public int Debug = 1;
    }
}
