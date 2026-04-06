using RimWorld;
using Verse;

namespace TheFlesh
{
    public class HediffCompTMInfection : HediffComp
    {
        //Okay, i admit, i am really sick right now and I cannot think of how to make this dynamic. todo?
        private const float infst2severity = 0.55f;
        private const float st2infst2severity = 0.75f;
        private const float criticalInfectionLevel = 0.83f;
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            if (parent.pawn.Dead || parent.pawn != null) return;
            bool entity = parent.pawn.IsEntity;
            bool animal = parent.pawn.IsAnimal;
            Log.WarningOnce($"Comp ticked for creature that is entity? {entity} animal? {animal}",1);
            int stage = GetSeverityLevelByPercent(parent.Severity);
            switch (stage)
            {
                case (100)://Fully lethal
                {
                    Log.Warning("infection complete");
                    ConvertToSludge();
                    return;
                }
                case (0)://Stage 2
                {
                    Log.Warning("Stage 2 of infection");
                    return;
                }
                case (1)://Stage 3
                {
                    Log.Warning("Stage 3 of infection");
                    return;
                }
                case (2)://Critical stage
                {
                    Log.Warning("Critical stage of infection");
                    return;
                }
                default://Any lesser amount
                { return; }
            }
        }


        private int GetSeverityLevelByPercent(float percent) => percent >= 1.0 ? 100 : (percent >= criticalInfectionLevel ? 2 : (percent >= st2infst2severity ? 1 : (percent >= infst2severity ? 0 : -1)));


        private void ConvertToSludge() 
        {
            Pawn victim = parent.pawn;
            Log.Error($"We should kill {victim.Name} here, who {(victim.Spawned == true ? "is" : "is not")} spawned");
            return;
        }

    }
}
