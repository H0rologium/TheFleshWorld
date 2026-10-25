
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TheFlesh.Generators
{
    public class GenStep_FleshLairPOI : GenStep
    {
        public override int SeedPart
        {
            get
            {
                return 26098423;
            }
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            Thing pitGateExit = map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>();
            Pawn dreadmeld = map.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => p.kindDef == PawnKindDefOf.Dreadmeld);
            int randomInRange = GenStep_FleshLairPOI.InterestPointCountRange.RandomInRange;
            List<IntVec3> interestPoints = new List<IntVec3>();
            for (int i = 0; i < randomInRange; i++)
            {
                Map map2 = map;
                Predicate<IntVec3> predicate = ((IntVec3 c) => c.Standable(map) && c.DistanceToEdge(map) > 5 && (pitGateExit == null || !c.InHorDistOf(pitGateExit.Position, 10f)) && (dreadmeld == null || !c.InHorDistOf(dreadmeld.Position, 10f)) && !interestPoints.Any((IntVec3 p) => c.InHorDistOf(p, 10f)));
                
                IntVec3 intVec;
                if (CellFinder.TryFindRandomCell(map2, predicate, out intVec))
                {
                    interestPoints.Add(intVec);
                }
            }
            foreach (IntVec3 intVec2 in interestPoints)
            {
                GenStep_FleshLairPOI.UnderCaveInterestKind underCaveInterestKind = Gen.RandomEnumValue<GenStep_FleshLairPOI.UnderCaveInterestKind>(false);
                foreach (IntVec3 intVec3 in GridShapeMaker.IrregularLump(intVec2, map, 20, null))
                {
                    foreach (Thing thing in intVec3.GetThingList(map).ToList<Thing>())
                    {
                        if (thing.def.destroyable)
                        {
                            Building edifice = intVec3.GetEdifice(map);
                            bool? flag;
                            if (edifice == null)
                            {
                                flag = null;
                            }
                            else
                            {
                                ThingDef def = edifice.def;
                                if (def == null)
                                {
                                    flag = null;
                                }
                                else
                                {
                                    BuildingProperties building = def.building;
                                    flag = ((building != null) ? new bool?(building.isNaturalRock) : null);
                                }
                            }
                            bool? flag2 = flag;
                            if (!flag2.GetValueOrDefault())
                            {
                                Building edifice2 = intVec3.GetEdifice(map);
                                if (((edifice2 != null) ? edifice2.def : null) != ThingDefOf.Fleshmass)
                                {
                                    continue;
                                }
                            }
                            thing.Destroy(DestroyMode.Vanish);
                        }
                    }
                }
                switch (underCaveInterestKind)
                {
                    case GenStep_FleshLairPOI.UnderCaveInterestKind.MushroomPatch:
                        this.GenerateMushroomPatch(map, intVec2);
                        break;
                    case GenStep_FleshLairPOI.UnderCaveInterestKind.CorpseGear:
                        this.GenerateCorpseGear(map, intVec2);
                        break;
                    case GenStep_FleshLairPOI.UnderCaveInterestKind.LootPile:
                        GenStep_FleshLairPOI.GenerateLootPile(map, intVec2);
                        break;
                    case GenStep_FleshLairPOI.UnderCaveInterestKind.SleepingFleshbeasts:
                        this.GenerateSleepingFleshbeasts(map, intVec2);
                        break;
                }
            }
        }

        private void GenerateMushroomPatch(Map map, IntVec3 cell)
        {
            List<ThingDef> list = new List<ThingDef>
            {
                ThingDefOf.Plant_HealrootWild,
                ThingDefOf.Glowstool,
                ThingDefOf.Bryolux,
                ThingDefOf.Agarilux
            };
            foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(cell, map, GenStep_FleshLairPOI.PatchSizeRange.RandomInRange, null))
            {
                ThingDef thingDef = list.RandomElement<ThingDef>();
                if (GenSpawn.CanSpawnAt(thingDef, intVec, map, null, true))
                {
                    map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
                    Thing thing = ThingMaker.MakeThing(thingDef, null);
                    if (Rand.Chance(0.7f) && GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Direct, null, null, null, 1))
                    {
                        Plant plant = thing as Plant;
                        if (plant != null)
                        {
                            plant.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
                        }
                    }
                }
            }
        }
        private void GenerateCorpseGear(Map map, IntVec3 cell)
        {
            List<ThingDef> list = new List<ThingDef>();
            list.Add(ThingDefOf.MedicineIndustrial);
            list.Add(ThingDefOf.MealSurvivalPack);
            list.Add(InternalDefOf.tfMechaniteSerum);
            Faction faction;
            Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
            pawn.health.SetDead();
            Corpse corpse = pawn.MakeCorpse(null, null);
            corpse.Age = Mathf.RoundToInt((float)(GenStep_FleshLairPOI.CorpseAgeRangeDays.RandomInRange * 60000));
            corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
            Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
            GenSpawn.Spawn(corpse, cell, map, WipeMode.Vanish);
            Thing thing = ThingMaker.MakeThing(list.RandomElement<ThingDef>(), null);
            thing.stackCount = GenStep_FleshLairPOI.GearStackCountRange.RandomInRange;
            GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 1);
        }

        public static void GenerateLootPile(Map map, IntVec3 cell)
        {
            int randomInRange = GenStep_FleshLairPOI.LootCountRange.RandomInRange;
            List<ThingDef> lootDrops = (new ThingDef[]{ InternalDefOf.tfMechaniteSerum, ThingDefOf.MedicineUltratech, ThingDefOf.Luciferium, ThingDefOf.Meat_Twisted, ThingDefOf.Gold, ThingDefOf.Silver, ThingDefOf.Bioferrite, ThingDefOf.Bioferrite, ThingDefOf.Bioferrite, ThingDefOf.Bioferrite, ThingDefOf.Bioferrite }).ToList<ThingDef>();
            Faction faction;
            Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
            for (int i = 0; i < randomInRange; i++)
            {
                Thing thingToSpawn = ThingMaker.MakeThing(lootDrops[i],null);
                thingToSpawn.stackCount = GenStep_FleshLairPOI.GearStackCountRange.RandomInRange;
                GenPlace.TryPlaceThing(thingToSpawn, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
                thingToSpawn.SetForbidden(true, true);
            }
        }

        private void GenerateSleepingFleshbeasts(Map map, IntVec3 cell)
        {
            int randomInRange = GenStep_FleshLairPOI.NumFleshbeastsRange.RandomInRange;
            for (int i = 0; i < randomInRange; i++)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(InternalDefOf.tfBloater, Faction.OfEntities, null);
                GenPlace.TryPlaceThing(pawn, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
                CompCanBeDormant compCanBeDormant;
                if (pawn.TryGetComp(out compCanBeDormant))
                {
                    compCanBeDormant.ToSleep();
                }
            }
        }

        private static readonly IntRange InterestPointCountRange = new IntRange(6, 10);
        private static readonly IntRange PatchSizeRange = new IntRange(50, 70);
        private static readonly IntRange CorpseAgeRangeDays = new IntRange(15, 120);
        private static readonly IntRange GearStackCountRange = new IntRange(2, 5);
        private static readonly IntRange LootCountRange = new IntRange(6, 10);
        private static readonly IntRange NumFleshbeastsRange = new IntRange(2, 4);
        private enum UnderCaveInterestKind
        {
            MushroomPatch,
            ChemfuelGenerator,
            InsectHive,
            CorpseGear,
            LootPile,
            SleepingFleshbeasts
        }
    }
}
