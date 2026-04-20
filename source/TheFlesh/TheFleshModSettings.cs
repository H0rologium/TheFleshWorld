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
                lst.Label($"<color=#14a82d>{"TFMS_MI_BTS_Title".Translate()}</color>");
                lst.CheckboxLabeled("TFMS_MI_BTS_AASD_Title".Translate(), ref settings.alwaysAllyWithStarway, "TFMS_MI_BTS".Translate());
            }

            lst.GapLine();
            

            lst.CheckboxLabeled("TFMS_Contactor_Title".Translate(), ref settings.spreadonHit, "TFMS_Contactor".Translate());
            
            if (settings.spreadonHit)
            {
                lst.Label($"Chance per hit to spread infection: {(settings.chanceperhitToApply * 100f):F0}% (The default amount is {(settings.chanceperhitToApply * 100f):F0}%)");
                settings.chanceperhitToApply = lst.Slider(settings.chanceperhitToApply, 0f, 1f);

                lst.Label($"Severity per hit on infected creatures: {(settings.severityPerHit * 100f):F0}% (The default amount is {(settings.severityPerHit * 100f):F0}%)");
                settings.severityPerHit = lst.Slider(settings.severityPerHit, 0f, 1f);

            }
            
            lst.GapLine();

            lst.CheckboxLabeled("TFMS_Fingers_Title".Translate(), ref settings.allowInfectedFingerspikes, $"<color=#f57842>{"TFMS_RESTART".Translate()}</color>{"TFMS_Fingers".Translate()}");
            lst.CheckboxLabeled("TFMS_Surprises_Title".Translate(), ref settings.enableSurpriseVisits, "TFMS_Surprises".Translate());
            lst.CheckboxLabeled("TFMS_NEDrops_Title".Translate(), ref settings.lesserMindDrop, "TFMS_NEDrops".Translate());
           



            lst.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "The Flesh";
        }
    }
}
