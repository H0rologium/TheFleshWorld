using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

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
            Perlin perlin = new Perlin((double)this.fleshFrequency, 2.0, 0.5, this.noiseOctaves, Rand.Int, QualityMode.Medium);
            Perlin perlin2 = new Perlin((double)this.bloodFrequency, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
            MapGenFloatGrid caves = MapGenerator.Caves;
            foreach (IntVec3 intVec in map.AllCells)
            {
                if (!map.generatorDef.isUnderground || caves[intVec] > 0f)
                {
                    Building edifice = intVec.GetEdifice(map);
                    if ((edifice == null || (this.fleshmassCanReplaceBuildings && !edifice.def.building.isNaturalRock)) && intVec.GetAffordances(map).Contains(ThingDefOf.Fleshmass.terrainAffordanceNeeded))
                    {
                        float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
                        if (this.fleshmassFalloffRadius > 0f)
                        {
                            float num2 = intVec.DistanceTo(map.Center);
                            float num3 = 1f - Mathf.Clamp01((num2 - this.fleshmassFalloffRadius) / ((float)map.Size.x / 2f - this.fleshmassFalloffRadius));
                            num *= num3;
                        }
                        if (num > this.fleshThreshold)
                        {
                            GenSpawn.Spawn(ThingDefOf.Fleshmass, intVec, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
                        }
                        if (num > this.fleshTerrainThreshold)
                        {
                            map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Flesh);
                        }
                        float num4 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
                        ThingDef thingDef = (Rand.Bool ? ThingDefOf.Filth_TwistedFlesh : ThingDefOf.Filth_Blood);
                        if (num4 > this.bloodThreshold)
                        {
                            FilthMaker.TryMakeFilth(intVec, map, thingDef, 1, FilthSourceFlags.None, true);
                        }
                    }
                }
            }
        }

        private float fleshFrequency = 0.3f;
        private readonly bool fleshmassCanReplaceBuildings = false;
        private int noiseOctaves = 6;
        private float fleshmassFalloffRadius = -1f;
        private float bloodFrequency = 0.3f;
        private float fleshThreshold = 0.001f;
        private float fleshTerrainThreshold = 0.1f;
        private float bloodThreshold = 0.2f;
    }
}
