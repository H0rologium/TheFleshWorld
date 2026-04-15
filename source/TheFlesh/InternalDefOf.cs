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
        public static ThingDef tfMechaniteSerum;
        public static ThingDef DyingFleshMind;

        public static FactionDef SWBioDivision;

        public static HediffDef tfInfection;
        public static HediffDef tfInfection_entity;
        public static HediffDef tfMechanitePurgerAdv;

        public static PawnKindDef tfNewborn;
        public static PawnKindDef tfBloater;
        public static PawnKindDef tfPolymeri;

        public static GameConditionDef GCFleshDayBreak;
        public static GameConditionDef FleshDayBreak_Onset;
        public static WeatherDef FleshDayBreak;

        public static EffecterDef tFleshMindDeath;
    }
}
