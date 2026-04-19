using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TheFlesh
{
    public class IngestionOutcomeDoer_FontOfLife : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn user, Thing thing , int count)
        {
            List<Hediff> injuries = user.health.hediffSet.hediffs.Where((Hediff hd) =>
                hd.IsPermanent() ||
                hd.def.chronic ||
                hd.def.isInfection).ToList();

            while (injuries.Count > 0)
            {
                Hediff latestHediff = injuries.PopFront();
                HealthUtility.Cure(latestHediff);
                if (PawnUtility.ShouldSendNotificationAbout(user))
                {
                    Messages.Message("MessagePermanentWoundHealed".Translate("HealCauseLabel".Translate(), user.LabelShort, latestHediff.Label, user.Named("PAWN")), user, MessageTypeDefOf.PositiveEvent, true);
                }
            }

        }
    }
}
