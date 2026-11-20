using RimWorld;
using Verse;
using Verse.AI.Group;

namespace TheFlesh
{
    public class IncidentWorker_FleshbeastHorde : IncidentWorker
    {
        private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
        {
            maxDistanceToColonyBuilding = -1f,
            minDistToEdge = 11,
            attemptNotUnderBuildings = true,
            canSpawnOnImpassable = false,
            attemptSpawnLocationType = SpawnLocationType.Outdoors
        };
        private const int MaxIters = 110;
        private const float FleshbeastHordePointFactor = 0.85f;
        private static readonly IntRange FleshBeastSpawnDelayTicks = new IntRange(190, 200);
        private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!ModsConfig.AnomalyActive || !LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().enableFleshHordeAssault)
                return false;
            Map map = (Map)parms.target;
            return base.CanFireNowSub(parms);
        }

        //protected static LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest)
        

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            float hordeScore = parms.points * FleshbeastHordePointFactor;
            List<Thing> spawnThings = new List<Thing>();
            int num2 = 0;
            while (hordeScore > 0f)
            {
                IntVec3 spawnVec;
                if (!LargeBuildingCellFinder.TryFindCell(out spawnVec, map, IncidentWorker_FleshbeastHorde.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow), null, null, false))
                    return false;
                float num3 = MathF.Min(hordeScore, 800f);//Spawn point cap of 800 compared to 500 for normal assaults
                Thing emergence = FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(spawnVec, map, num3, 
                    IncidentWorker_FleshbeastHorde.PitBurrowEmergenceDelayRangeTicks,
                    IncidentWorker_FleshbeastHorde.FleshBeastSpawnDelayTicks, true);
                spawnThings.Add(emergence);
                hordeScore -= num3;
                num2++;
                if (num2 > MaxIters)
                    break;
            }
            base.SendStandardLetter(this.def.letterLabel, (spawnThings.Count > 1) ? this.def.letterTextPlural : this.def.letterText, this.def.letterDef, parms, spawnThings, Array.Empty<NamedArgument>());
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            //LordMaker.MakeNewLord(parms.faction, this.GenerateLordJob(X, Y), map, spawnThings);
            return base.TryExecuteWorker(parms);
        }
    }
}
