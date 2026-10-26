using RimWorld;
using System;
using System.Linq;
using Verse;

namespace TheFlesh
{
    public class Alert_EmergingFleshDeepenedPitGate : Alert
    {
        public Alert_EmergingFleshDeepenedPitGate()
        {
            this.defaultPriority = AlertPriority.High;
            this.requireAnomaly = true;
        }

        public override string GetLabel()
        {
            return "AlertEmergingPitGate".Translate() + ": " + this.pitGate.TicksUntilSpawn.ToStringTicksToPeriodVerbose(true, true);
        }

        public override TaggedString GetExplanation()
        {
            return string.Format("AlertEmergingPitGateDesc".Translate(), Array.Empty<object>());
        }


        public override AlertReport GetReport()
        {
            return AlertReport.CulpritIs(this.EmergingPitGate);
        }

        private BuildingGroundSpawner EmergingPitGate
        {
            get
            {
                Map currentMap = Find.CurrentMap;
                this.pitGate = ((currentMap != null) ? currentMap.listerThings.ThingsOfDef(InternalDefOf.FleshLairSpawner).FirstOrDefault<Thing>() : null) as BuildingGroundSpawner;
                return this.pitGate;
            }
        }

        private BuildingGroundSpawner pitGate;
    }
}
