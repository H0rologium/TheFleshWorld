using System;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    public class TheFleshModSettings : ModSettings
    {
        public bool mi_zerg_enablecarriers = false;
        public bool alwaysAllyWithStarway = true;
        public bool spreadonHit = true;
        public bool allowInfectedFingerspikes = true;
        public bool enableSurpriseVisits = true;
        public bool lesserMindDrop = false;
        public float severityPerHit = 0.03f;
        public float chanceperhitToApply = 0.7f;
        public float surpriseextrachance = 1.0f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref mi_zerg_enablecarriers, "tfmi_zergcancarrymechanites");
            Scribe_Values.Look(ref alwaysAllyWithStarway, "alwaysAlly");
            Scribe_Values.Look(ref spreadonHit, "alwaysHitSpread");
            Scribe_Values.Look(ref enableSurpriseVisits, "enablesurprisevisits");
            Scribe_Values.Look(ref allowInfectedFingerspikes, "allowSickFingers");
            Scribe_Values.Look(ref chanceperhitToApply, "onhitApplyChance");
            Scribe_Values.Look(ref severityPerHit, "tmsevperhit");
            Scribe_Values.Look(ref lesserMindDrop, "tfbossdropmid");
            Scribe_Values.Look(ref surpriseextrachance, "surpriseExtraChance");
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

            lst.GapLine();

            if (ModLister.GetActiveModWithIdentifier("Horo.BTEOTW.betweenthestars", true) != null)
            {
                lst.Label($"<color=#14a82d>{"TFMS_MI_BTS_Title".Translate()}</color>");
                lst.CheckboxLabeled("TFMS_MI_BTS_AASD_Title".Translate(), ref settings.alwaysAllyWithStarway, "TFMS_MI_BTS".Translate());
                lst.GapLine();
            }

            

            if (ModLister.GetActiveModWithIdentifier("mustard1557.musc.zerg", true) != null)
            {
                lst.Label($"<color=#14a82d>{"TFMS_MI_ZERG_Title".Translate()}</color>");
                lst.CheckboxLabeled("TFMS_MI_ZERG_EZC_Title".Translate(), ref settings.mi_zerg_enablecarriers, $"<color=#f57842>{"TFMS_RESTART".Translate()}</color>{"TFMS_MI_ZERG".Translate()}");
                lst.GapLine();
            }



            lst.CheckboxLabeled("TFMS_Contactor_Title".Translate(), ref settings.spreadonHit, "TFMS_Contactor".Translate());
            
            if (settings.spreadonHit)
            {
                lst.Label($"Chance per hit to spread infection: {(settings.chanceperhitToApply * 100f):F0}% (The default amount is 70%)");
                settings.chanceperhitToApply = lst.Slider(settings.chanceperhitToApply, 0f, 1f);

                lst.Label($"Severity per hit on infected creatures: {(settings.severityPerHit * 100f):F0}% (The default amount is 3%)");
                settings.severityPerHit = lst.Slider(settings.severityPerHit, 0f, 1f);

            }
            
            lst.GapLine();

            lst.CheckboxLabeled("TFMS_Fingers_Title".Translate(), ref settings.allowInfectedFingerspikes, $"<color=#f57842>{"TFMS_RESTART".Translate()}</color>{"TFMS_Fingers".Translate()}");
            lst.CheckboxLabeled("TFMS_Surprises_Title".Translate(), ref settings.enableSurpriseVisits, "TFMS_Surprises".Translate());
            if (settings.enableSurpriseVisits)
            {
                lst.Label($"{"TFMS_ExtraSurpriseRoll_Title".Translate()}: {(settings.surpriseextrachance * 100f):F0}% (The Default is 100%)");
                settings.surpriseextrachance = lst.Slider(settings.surpriseextrachance, 0f, 1f);
            }
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
