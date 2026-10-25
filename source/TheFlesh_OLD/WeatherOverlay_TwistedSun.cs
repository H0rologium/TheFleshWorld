using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace TheFlesh
{
    [StaticConstructorOnStartup]
    [Obsolete("Works but didn't look good during testing. Could visit as a later TODO")]
    public class WeatherOverlay_TwistedSun : WeatherOverlay_Fog
    {
        private Material mat;

        public WeatherOverlay_TwistedSun()
        {
            mat = Skymat;
        }

        public override void TickOverlay(Map map, float lerpFactor)
        {
            base.TickOverlay(map, lerpFactor);

            float t = Find.TickManager.TicksGame * 0.08f;

            float pulse =
                0.8f +
                Mathf.Sin(t * 1.7f) * 0.08f +
                Mathf.Sin(t * 3.3f) * 0.04f +
                Mathf.Sin(t * 0.7f) * 0.06f;

            Vector2 driftA = new Vector2(
                Mathf.Sin(t * 0.6f),
                Mathf.Cos(t * 0.43f)
            ) * 0.02f;

            Vector2 driftB = new Vector2(
                Mathf.Cos(t * 0.37f),
                Mathf.Sin(t * 0.51f)
            ) * 0.02f;

            SetOverlayColor(new Color(0.65f,0.25f,0.22f,0.65f));
            worldOverlayPanSpeed1 = 0.15f + driftA.magnitude;
            worldOverlayPanSpeed2 = 0.12f + driftB.magnitude;

            worldPanDir1 = (Vector2.right + driftA).normalized;
            worldPanDir2 = (Vector2.left + driftB).normalized;
        }


        private static readonly Material Skymat = TheFleshTools.skyfleshmat;
    }
}
