using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TheFlesh
{
    public class GameCondition_FleshDayBreakBase : GameCondition_ForceWeather
    {
        private const float SEVERITY_PER_BLAST = 0.2f;
        private readonly HediffDef[] buffDefs = { HediffDefOf.FrenzyField, HediffDefOf.BloodRage };
        public override void Init()
        {
            foreach (Map map in base.AffectedMaps)
            {
                map.gameConditionManager.SetTargetBrightness(1f, 5f);
            }
            base.Init();
        }

        public override void GameConditionTick()
        {
            if (GenLocalDate.HourFloat(this.gameConditionManager.ownerMap) == 18.0f) Messages.Message("TwistedSunNightfall".Translate(), MessageTypeDefOf.NeutralEvent, false);
            if (GenLocalDate.HourFloat(this.gameConditionManager.ownerMap) == 3.0f) Messages.Message("TwistedSunMorning".Translate(), MessageTypeDefOf.ThreatBig, false);


            if (!(Find.TickManager.TicksGame % 167 == 0) || !TheFleshTools.isDaytime(GenLocalDate.HourFloat(this.gameConditionManager.ownerMap))) return;
            foreach (Map map in base.AffectedMaps)
            {
                List<Pawn> victims = map.mapPawns.AllPawnsSpawned.ToList<Pawn>();
                if (victims == null) continue;
                if (victims.Count < 1) continue;

                while (victims.Count > 0)
                {
                    Pawn v = victims.PopFront();
                    if (!v.Position.Roofed(map) && TheFleshTools.isInfected(v,true)) { buffEntity(v); continue; }
                    if ( v.Position.Roofed(map) || !TheFleshTools.isInfectible(v) || v.Dead) continue;
                    Hediff iln = TheFleshTools.TryInfectPawn(v);
                    if (iln == null) TheFleshTools.TryAddSeverity(v, SEVERITY_PER_BLAST);
                }
            }
        }


        private void buffEntity(Pawn entity)
        {
            if (!TheFleshTools.isDaytime(GenLocalDate.HourFloat(this.gameConditionManager.ownerMap))) return;
            Hediff hasFrenzy = entity.health.hediffSet.GetFirstHediffOfDef(buffDefs[0]);
            Hediff hasRage = entity.health.hediffSet.GetFirstHediffOfDef(buffDefs[1]);
            if (hasFrenzy == null) { addBuffToEntity(entity, buffDefs[0]); hasFrenzy = entity.health.hediffSet.GetFirstHediffOfDef(buffDefs[0]); }
            if (hasRage == null) { addBuffToEntity(entity, buffDefs[1]); hasRage = entity.health.hediffSet.GetFirstHediffOfDef(buffDefs[1]); }
            hasFrenzy.Severity += 0.02f;
            hasRage.Severity += 0.02f;
        }
        private void addBuffToEntity(Pawn entity, HediffDef bta) => entity.health.AddHediff(bta);

        public override float TemperatureOffset()
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks, 5f);
        }
        public override WeatherDef ForcedWeather()
        {
            return InternalDefOf.FleshDayBreak;
        }
        public override bool AllowEnjoyableOutsideNow(Map map)
        {
            return !(base.SingleMap.weatherManager.curWeather == InternalDefOf.FleshDayBreak);
        }

        public override void End()
        {
            base.End();
            foreach (Map map in base.AffectedMaps)
            {
                map.weatherDecider.StartNextWeather();
                map.weatherManager.TransitionTo(WeatherDefOf.Clear);
            }
        }
    }
}
