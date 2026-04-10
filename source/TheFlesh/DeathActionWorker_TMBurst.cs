using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace TheFlesh
{
    public class DeathActionWorker_TMBurst : DeathActionWorker
    {
        private const int BLAST_BASE_RAD = 5;
        private const int ROT_DAMAGE_BASE = 7;
        public DeathActionProperties_TMBurst Props
        {
            get
            {
                return (DeathActionProperties_TMBurst)this.props;
            }
        }

        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            Pawn innerpawn = corpse.InnerPawn;
            if (innerpawn == null) return;
            if (innerpawn.IsCaravanMember() || !innerpawn.Spawned) return;
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, BLAST_BASE_RAD, DamageDefOf.AcidBurn, innerpawn, ROT_DAMAGE_BASE, DamageDefOf.AcidBurn.defaultArmorPenetration,SoundDef.Named("FleshmassBirth"));
            var nearbyes = GenRadial.RadialDistinctThingsAround(innerpawn.Position, innerpawn.Map, (BLAST_BASE_RAD * 1f),true).OfType<Pawn>().Where(p => TheFleshTools.isInfectible(p));
            foreach (Pawn p in nearbyes)
            {
                HediffSet hs = p.health.hediffSet;
                if (!hs.HasHediff(InternalDefOf.tfInfection))
                {
                    p.health.AddHediff(HediffMaker.MakeHediff(InternalDefOf.tfInfection, p));
                }
                p.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection).Severity += Props.intensity;
            }
        }
    }
}
