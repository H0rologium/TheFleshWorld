using RimWorld;
using Verse;

namespace TheFlesh
{
    public class HediffCompTwistedGut : HediffComp
    {
        private const int FEEDING_TICK_INTERVAL = 12500;//2,500 is about an ingame hour?
        private const float FEEDING_NUT_BASE = 0.3f;
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            if (!parent.pawn.IsHashIntervalTick(FEEDING_TICK_INTERVAL, delta) || parent.pawn.Dead) return;
            if (parent.pawn.needs.food.CurLevel < (parent.pawn.needs.food.MaxLevel - FEEDING_NUT_BASE))
            {
                float missingNutrition = parent.pawn.needs.food.NutritionWanted;
                parent.pawn.needs.food.CurLevel = (parent.pawn.needs.food.CurLevel + missingNutrition);
                parent.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AteRottenFood, null, null);
                parent.pawn.records.AddTo(RecordDefOf.NutritionEaten, missingNutrition);
            }
        }
    }
}
