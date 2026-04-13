

using RimWorld;
using System.Linq;
using Verse;

namespace TheFlesh
{
    public class IncidentWorker_AnimalInfectionSingle : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms) || TheFleshTools.anomalyShutOff(true))
            {
                return false;
            }
            Map map = (Map)parms.target;
            Pawn pawn;
            return this.TryFindRandomAnimal(map, out pawn);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            Pawn pawn;
            if (!this.TryFindRandomAnimal(map, out pawn))
            {
                return false;
            }
            Messages.Message("AnimalInfectMessage".Translate(),MessageTypeDefOf.NeutralEvent,true);
            Hediff infection = TheFleshTools.TryInfectPawn(pawn);
            infection.Severity = 0.92f;
            return true;
        }

        private bool TryFindRandomAnimal(Map map, out Pawn animal)
        {
            int maxPoints = 300;
            if (GenDate.DaysPassedSinceSettle < 7)
            {
                maxPoints = 40;
            }
            return map.mapPawns.AllPawnsSpawned.Where((Pawn p) => p.IsAnimal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)).TryRandomElement(out animal);
        }

    }
}
