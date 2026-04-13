using RimWorld;
using Verse;
using Verse.Sound;

namespace TheFlesh
{
    public class HediffCompTMInfection : HediffComp
    {
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            if (!parent.pawn.IsHashIntervalTick(2500, delta)) return;
            if (TheFleshTools.anomalyShutOff(true)) removeHediff();
        }

        public override void Notify_PawnKilled()
        {
            if (parent.pawn == null) return;
            if (!parent.pawn.Spawned || parent.FullyImmune() || (parent.Severity < 0.79f) || parent.pawn.HasDeathRefusalOrResurrecting) return;
            Pawn offspring = PawnGenerator.GeneratePawn(InternalDefOf.tfNewborn,Faction.OfHoraxCult,null);
            GenSpawn.Spawn(offspring, CellFinder.StandableCellNear(parent.pawn.Position, parent.pawn.Map, 2f), parent.pawn.Map);
            FilthMaker.TryMakeFilth(parent.pawn.Position, parent.pawn.Map, ThingDefOf.Filth_Blood,count:30);
            SoundDefOf.FleshmassBirth.PlayOneShot(offspring);

            if (!parent.pawn.IsAnimal) parent.pawn.equipment.DropAllEquipment(parent.pawn.Position);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {

            if (parent.Severity < 0.9999f) return;
            if (!parent.pawn.IsAnimal)
            {
                if (!TheFleshTools.isReadytoDie(parent.pawn)) return;
            }
            removeHediff();
            parent.pawn.Corpse.Destroy(DestroyMode.Vanish);
        }

        private void removeHediff() => parent.pawn.health.RemoveHediff(parent.pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection));
    }
}
