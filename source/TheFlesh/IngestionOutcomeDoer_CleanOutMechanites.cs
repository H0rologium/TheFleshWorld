
using RimWorld;
using System.Linq;
using Verse;

namespace TheFlesh
{
    public class IngestionOutcomeDoer_CleanOutMechanites : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            HediffDef mechOne = DefDatabase<HediffDef>.GetNamedSilentFail("SensoryMechanites");
            HediffDef mechTwo = InternalDefOf.tfInfection;
            HediffDef mechThree = DefDatabase<HediffDef>.GetNamedSilentFail("FibrousMechanites");
            HediffDef mechFour = DefDatabase<HediffDef>.GetNamedSilentFail("MuscleParasites");
            HediffDef mechFive = DefDatabase<HediffDef>.GetNamedSilentFail("GutWorms");
            int removedCount = 0;
            System.Collections.Generic.List<Hediff> hediffsToCheck = pawn.health.hediffSet.hediffs.ToList<Hediff>();
            
            while (hediffsToCheck.Count > 0)
            {
                Hediff h = hediffsToCheck.PopFront();
                foreach (HediffDef hediff in new HediffDef[] { mechOne, mechTwo, mechThree, mechFour, mechFive })
                {
                    if (h.def == hediff) pawn.health.RemoveHediff(h);
                    removedCount++;
                    continue;
                }
            }
            if (removedCount > 0 && pawn.RaceProps.Humanlike && !pawn.IsAnimal)
            {
                Hediff ns = pawn.health.AddHediff(HediffDefOf.FoodPoisoning,null,null,null);
            }
        }
    }
}
