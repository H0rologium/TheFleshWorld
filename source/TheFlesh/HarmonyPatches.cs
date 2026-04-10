using Verse;
using HarmonyLib;

namespace TheFlesh
{
    public class HarmonyPatches
    {
        public HarmonyPatches()
        {
            Log.Message("Constructing patches for The Flesh");
            return;
        }


        [StaticConstructorOnStartup]
        public static class HarmonyInit
        {
            static HarmonyInit()
            {
                HarmonyInit.harmonyInstance.PatchAll();
            }

            public static Harmony harmonyInstance = new Harmony("TheFlesh.HarmonyPatches");
        }

        #region Patches

        [HarmonyPatch(typeof(Pawn), "PostApplyDamage")]
        public static class PatchHediffComps
        {
            [HarmonyPostfix]
            public static void AddMechaniteInfectionOnHit(DamageInfo dinfo, Pawn __instance)
            {
                if (__instance == null || (dinfo.Instigator as Pawn) == null) return;
                if (dinfo.Def.isRanged || dinfo.Def.isExplosive) return;
                if (!LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().spreadonHit) return;
                if (!Rand.Chance(LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().chanceperhitToApply)) return;
                //Check if attacker has infection
                bool hasBaseinf = ((Pawn)dinfo.Instigator).health.hediffSet.HasHediff(InternalDefOf.tfInfection);
                bool hasAdvinf = ((Pawn)dinfo.Instigator).health.hediffSet.HasHediff(InternalDefOf.tfInfection_entity);
                if (hasBaseinf || hasAdvinf)
                {
                    if (TheFleshTools.isInfectible(__instance))
                    {
                        Hediff inff = __instance.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection);
                        if (inff != null)
                        {
                            inff.Severity += LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().severityPerHit;
                        }
                        else
                        {
                            __instance.health.AddHediff(HediffMaker.MakeHediff(InternalDefOf.tfInfection,__instance));
                        }
                    }
                }

            }
        }

        #endregion
        }
}
