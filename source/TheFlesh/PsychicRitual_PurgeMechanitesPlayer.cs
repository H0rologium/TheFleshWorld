using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace TheFlesh
{
    public class PsychicRitual_PurgeMechanitesPlayer : PsychicRitualDef_InvocationCircle
    {
        private const float WEAKEN_THRESHOLD = 0.55f;
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
            list.Add(new PsychicRitualToil_PurgeMechanitesPlayer(this.InvokerRole));
            return list;
        }

        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return this.outcomeDescription.Formatted(qualityRange.min >= WEAKEN_THRESHOLD ? $"\n\n{"AdditionalRitualRewardTFREQ".Translate()}" : string.Empty);
        }
    }
}
