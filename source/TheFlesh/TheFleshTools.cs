using UnityEngine;
using Verse;

namespace TheFlesh
{
    [StaticConstructorOnStartup]
    public static class TheFleshTools
    {
        public static Material skyfleshmat;
        public static readonly SkyColorSet FleshSkyColors = new SkyColorSet(new Color(0.67f, 0.37f, 0.37f), Color.white, new Color(0.67f, 0.37f, 0.37f), 1f);

        static TheFleshTools()
        {
            skyfleshmat = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("Things/fleshmapbase"), ShaderDatabase.TransparentPostLight, Color.white);
        }

        #region Methods

        public static Hediff TryInfectPawn(Pawn victim)
        {
            Hediff activeinfection = victim.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection);
            if (activeinfection == null) activeinfection = victim.health.AddHediff(InternalDefOf.tfInfection); else return null;
            return activeinfection;
        }

        public static void TryAddSeverity(Pawn victim, float severity = 1f)
        {
            victim.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfInfection).Severity += severity;
        }

        #endregion

        #region Checks
        public static bool isInfectible(Pawn victim)
        {
            RaceProperties rProps = victim.RaceProps;
            return (victim.Spawned &&
                rProps.IsFlesh &&
                !victim.Dead &&
                !rProps.IsAnomalyEntity &&
                !rProps.Dryad &&
                !rProps.IsDrone &&
                !rProps.IsMechanoid &&
                !rProps.IsWorkMech
                );
        }

        public static bool isInfected(Pawn victim)
        {
            return (victim.health.hediffSet.HasHediff(InternalDefOf.tfInfection));
        }

        public static bool isInfected(Pawn victim, bool entitySpecific)
        {
            return (victim.health.hediffSet.HasHediff(InternalDefOf.tfInfection_entity) && victim.IsEntity == entitySpecific);
        }

        public static bool isDaytime(float curGameHour)
        {
            return (curGameHour >= 5f && curGameHour <= 18f);
        }

        public static bool isReadytoDie(Pawn victim)
        {
            return !(victim.HasDeathRefusalOrResurrecting || victim.Deathresting);
        }

        #endregion
    }
}
