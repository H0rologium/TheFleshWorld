using RimWorld;
using System.Collections.Generic;
using Verse.AI.Group;

namespace TheFlesh
{
    public class PsychicRitual_PurgeMechanitesPlayer : PsychicRitualDef_InvocationCircle
    {
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
            list.Add(new PsychicRitualToil_PurgeMechanitesPlayer(this.InvokerRole));
            return list;
        }
    }
}
