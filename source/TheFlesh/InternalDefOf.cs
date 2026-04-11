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

        public static FactionDef SWBioDivision;
        public static HediffDef tfInfection;
        public static HediffDef tfInfection_entity;
        public static PawnKindDef tfNewborn;
    }
}
