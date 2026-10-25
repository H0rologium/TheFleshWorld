using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TheFlesh.Generators
{
    public class GenStep_Boss : GenStep
    {
        public override int SeedPart
        {
            get
            {
                return 21341517;
            }
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            PocketMapExit pitGateExit = (PocketMapExit)map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).First<Thing>();
            IntVec3 intVec;
            CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.Standable(map) && !c.InHorDistOf(pitGateExit.Position, 20f) && c.DistanceToEdge(map) > 10, out intVec);
            List<IntVec3> dellist = GridShapeMaker.IrregularLump(intVec, map, 250);
            List<IntVec3> list = GridShapeMaker.IrregularLump(intVec, map, 150);
            List<IntVec3> list2 = GridShapeMaker.IrregularLump(intVec, map, 60);
            foreach (IntVec3 intVec1 in dellist)
            {
                foreach (Thing thing in from t in intVec1.GetThingList(map).ToList<Thing>()
                                        where t.def.destroyable
                                        select t)
                {
                    thing.Destroy(DestroyMode.Vanish);
                }
            }
            foreach (IntVec3 intVec2 in list)
            {
                GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass, null), intVec2, map, Rot4.Random, WipeMode.Vanish, false, false).SetFaction(Faction.OfEntities, null);
                map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Flesh);
            }
            foreach (IntVec3 intVec3 in list2)
            {
                foreach (Thing thing in from t in intVec3.GetThingList(map).ToList<Thing>()
                                        where t.def.destroyable
                                        select t)
                {
                    thing.Destroy(DestroyMode.Vanish);
                }
            }
            int randomInRange = GenStep_Boss.NumFleshBulbsRange.RandomInRange;
            
            for (int i = 0; i < randomInRange; i++)
            {
                IntVec3 intVec4 = intVec;
                Map map2 = map;
                int num = 5;
                Predicate<IntVec3> predicate = (IntVec3 c) => c.GetEdifice(map) == null;
                
                IntVec3 intVec5;
                if (CellFinder.TryFindRandomCellNear(intVec4, map2, num, predicate, out intVec5, -1))
                {
                    GenSpawn.Spawn(ThingDefOf.Fleshbulb, intVec5, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
                }
            }
            GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(InternalDefOf.tfPolymeri, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), intVec, map, Rot4.Random, WipeMode.Vanish, false, false);
            string text = "dreadmeldApproached-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
            CellRect cellRect = CellRect.FromCellList(list).ExpandedBy(2).ClipInsideMap(map);
            RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
            rectTrigger.signalTag = text;
            rectTrigger.Rect = cellRect;
            rectTrigger.destroyIfUnfogged = true;
            GenSpawn.Spawn(rectTrigger, cellRect.CenterCell, map, WipeMode.Vanish);
            SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
            signalAction_Letter.signalTag = text;
            signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
            signalAction_Letter.letterLabelKey = "LetterLabelDreadmeldWarning";
            signalAction_Letter.letterMessageKey = "LetterDreadmeldWarning";
            GenSpawn.Spawn(signalAction_Letter, cellRect.CenterCell, map, WipeMode.Vanish);
        }

        private static readonly IntRange NumFleshBulbsRange = new IntRange(2, 4);
    }
}
