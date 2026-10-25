using RimWorld;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    [StaticConstructorOnStartup]
    public static class TheFleshTools
    {
        public static Material skyfleshmat;
        public static readonly SkyColorSet FleshSkyColors = new SkyColorSet(new Color(0.67f, 0.37f, 0.37f), Color.grey, new Color(0.67f, 0.37f, 0.37f), 1f);

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
            Hediff hasHediff = victim.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.tfMechanitePurgerAdv);
            RaceProperties rProps = victim.RaceProps;
            return (victim.Spawned &&
                rProps.IsFlesh &&
                hasHediff == null &&
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
            //IsEntity uses FACTION checking too
            return (victim.health.hediffSet.HasHediff(InternalDefOf.tfInfection_entity) && (victim.IsEntity == entitySpecific || entitySpecific == (victim.Faction == Faction.OfHoraxCult)));
        }

        public static bool isDaytime(float curGameHour)
        {
            return (curGameHour >= 5f && curGameHour <= 18f);
        }

        public static bool isDawning(float curGameHour)
        {
            return (curGameHour >= 3f && curGameHour < 5f);
        }

        public static bool isReadytoDie(Pawn victim)
        {
            return !(victim.HasDeathRefusalOrResurrecting || victim.Deathresting);
        }

        /// <summary>
        /// Checks if anomalous events are turned off. Does not check if anomaly is active or not.
        /// </summary>
        /// <param name="ignoreAmbientHorror">setting this to true will cause this method to ignore if ambient horror mode is on or not</param>
        /// <returns> true if ambient horror is on, or if the anomaly ending was reached and the connection to the void was disrupted</returns>
        public static bool anomalyShutOff(bool ignoreAmbientHorror=false)
        {
            return ((Find.Anomaly.AmbientHorrorMode && !ignoreAmbientHorror) || Find.Anomaly.LevelDef == MonolithLevelDefOf.Disrupted);
        }

        #endregion
    }
}
