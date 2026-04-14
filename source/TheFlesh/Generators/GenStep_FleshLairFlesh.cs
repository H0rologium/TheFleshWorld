using RimWorld;
using Verse;

namespace TheFlesh.Generators
{
    public class GenStep_FleshLairFlesh : GenStep
    {
        public override int SeedPart
        {
            get
            {
                return 98721367;
            }
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            foreach (IntVec3 intVec in map.AllCells)
            {
                if (map.generatorDef.isUnderground)
                {
                    Building edifice = intVec.GetEdifice(map);
                    if ((edifice == null || (this.fleshmassCanReplaceBuildings && !edifice.def.building.isNaturalRock)) && intVec.GetAffordances(map).Contains(ThingDefOf.Fleshmass.terrainAffordanceNeeded))
                    {
                        GenSpawn.Spawn(ThingDefOf.Fleshmass, intVec, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
                        map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Flesh);
                        ThingDef thingDef = (Rand.Bool ? ThingDefOf.Filth_TwistedFlesh : ThingDefOf.Filth_Blood);
                    }
                }
            }
        }

        private bool fleshmassCanReplaceBuildings;
    }
}
