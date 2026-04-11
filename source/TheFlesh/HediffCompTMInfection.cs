using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace TheFlesh
{
    public class HediffCompTMInfection : HediffComp
    {
        //public override void CompPostTickInterval(ref float severityAdjustment, int delta)

        public override void Notify_PawnKilled()
        {
            if (parent.pawn == null) return;
            if (!parent.pawn.Spawned || parent.FullyImmune() || (parent.Severity < 0.79f) || parent.pawn.HasDeathRefusalOrResurrecting) return;
            //bool animalHost = parent.pawn.IsAnimal;
            Pawn offspring = PawnGenerator.GeneratePawn(InternalDefOf.tfNewborn,Faction.OfHoraxCult,null);
            GenSpawn.Spawn(offspring, CellFinder.StandableCellNear(parent.pawn.Position, parent.pawn.Map, 2f), parent.pawn.Map);
            FilthMaker.TryMakeFilth(parent.pawn.Position, parent.pawn.Map, ThingDefOf.Filth_Blood,count:30);
            SoundDefOf.FleshmassBirth.PlayOneShot(offspring);

            parent.pawn.equipment.DropAllEquipment(parent.pawn.Position);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            if (parent.pawn.HasDeathRefusalOrResurrecting) return;
            parent.pawn.Corpse.Destroy(DestroyMode.Vanish);
        }
    }
}
