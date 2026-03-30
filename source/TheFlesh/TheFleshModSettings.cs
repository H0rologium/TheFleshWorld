using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class TheFleshModSettings : ModSettings
    {
        public bool alwaysAllyWithStarway;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref alwaysAllyWithStarway, "alwaysAlly", true);
            base.ExposeData();
        }
    }

    public class TheFlesh : Mod
    {
        TheFleshModSettings settings;

        public TheFlesh(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<TheFleshModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard lst = new Listing_Standard();

            lst.Begin(inRect);

            if (ModsConfig.IsActive("Horo.BTEOTW.betweenthestars"))
            {
                lst.CheckboxLabeled("Always Ally Starway divisions", ref settings.alwaysAllyWithStarway, "Whenever you generate a new world, starway factions will never be enemies with each other. Turning this setting off allows them to possibly be enemies with each other.");
            }
            lst.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "The Flesh";
        }
    }
}
