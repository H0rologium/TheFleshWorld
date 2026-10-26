using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class IncidentWorker_MakeFleshyGameCondition : IncidentWorker_MakeGameCondition
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            GameConditionManager activeConditionManager = parms.target.GameConditionManager;
            return (!TheFleshTools.anomalyShutOff() && !activeConditionManager.IsAlwaysDarkOutside && !activeConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse));
        }
    }
}
