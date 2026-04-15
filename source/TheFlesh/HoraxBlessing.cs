using RimWorld;
using Verse;

namespace TheFlesh
{
    public class HoraxBlessing : HediffComp
    {
        public override void Notify_PawnKilled()
        {
            if (!parent.pawn.RaceProps.IsAnomalyEntity) return;
            ((DyingFleshMind)GenSpawn.Spawn(InternalDefOf.DyingFleshMind, parent.pawn.Position, parent.pawn.Map, WipeMode.Vanish)).InitWith();
        }

    }
}
