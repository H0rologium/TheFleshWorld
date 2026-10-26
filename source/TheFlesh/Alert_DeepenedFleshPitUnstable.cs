using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class Alert_DeepenedFleshPitUnstable : Alert_Critical
    {
        private PitGate PitGate
        {
            get
            {
                return this.targets[0].Thing as PitGate;
            }
        }
        protected override Color BGColor
        {
            get
            {
                PitGate pitGate = this.PitGate;
                if (pitGate != null && pitGate.CollapseStage == 1)
                {
                    return Color.clear;
                }
                return base.BGColor;
            }
        }

        public Alert_DeepenedFleshPitUnstable()
        {
            this.defaultLabel = "Alert_UndercaveUnstable".Translate();
            this.defaultExplanation = "Alert_UndercaveUnstableDesc".Translate();
            this.requireAnomaly = true;
        }

        private void CalculateTargets()
        {
            this.targets.Clear();
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                foreach (Thing thing in maps[i].listerThings.ThingsOfDef(InternalDefOf.tfMindPitGate))
                {
                    PitGate pitGate = (PitGate)thing;
                    if (pitGate.IsCollapsing)
                    {
                        this.targets.Add(pitGate);
                    }
                }
            }
        }

        public override string GetLabel()
        {
            return this.defaultLabel + ": " + this.PitGate.TicksUntilCollapse.ToStringTicksToPeriodVerbose(true, true);
        }

        public override AlertReport GetReport()
        {
            this.CalculateTargets();
            return AlertReport.CulpritsAre(this.targets);
        }
        
        private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
    }
}
