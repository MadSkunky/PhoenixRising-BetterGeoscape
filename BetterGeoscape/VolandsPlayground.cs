using Base;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using Base.UI;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Addons;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Common.View.ViewControllers.Inventory;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using PhoenixPoint.Geoscape.Entities.Missions;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.InterceptionPrototype.UI;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.View.ViewControllers.AugmentationScreen;
using PhoenixPoint.Geoscape.View.ViewControllers.Inventory;
using PhoenixPoint.Geoscape.View.ViewModules;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.ActorsInstance;
using PhoenixPoint.Tactical.Entities.Effects;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Levels;
using PhoenixPoint.Tactical.Levels.FactionObjectives;
using PhoenixPoint.Tactical.View.ViewControllers;
using PhoenixPoint.Tactical.View.ViewStates;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PhoenixRising.BetterGeoscape
{
    class VolandsPlayground
    {
        
        private static readonly DefRepository Repo = BetterGeoscapeMain.Repo;

        public static void Apply_Changes()
        {
            // @Voland: play down form here
            try
            {
                //ID all the factions for later
                GeoFactionDef PhoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoFactionDef NewJericho = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("NewJericho_GeoFactionDef"));
                GeoFactionDef Anu = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Anu_GeoFactionDef"));
                GeoFactionDef Synedrion = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Synedrion_GeoFactionDef"));

                //Source for creating new events
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));              
                        

                //Experiment new HD
                // var survive3turnsobjective = AmbushALN.CustomObjectives[0];
                // CustomMissionTypeDef ALNvsANUHD = Repo.GetAllDefs<CustomMissionTypeDef>().FirstOrDefault(ged => ged.name.Equals("HavenDefAlienAN_CustomMissionTypeDef"));
                //CustomMissionTypeDef NewALNvsANUHD = Helper.CreateDefFromClone(AmbushALN, "C5BD29BE-2A61-4C5E-A578-F58FCB40BE14", "NewHavenDefAlienAN_CustomMissionTypeDef");
                // ALNvsANUHD.ParticipantsData[0].InfiniteReinforcements = true;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Max = 0.3f;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsDeploymentPart.Min = 0.3f;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Max = 1;
                // ALNvsANUHD.ParticipantsData[0].ReinforcementsTurns.Min = 1;
                // ALNvsANUHD.CustomObjectives[0]=survive3turnsobjective;

                //Change medbay
                //PhoenixFacilityDef medicalBay_PhoenixFacilityDef = Repo.GetAllDefs<PhoenixFacilityDef>().FirstOrDefault(ged => ged.name.Equals("MedicalBay_PhoenixFacilityDef"));
                HealFacilityComponentDef e_HealMedicalBay_PhoenixFacilityDe = Repo.GetAllDefs<HealFacilityComponentDef>().FirstOrDefault(ged => ged.name.Equals("E_Heal [MedicalBay_PhoenixFacilityDef]"));
                e_HealMedicalBay_PhoenixFacilityDe.BaseHeal = 16;

                //Bonus damage from corruption reduce to 0%
                CorruptionStatusDef corruption_StatusDef = Repo.GetAllDefs<CorruptionStatusDef>().FirstOrDefault(ged => ged.name.Equals("Corruption_StatusDef"));
                corruption_StatusDef.Multiplier = 0.0f;
            }

            catch (Exception e)
            {
                Logger.Error(e);
            }
        }   
            
    }

}

