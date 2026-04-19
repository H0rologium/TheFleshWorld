using System;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class TheFleshModSettings : ModSettings
    {
        public bool alwaysAllyWithStarway = true;
        public bool spreadonHit = true;
        public bool allowInfectedFingerspikes = true;
        public bool enableSurpriseVisits = true;
        public bool lesserMindDrop = false;
        public float severityPerHit = 0.05f;
        public float chanceperhitToApply = 0.7f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref alwaysAllyWithStarway, "alwaysAlly");
            Scribe_Values.Look(ref spreadonHit, "alwaysHitSpread");
            Scribe_Values.Look(ref enableSurpriseVisits, "enablesurprisevisits");
            Scribe_Values.Look(ref allowInfectedFingerspikes, "allowSickFingers");
            Scribe_Values.Look(ref chanceperhitToApply, "onhitApplyChance");
            Scribe_Values.Look(ref severityPerHit, "tmsevperhit");
            Scribe_Values.Look(ref lesserMindDrop, "tfbossdropmid");
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

            if (ModLister.GetActiveModWithIdentifier("Horo.BTEOTW.betweenthestars", true) != null)
            {
                lst.Label("<color=#14a82d>Mod integrations - Between the Stars</color>");
                lst.CheckboxLabeled("Always Ally Starway divisions", ref settings.alwaysAllyWithStarway, "Whenever you generate a new world, starway factions will never be enemies with each other. Turning this setting off allows them to possibly be enemies with each other.");
            }

            lst.GapLine();
            

            lst.CheckboxLabeled("Infection spreads through contact", ref settings.spreadonHit, "When enabled, infected creatures can inflict the infection upon hitting another creature, or increase the severity of an existing infection.\n\nDisabling this only disables spread through direct combat.");
            
            if (settings.spreadonHit)
            {
                lst.Label($"Chance per hit to spread infection: %{(settings.chanceperhitToApply * 100f):F0} (The default amount is %{(settings.chanceperhitToApply * 100f):F0})");
                settings.chanceperhitToApply = lst.Slider(settings.chanceperhitToApply, 0f, 1f);

                lst.Label($"Severity per hit on infected creatures: %{(settings.severityPerHit * 100f):F0} (The default amount is %{(settings.severityPerHit * 100f):F0})");
                settings.severityPerHit = lst.Slider(settings.severityPerHit, 0f, 1f);

            }
            
            lst.GapLine();

            lst.CheckboxLabeled("Fingerspikes are also infected", ref settings.allowInfectedFingerspikes, "<color=#f57842>Requires a Game Restart to Apply Changes.</color>\n\nFingerspikes will not be infected with twisted mechanites if this is disabled");
            lst.CheckboxLabeled("Enable Surprises", ref settings.enableSurpriseVisits, "Sometimes someone can show up and be hiding a dangerous secret. Does not occur with wanderer / refugee join events.");
            lst.CheckboxLabeled("Nerf Elite Drops", ref settings.lesserMindDrop, "If enabled, nerfs miniboss drops from Persona Core to 5 Shards instead. This is in addition to its normal drops.");
           



            lst.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "The Flesh";
        }
    }
}
