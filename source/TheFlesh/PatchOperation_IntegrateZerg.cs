using System.Xml;
using Verse;

namespace TheFlesh
{
    public class PatchOperation_IntegrateZerg : PatchOperation
    {
        public PatchOperation inner;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().mi_zerg_enablecarriers)
            {
                return inner?.Apply(xml) ?? false;
            }
            return true;//false will fail the patch, we don't want to error, just to skip
        }
    }
}
