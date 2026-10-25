using System.Collections.Generic;
using Verse;

namespace TheFlesh
{
    public class DeathActionProperties_TMBurst : DeathActionProperties
    {
        public DeathActionProperties_TMBurst() 
        {
            this.workerClass = typeof(DeathActionWorker_TMBurst);
        }


        public override IEnumerable<string> ConfigErrors()
        {
            if (this.intensity <= 0.01f)
            {
                yield return "deathActionWorkerClass is DeathActionProperties_TMBurst or subclass, but intensity <= 0.01";
            }
            
            yield break;
        }

        public float intensity;

    }
}
