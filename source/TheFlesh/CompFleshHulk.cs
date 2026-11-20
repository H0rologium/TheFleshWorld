using RimWorld;
using Verse;

namespace TheFlesh
{
    public class CompFleshHulk : ThingComp
    {
        #region Fields
        private static readonly IntRange SpikerCount = new IntRange(0, 2);
        private static readonly IntRange FingerCount = new IntRange(0, 1);
        private static readonly IntRange BloodFilthCountRange = new IntRange(1, 2);
        private float totalHulkDamageTaken;
        private const int SpawnDmgThreshHoldIncrements = 50;
        #endregion

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (!this.parent.Spawned) return;
            float preNum = this.totalHulkDamageTaken;
            this.totalHulkDamageTaken += totalDamageDealt;
            if ((int)preNum / SpawnDmgThreshHoldIncrements < this.totalHulkDamageTaken / SpawnDmgThreshHoldIncrements)
            {
                int randomFingerInRange = CompFleshHulk.FingerCount.RandomInRange;
                int randomSpikeInRange = CompFleshHulk.SpikerCount.RandomInRange;
                RimWorld.Faction fac = this.parent.Faction;
                PawnGenerationContext ctx = PawnGenerationContext.NonPlayer;
                if (randomFingerInRange > 0) SpawnMinions(randomFingerInRange, PawnKindDefOf.Fingerspike,fac,ctx);
                if (randomSpikeInRange > 0) SpawnMinions(randomSpikeInRange, InternalDefOf.tfFleshHulk,fac,ctx);
                FleshbeastUtility.MeatSplatter(CompFleshHulk.BloodFilthCountRange.RandomInRange, this.parent.PositionHeld, this.parent.MapHeld, FleshbeastUtility.MeatExplosionSize.Small);
                FilthMaker.TryMakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
            }
        }

        private void SpawnMinions(int count, PawnKindDef pawnType, Faction faction, PawnGenerationContext pawntext)
        {
            count--;
            Pawn spawnPawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnType, faction, pawntext,null,true,false,false,false,true,0f,false,false,false,false,false,false,false,false,false,0f,0f,null,0f,null,null,null,null,null,null,null,null,null,null,null));
            GenSpawn.Spawn(spawnPawn, this.parent.PositionHeld, this.parent.MapHeld, WipeMode.VanishOrMoveAside);
            FleshbeastUtility.SpawnPawnAsFlyer(spawnPawn, this.parent.MapHeld, this.parent.PositionHeld, 4, true);
            if (count > 0)
            {
                SpawnMinions(count, pawnType, faction, pawntext);
            }
        }
    }
}
