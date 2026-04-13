using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace TheFlesh
{
    public class PsychicRitualToil_PurgeMechanitesPlayer : PsychicRitualToil
    {
        public PsychicRitualToil_PurgeMechanitesPlayer(PsychicRitualRoleDef invokerRole)
        {
            this.invokerRole = invokerRole;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
            if (pawn == null)
            {
                return;
            }
            if (TheFleshTools.anomalyShutOff()) return;
            this.CleanMechanites(psychicRitual);
            Find.LetterStack.ReceiveLetter("CleanRitualCompleteLabel".Translate(psychicRitual.def.label), "CleanRitualCompleteText".Translate(pawn, psychicRitual.def.Named("RITUAL")), LetterDefOf.PositiveEvent,LookTargets.Invalid, null, null, null, null, 0, true);
        }

        private void CleanMechanites(PsychicRitual rit)
        {
            //float effectiveness = rit.PowerPercent;
            List<Pawn> aberrants = rit.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>();
            while (aberrants.Count > 0)
            {
                Pawn potential = aberrants.PopFront();
                if (TheFleshTools.isInfected(potential))
                {
                    potential.health.RemoveHediff(potential.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection));
                    //if (effectiveness < 0.49f) potential.health.
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
        }

        public PsychicRitualRoleDef invokerRole;

    }
}
