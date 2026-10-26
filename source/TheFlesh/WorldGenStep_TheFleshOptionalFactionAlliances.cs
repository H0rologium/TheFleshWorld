using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;

namespace TheFlesh
{
    
    public class WorldGenStep_TheFleshOptionalFactionAlliances : Verse.WorldGenStep
    {
        public override int SeedPart
        {
            get
            {
                return 777998367;
            }
        }

        public override void GenerateFresh(string seed, PlanetLayer layer)
        { 
            
            if (ModsConfig.IsActive("Horo.BTEOTW.betweenthestars") && LoadedModManager.GetMod<TheFlesh>().GetSettings<TheFleshModSettings>().alwaysAllyWithStarway)
            {
                FactionDef BTS_Starway = DefDatabase<FactionDef>.GetNamedSilentFail("BTS_Starway");
                //Contrary to popular belief, Find.FactionManager.FirstFactionOfDef(InternalDefOf.BTS_Starway) WILL create a nullref when the player chooses
                //To not include the faction during world gen. This is why the below logic exists
                if ((new FactionDef[]{ BTS_Starway, InternalDefOf.SWBioDivision}).All(y => Find.FactionManager.AllFactionsListForReading.Any(x => (x?.def == y))))
                {
                    Faction us = Find.FactionManager.FirstFactionOfDef(InternalDefOf.SWBioDivision);
                    Faction them = Find.FactionManager.FirstFactionOfDef(BTS_Starway);
                    FactionRelation ourRelation = us.RelationWith(them,true);
                    ourRelation.baseGoodwill = 100;

                    ourRelation.kind = FactionRelationKind.Ally;

                    FactionRelation theirRelation = them.RelationWith(us,false);

                    theirRelation.baseGoodwill = ourRelation.baseGoodwill;
                    theirRelation.kind = ourRelation.kind;

                    return;
                }
            }
        }
    }
}
