using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
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
            if (corpse == null) return;
            if (corpse.InnerPawn == null) return;
            //instigator is null here because i put 'null' in the parameter, hope that helps
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, BLAST_BASE_RAD, DamageDefOf.AcidBurn, null, ROT_DAMAGE_BASE, DamageDefOf.AcidBurn.defaultArmorPenetration,SoundDef.Named("FleshmassBirth"),doVisualEffects:false);
            //once again a side effect of being sick... is there no nondistinct version of RadialDistinctThingsAround?
            var nearbyes = corpse.Map.mapPawns.AllPawnsSpawned.Where(pt => 
                (pt.Position.InHorDistOf(corpse.Position,(BLAST_BASE_RAD*1f)) &&
                TheFleshTools.isInfectible(pt)));
            foreach (Pawn p in nearbyes)
            {
                TheFleshTools.TryInfectPawn(p);
                TheFleshTools.TryAddSeverity(p, Props.intensity);
            }
        }
    }
}
