

using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace TheFlesh
{
    public class CompFleshMind : ThingComp
    {
        private Pawn FleshMind
        {
            get
            {
                return (Pawn)this.parent;
            }
        }

        
        private HediffComp_Invisibility Invisibility
        {
            get
            {
                HediffComp_Invisibility hediffComp_Invisibility;
                if ((hediffComp_Invisibility = this.invisibility) == null)
                {
                    Hediff firstHediffOfDef = this.FleshMind.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility, false);
                    hediffComp_Invisibility = (this.invisibility = ((firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_Invisibility>() : null));
                }
                return hediffComp_Invisibility;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look<int>(ref this.lastDetectedTick, "lastDetectedTick", 0, false);
        }

        public override void CompTick()
        {
            if (this.FleshMind.IsShambler)
            {
                return;
            }
            if (this.Invisibility == null)
            {
                this.FleshMind.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null, null);
            }
            if (!this.FleshMind.Spawned)
            {
                return;
            }
            if (this.FleshMind.IsHashIntervalTick(7))
            {
                if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
                {
                    this.CheckDetected();
                }
                if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
                {
                    this.Invisibility.BecomeInvisible(false);
                }
            }
            Lord lord = this.FleshMind.GetLord();
            if (lord != null && Rand.MTBEventOccurs(600f, 1f, 1f))
            {
                Job curJob = this.FleshMind.CurJob;
                if (((curJob != null) ? curJob.def : null) == JobDefOf.Wait || lord.LordJob is LordJob_EntitySwarm)
                {
                    Pawn_CallTracker caller = this.FleshMind.caller;
                    if (caller == null)
                    {
                        return;
                    }
                    caller.DoCall(false);
                }
            }
        }

        private void CheckDetected()
        {
            foreach (Thing thing in this.FleshMind.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
            {
                Pawn pawn = (Pawn)thing;
                if (this.PawnCanDetect(pawn))
                {
                    if (!this.Invisibility.PsychologicallyVisible)
                    {
                        this.Invisibility.BecomeVisible(false);
                    }
                    this.lastDetectedTick = Find.TickManager.TicksGame;
                }
            }
        }

        private bool PawnCanDetect(Pawn pawn)
        {
            return pawn.Faction != Faction.OfEntities && !pawn.Downed && pawn.Awake() && !pawn.IsAnimal && this.FleshMind.Position.InHorDistOf(pawn.Position, CompFleshMind.GetPawnSightRadius(pawn, this.FleshMind)) && GenSight.LineOfSightToThing(pawn.Position, this.FleshMind, this.parent.Map, false, null);
        }

        private static float GetPawnSightRadius(Pawn pawn, Pawn sightstealer)
        {
            float num = 14f;
            if (pawn.genes == null || pawn.genes.AffectedByDarkness)
            {
                float num2 = sightstealer.Map.glowGrid.GroundGlowAt(sightstealer.Position, false, false);
                num *= Mathf.Lerp(0.33f, 1f, num2);
            }
            return num * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Sight);
        }

        public override void Notify_UsedVerb(Pawn pawn, Verb verb)
        {
            base.Notify_UsedVerb(pawn, verb);
            if (this.FleshMind.IsShambler)
            {
                return;
            }
            this.Invisibility.BecomeVisible(false);
            this.lastDetectedTick = Find.TickManager.TicksGame;
        }

        public override void Notify_BecameVisible()
        {
            SoundDefOf.Pawn_Sightstealer_Howl.PlayOneShotOnCamera(null);
            foreach (Thing thing in this.FleshMind.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
            {
                Pawn pawn = (Pawn)thing;
                if (pawn.kindDef == InternalDefOf.tfPolymeri && pawn != this.FleshMind && pawn.Position.InHorDistOf(this.FleshMind.Position, 30f) && !pawn.IsPsychologicallyInvisible() && GenSight.LineOfSight(pawn.Position, this.FleshMind.Position, pawn.Map))
                {
                    return;
                }
            }
            
            Messages.Message("FleshMindReveal".Translate(), this.FleshMind, MessageTypeDefOf.ThreatBig, true);
            
            CompFleshMind.lastNotified = RealTime.LastRealTime;
            this.lastDetectedTick = Find.TickManager.TicksGame;
        }

        [Unsaved(false)]
        private HediffComp_Invisibility invisibility;
        private int lastDetectedTick = -99999;
        private static float lastNotified = -99999f;
    }
}
