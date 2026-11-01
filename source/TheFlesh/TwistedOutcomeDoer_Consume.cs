using RimWorld;
using Verse;

namespace TheFlesh
{
    public class TwistedOutcomeDoer_Consume : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Log.Error("Hey you hit me!");
        }
    }
}
