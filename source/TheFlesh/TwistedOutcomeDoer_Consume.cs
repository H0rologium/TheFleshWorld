using RimWorld;
using Verse;

namespace TheFlesh
{
    public class TwistedOutcomeDoer_Consume : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (chanceToInfect.RandomInRange < 85)
            {
                if (TheFleshTools.isInfectible(pawn))
                {
                    Hediff newInfection = TheFleshTools.TryInfectPawn(pawn);
                    TheFleshTools.TryAddSeverity(pawn, 0.08f);
                    if (pawn.Faction == Faction.OfPlayer)
                    {
                        ChoiceLetter letter = LetterMaker.MakeLetter("TFITFM_Label".Translate(), "TFITFM_Text".Translate(), LetterDefOf.ThreatSmall, pawn, null, null);
                        Find.LetterStack.ReceiveLetter(letter);
                    }
                    else
                    {
                        TheFleshTools.TryAddSeverity(pawn, 0.5f);
                    }
                }
            }
        }


        private IntRange chanceToInfect = new IntRange(0, 100);
    }
}
