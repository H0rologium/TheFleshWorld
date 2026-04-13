using RimWorld;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class IncidentWorker_MakeFleshyGameCondition : IncidentWorker_MakeGameCondition
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return !TheFleshTools.anomalyShutOff();
        }
    }
}
