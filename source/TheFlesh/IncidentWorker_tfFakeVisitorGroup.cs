using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace TheFlesh
{
    public class IncidentWorker_tfFakeVisitorGroup : IncidentWorker_NeutralGroup
    {
        protected virtual LordJob_VisitColony CreateLordJob(IncidentParms parms, List<Pawn> pawns)
        {
            IntVec3 intVec;
            RCellFinder.TryFindRandomSpotJustOutsideColony(pawns[0], out intVec);
            return new LordJob_VisitColony(parms.faction, intVec, null);
        }
        protected override void ResolveParmsPoints(IncidentParms parms)
        {
            if (parms.points >= 0f)
            {
                return;
            }
            parms.points = Rand.ByCurve(IncidentWorker_tfFakeVisitorGroup.PointsCurve);
        }
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (!base.TryResolveParms(parms) || TheFleshTools.anomalyShutOff(true))
            {
                return LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().enableSurpriseVisits;
            }
            List<Pawn> list = base.SpawnPawns(parms);
            if (list.Count == 0)
            {
                return false;
            }
            LordMaker.MakeNewLord(parms.faction, this.CreateLordJob(parms, list), map, list);
            foreach (Pawn faker in list) {Hediff twist =  TheFleshTools.TryInfectPawn(faker); twist.Severity = 0.955f; twist.Tended(0.05f, 0.5f); }
            this.SendLetter(parms, list, null, false);
            return true;
        }

        protected virtual void SendLetter(IncidentParms parms, List<Pawn> pawns, Pawn leader, bool traderExists)
        {
            TaggedString taggedString3;
            TaggedString taggedString4;
            if (pawns.Count == 1)
            {
                taggedString3 = "LetterLabelSingleVisitorArrives".Translate();
                taggedString4 = "SingleVisitorArrivesFleshprise".Translate(pawns[0].story.Title, parms.faction.NameColored, pawns[0].Name.ToStringFull, pawns[0].Named("PAWN")).AdjustedFor(pawns[0], "PAWN", true);
            }
            else
            {
                taggedString3 = "LetterLabelGroupVisitorsArrive".Translate();
                taggedString4 = "GroupVisitorsArriveFleshprise".Translate(parms.faction.NameColored);
            }
            base.SendStandardLetter(taggedString3, taggedString4, LetterDefOf.NeutralEvent, parms, pawns[0], Array.Empty<NamedArgument>());
        }

        private static readonly SimpleCurve PointsCurve = new SimpleCurve
        {
            {
                new CurvePoint(45f, 0f),
                true
            },
            {
                new CurvePoint(50f, 1f),
                true
            },
            {
                new CurvePoint(100f, 1f),
                true
            },
            {
                new CurvePoint(200f, 0.25f),
                true
            },
            {
                new CurvePoint(300f, 0.1f),
                true
            },
            {
                new CurvePoint(500f, 0f),
                true
            }
        };
    }
}
