using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class Alert_TwistedSunHasRisen : Alert_Critical
    {
        #region fields
        protected override Color BGColor
        {
            get
            {
                return Alert_TwistedSunHasRisen.TFBgColor();
            }
        }
        private static Color TFBgColor()
        {
            int phase = GetSunPhase();
            float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
            return new Color(num, num, num) * ((phase == 0) ? Color.red : (phase == 1 ? Color.red : Color.blue));
        }

        private static Map tfmph;
        #endregion

        public override string GetLabel()
        {
            StringBuilder label = new StringBuilder();
            switch (GetSunPhase())
            {
                case 0:
                case 3:
                    label.Append("TwistedSunAlertLBL_Incoming".Translate());
                    break;
                case 1:
                    label.Append("TwistedSunAlertLBL_Active".Translate());
                    break;
                case 2:
                    label.Append("TwistedSunAlertLBL_Inactive".Translate());
                    break;
                default:
                    Log.WarningOnce("The Flesh sun alert is trying to set its label to an invalid value. Are we looking at the wrong map(s)?", 4192027);
                    label.Append(string.Empty);
                    break;
            }

            return label.ToString();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder explanation = new StringBuilder();
            switch (GetSunPhase())
            {
                case 0:
                case 3:
                    explanation.Append("TwistedSunAlertTXT_Incoming".Translate());
                    break;
                case 1:
                    explanation.Append("TwistedSunAlertTXT_Active".Translate());
                    break;
                case 2:
                    explanation.Append("TwistedSunAlertTXT_Inactive".Translate());
                    break;
                default:
                    Log.WarningOnce("The Flesh sun alert is trying to set its text to an invalid value. Are we looking at the wrong map(s)?",4192026);
                    explanation.Append(string.Empty);
                    break;
            }

            return explanation.ToString();
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

        private static int GetSunPhase()
        {
            Map map = Find.CurrentMap;
            if (map == null) return -1;

            if (tfmph.gameConditionManager.GetActiveCondition(InternalDefOf.FleshDayBreak_Onset) != null) return 0; else return (TheFleshTools.isDaytime(GenLocalDate.HourFloat(tfmph)) ? 1 : (TheFleshTools.isDawning(GenLocalDate.HourFloat(tfmph)) ? 3 : 2));
        }

        private bool HasTwistedSun(Map map)
        {
            if (!map.IsPlayerHome) return false;
            tfmph = map;
            return (map.gameConditionManager.GetActiveCondition(InternalDefOf.FleshDayBreak_Onset) != null) || (map.gameConditionManager.GetActiveCondition(InternalDefOf.GCFleshDayBreak) != null);
        }
    }
}
