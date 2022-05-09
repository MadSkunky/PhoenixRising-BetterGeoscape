namespace PhoenixRising.BetterGeoscape
{
    // New config class.
    internal class ModConfig
    {  
        // Determines amount of scavenging missions available and type of mission (crates, vehicles, or soldiers)
        // Is setup at start of new game, so a game in progress will not be affected by a change in settings
        public int InitialScavSites = 8; // 16 on Vanilla
        public int ChancesScavCrates = 4; // 4 on Vanilla
        public int ChancesScavSoldiers = 1; // 1 on Vanilla
        public int ChancesScavGroundVehicleRescue = 1; // 1 on Vanilla 
        
        // Determines amount of resources gained in Events. 1f = 100% if Azazoth level.
        // 0.8f = 80% of Azazoth = Pre-Azazoth level (default on BetterGeoscape).
        // For example, to double amount of resources from current Vanilla (Azazoth level), change to 2f 
        // Can be applied to game in progress
        public float ResourceMultiplier = 0.8f;
       
        // Determines if diplomatic penalties are applied when cozying up to one of the factions by the two other factions
        // Can be applied to game in progress
        public bool DiplomaticPenalties = true;

        //If set to 1, shows when any error ocurrs. Do not change unless you know what you are doing.
        public int Debug = 1;

        //If set to true, the passenger module FAR-M will no longer regenerate Stamina in flight
        //For players who prefer having to come back to base more often
        public bool DisableStaminaRecuperatonModule = false;

        //If set to false, a disabled limb in tactical will not set the character's Stamina to zero in geo
        public bool StaminaPenaltyFromInjury = true;

        // Below are advanced settings. The mod was designed to be played with all of them turned on.
        // If set to true activates DLC4 Corrupted Horizons story rework
        public bool ActivateCHRework = true;
        // If set to true activates DLC5 Kaos Engines story rework (in progress)
        public bool ActivateKERework = true;
        // If set to true reversing engineering an item allows to research the faction technology that allows manufacturing the item 
        public bool ActivateReverseEngineeringResearch = true;
        // If set to true changes all aircraft (except Anu Blimp) so that they cost less but seat 4 less.
        // There are 3 different passenger modules that give 4 extra seats and give one additional bonus  
        public bool ActivateInterceptors = true;
        
    }
}
