using RimWorld;
using Verse;

namespace TheFlesh
{
    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
        public static ThingDef FleshLairSpawner;
        public static ThingDef tfMindPitGate;

        public static FactionDef SWBioDivision;

        public static HediffDef tfInfection;
        public static HediffDef tfInfection_entity;

        public static PawnKindDef tfNewborn;

        public static GameConditionDef GCFleshDayBreak;
        public static WeatherDef FleshDayBreak;
    }
}
