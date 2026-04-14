

using RimWorld;
using System;
using Verse;

namespace TheFlesh
{
    public class IncidentWorker_TheFleshPitGate : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!ModsConfig.AnomalyActive || TheFleshTools.anomalyShutOff())
            {
                return false;
            }
            Map map = (Map)parms.target;
            return base.CanFireNowSub(parms) && 
                map.listerThings.ThingsOfDef(InternalDefOf.tfMindPitGate).Count == 0 &&
                map.listerThings.ThingsOfDef(ThingDefOf.PitGate).Count == 0 &&
                map.listerThings.ThingsOfDef(InternalDefOf.FleshLairSpawner).Count == 0 &&
                map.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).Count == 0;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            if (!LargeBuildingCellFinder.TryFindCell(out intVec, map, IncidentWorker_TheFleshPitGate.PitGateSpawnParms.ForThing(InternalDefOf.tfMindPitGate), null, null, false))
            {
                return false;
            }
            BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(InternalDefOf.FleshLairSpawner, null);
            PitGate pitGate = buildingGroundSpawner.ThingToSpawn as PitGate;
            pitGate.SetFaction(Faction.OfEntities, null);
            pitGate.pointsMultiplier = parms.pointMultiplier;
            GenSpawn.Spawn(buildingGroundSpawner, intVec, map, WipeMode.Vanish);
            base.SendStandardLetter(parms, buildingGroundSpawner, Array.Empty<NamedArgument>());
            return true;
        }

        public static readonly LargeBuildingSpawnParms PitGateSpawnParms = new LargeBuildingSpawnParms
        {
            ignoreTerrainAffordance = true
        };
    }
}
