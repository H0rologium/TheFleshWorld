using RimWorld;
using Verse;

namespace TheFlesh
{
    public class DyingFleshMind : Thing
    {
        protected override void Tick()
        {
            if (Find.TickManager.TicksGame >= this.tfcompleteTick)
            {
                this.Complete();
            }
        }

        public void InitWith()
        {
            Effecter effecter = InternalDefOf.tFleshMindDeath.SpawnMaintained(base.Position, base.Map, 1f);
            this.tfcompleteTick = base.TickSpawned + effecter.ticksLeft;
        }

        public void Complete()
        {
            if (FilthMaker.TryMakeFilth(base.PositionHeld, base.Map, ThingDefOf.Filth_RevenantBloodPool, 1, FilthSourceFlags.None, true))
            {
                EffecterDefOf.RevenantKilledCompleteBurst.SpawnMaintained(base.PositionHeld, base.Map, 1f);
                foreach (IntVec3 intVec in CellRect.CenteredOn(base.PositionHeld, 2))
                {
                    Plant plant = intVec.GetPlant(base.Map);
                    if (plant != null && plant.MaxHitPoints < 100)
                    {
                        plant.Destroy(DestroyMode.Vanish);
                    }
                }
            }
            bool nerfLoof = LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().lesserMindDrop;
            Thing thing = nerfLoof ? ThingMaker.MakeThing(ThingDefOf.Shard, null) : ThingMaker.MakeThing(ThingDefOf.AIPersonaCore, null);
            thing.stackCount = nerfLoof ? 5 : 1;
            GenSpawn.Spawn(thing, base.PositionHeld, base.Map, WipeMode.Vanish);
            this.Destroy(DestroyMode.Vanish);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.tfcompleteTick, "tfcompleteTick", 0, false);
        }

        private int tfcompleteTick;
    }
}
