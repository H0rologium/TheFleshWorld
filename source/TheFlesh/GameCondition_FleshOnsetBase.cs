using RimWorld;
using Verse;

namespace TheFlesh
{
    public class GameCondition_FleshOnsetBase : GameCondition
    {
        private const int EVENT_DELAY_TIME_TICKS = 21067;

        public override void GameConditionTick()
        {

            if (this.TicksPassed >= EVENT_DELAY_TIME_TICKS && TheFleshTools.isDaytime(GenLocalDate.HourFloat(base.SingleMap)))
            {
                GameCondition ngc = GameConditionMaker.MakeCondition(InternalDefOf.GCFleshDayBreak, (this.TicksLeft));
                Find.World.GameConditionManager.RegisterCondition(ngc);
                this.End();
            }
        }

        public override bool AllowEnjoyableOutsideNow(Map map)
        {
            return false;
        }

        public override float TemperatureOffset()
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks, 5f);
        }

        public override WeatherDef ForcedWeather()
        {
            return WeatherDefOf.Clear;
        }

        public override void End()
        {
            base.End();
            foreach (Map map in base.AffectedMaps)
            {
                map.weatherDecider.StartNextWeather();
            }
        }
    }
}
