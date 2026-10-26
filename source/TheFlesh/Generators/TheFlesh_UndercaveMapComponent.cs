using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TheFlesh.Generators
{
    public class TheFlesh_UndercaveMapComponent : UndercaveMapComponent
    {
        #region Fields
        private static readonly IntRange SpawnGroupSize = new IntRange(3, 6);
        private static readonly SimpleCurve ThreatPointsCurve = new SimpleCurve
        {
            {
                new CurvePoint(100f, 250f),
                true
            },
            {
                new CurvePoint(500f, 800f),
                true
            },
            {
                new CurvePoint(1000f, 1500f),
                true
            },
            {
                new CurvePoint(2000f, 2000f),
                true
            },
            {
                new CurvePoint(8000f, 3000f),
                true
            }
        };
        #endregion

        public TheFlesh_UndercaveMapComponent(Map map)
            : base(map)
        {
        }

        public override void MapGenerated()
        {
            Map sourceMap = this.SourceMap;
            object obj;
            if (sourceMap == null)
            {
                obj = null;
            }
            else
            {
                ListerThings listerThings = sourceMap.listerThings;
                obj = ((listerThings != null) ? listerThings.ThingsOfDef(InternalDefOf.tfMindPitGate).FirstOrDefault<Thing>() : null);
            }
            this.pitGate = obj as PitGate;
            this.exit = this.map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>() as PocketMapExit;
            if (this.pitGate == null)
            {
                Log.Warning("Pit gate was not found after generating undercave, if this map was created via dev tools you can ignore this");
                return;
            }
            if (this.exit == null)
            {
                Log.Error("Pit gate exit was not found after generating undercave");
                return;
            }
            PitGate pitGate = this.pitGate;
            float num = ((pitGate != null) ? pitGate.pointsMultiplier : 1f);
            List<Pawn> fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(TheFlesh_UndercaveMapComponent.ThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.SourceMap) * num), this.map, false);
            int num2 = 0;
            int num3 = TheFlesh_UndercaveMapComponent.SpawnGroupSize.RandomInRange;
            IntVec3 intVec;
            CellFinder.TryFindRandomCell(this.map, (IntVec3 c) => c.Standable(this.map) && !c.InHorDistOf(this.exit.Position, 10f), out intVec);
            foreach (Pawn pawn in fleshbeastsForPoints)
            {
                IntVec3 intVec2;
                CellFinder.TryFindRandomCellNear(intVec, this.map, 3, (IntVec3 c) => c.Standable(this.map), out intVec2, -1);
                if (intVec2.IsValid)
                {
                    GenSpawn.Spawn(pawn, intVec2, this.map, WipeMode.Vanish);
                }
                num2++;
                if (num2 >= num3)
                {
                    num2 = 0;
                    num3 = TheFlesh_UndercaveMapComponent.SpawnGroupSize.RandomInRange;
                    CellFinder.TryFindRandomCell(this.map, (IntVec3 c) => c.Standable(this.map) && !c.InHorDistOf(this.exit.Position, 10f), out intVec);
                }
            }
        }
    }
}
