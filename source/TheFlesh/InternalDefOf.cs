using RimWorld;

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
    }
}
