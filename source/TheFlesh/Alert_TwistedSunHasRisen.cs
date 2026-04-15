/*using RimWorld;
using System.Collections.Generic;
using Verse;

namespace TheFlesh
{
    public class Alert_TwistedSunHasRisen : Alert_Critical
    {
        public override string GetLabel()
        {
            if (GetReport().AnyCulpritValid) return "TEST LABEL";
            return this.defaultLabel;
        }
        public override AlertReport GetReport()
        {
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                if (this.HasTwistedSun(maps[i]))
                {
                    return true;
                }
            }
            return false;
        }
        private bool HasTwistedSun(Map map)
        {
            return (map.gameConditionManager.GetActiveCondition(InternalDefOf.FleshDayBreak_Onset) != null) || (map.gameConditionManager.GetActiveCondition(InternalDefOf.GCFleshDayBreak) != null);
        }
    }
}*/
