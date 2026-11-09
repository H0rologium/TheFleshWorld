using RimWorld;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TheFlesh
{
    public class Building_HemogenExtractor : Building
    {
        public CompFacility FacilityComp
        {
            get
            {
                CompFacility compFacility;
                if ((compFacility = this.facilityComp) == null)
                {
                    compFacility = (this.facilityComp = base.GetComp<CompFacility>());
                }
                return compFacility;
            }
        }
        public CompPowerTrader Power
        {
            get
            {
                CompPowerTrader compPowerTrader;
                if ((compPowerTrader = this.powerComp) == null)
                {
                    compPowerTrader = (this.powerComp = base.GetComp<CompPowerTrader>());
                }
                return compPowerTrader;
            }
        }
        public List<Thing> Platforms
        {
            get
            {
                return this.FacilityComp.LinkedBuildings;
            }
        }
        public bool ReadyForHauling
        {
            get
            {
                return Mathf.FloorToInt(this.storedHemogenPacks) >= HaulingThreshold;
            }
        }
        private float HemogenPerDay
        {
            get
            {
                if (!this.Power.PowerOn)
                {
                    return 0f;
                }
                float num = 0f;
                foreach (Thing thing in this.Platforms)
                {
                    Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
                    if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
                    {
                        num += CompProducesBioferrite.BioferritePerDay(building_HoldingPlatform.HeldPawn);
                    }
                }
                return num;
            }
        }
        public CompCableConnection CableConnection
        {
            get
            {
                CompCableConnection compCableConnection;
                if ((compCableConnection = this.cableConnection) == null)
                {
                    compCableConnection = (this.cableConnection = base.GetComp<CompCableConnection>());
                }
                return compCableConnection;
            }
        }
        private void Initialize()
        {
            if (this.initalized)
            {
                return;
            }
            this.initalized = true;
            this.FacilityComp.OnLinkAdded += this.OnLinkAdded;
            this.FacilityComp.OnLinkRemoved += this.OnLinkRemoved;
            foreach (Thing thing in this.Platforms)
            {
                Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
                if (building_HoldingPlatform != null)
                {
                    building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
                }
            }
            this.RebuildCables();
        }
        private static readonly Texture2D toggleUnloadIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/ToggleHemogenUnloadGizmo");
        private static readonly Texture2D ejectHemogenIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/EjectHemogenGizmo");

        #region events
        private void OnLinkRemoved(CompFacility facility, Thing thing)
        {
            Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
            if (building_HoldingPlatform == null)
            {
                return;
            }
            building_HoldingPlatform.innerContainer.OnContentsChanged -= this.RebuildCables;
            this.RebuildCables();
        }

        private void OnLinkAdded(CompFacility facility, Thing thing)
        {
            Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
            if (building_HoldingPlatform == null)
            {
                return;
            }
            building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
            this.RebuildCables();
        }
        #endregion

        #region overrides
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                this.Initialize();
            }
        }
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            this.FacilityComp.OnLinkAdded -= this.OnLinkAdded;
            this.FacilityComp.OnLinkRemoved -= this.OnLinkRemoved;
            this.initalized = false;
            Sustainer sustainer = this.workingSustainer;
            if (sustainer != null)
            {
                sustainer.End();
            }
            this.workingSustainer = null;
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            if (!this.initalized)
            {
                this.Initialize();
            }
        }

        protected override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(250))
            {
                this.storedHemogenPacks = Mathf.Min(this.storedHemogenPacks + this.HemogenPerDay / 180000f * 250f, MaxCapacity); //60000 == 1 ingame day
            }
            if (this.IsWorking())
            {
                if (this.workingSustainer == null)
                {
                    this.workingSustainer = SoundDefOf.BioferriteHarvester_Ambient.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.None));
                }
                this.workingSustainer.Maintain();
                return;
            }
            Sustainer sustainer = this.workingSustainer;
            if (sustainer != null)
            {
                sustainer.End();
            }
            this.workingSustainer = null;
        }

        public override bool IsWorking()
        {
            return this.HemogenPerDay != 0f;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }
            stringBuilder.Append("TFHemoGenExtractLOne".Translate());
            StringBuilder stringBuilder2 = stringBuilder;
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
            defaultInterpolatedStringHandler.AppendLiteral(" ");
            defaultInterpolatedStringHandler.AppendFormatted<float>(this.storedHemogenPacks, "F2");
            defaultInterpolatedStringHandler.AppendLiteral(" / ");
            defaultInterpolatedStringHandler.AppendFormatted(MaxCapacity.ToString());
            defaultInterpolatedStringHandler.AppendLiteral(" (+");
            defaultInterpolatedStringHandler.AppendFormatted<float>(this.HemogenPerDay, "F2");
            defaultInterpolatedStringHandler.AppendLiteral(" ");
            stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
            StringBuilder stringBuilder3 = stringBuilder;
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
            defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TFHemogenHarvestedPerDay".Translate());
            defaultInterpolatedStringHandler2.AppendLiteral(")");
            stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
            return stringBuilder.ToString();
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            yield return new Command_Toggle
            {
                defaultLabel = "TFHGEUnloadToggle".Translate(),
                defaultDesc = "TFHGEUnloadDesc".Translate(),
                toggleAction = () => unloadingEnabled = !unloadingEnabled,
                isActive = () => unloadingEnabled,
                icon = toggleUnloadIcon
            };

            if (storedHemogenPacks >= 1f)
            {
                yield return new Command_Action
                {
                    action = new Action(EjectContents),
                    defaultLabel = "TFHGEEjectHemogenLabel".Translate(),
                    defaultDesc = "TFHGEEjectHemogenDesc".Translate(),
                    icon = ejectHemogenIcon
                };
            }
            
        }


        public override void Notify_DefsHotReloaded()
        {
            base.Notify_DefsHotReloaded();
            this.RebuildCables();
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.storedHemogenPacks, "storedHemogenPacks", 0f, false);
            Scribe_Values.Look<bool>(ref this.unloadingEnabled, "unloadingEnabled", false, false);
        }
        #endregion

        public Thing TakeOutHemogen()
        {
            int num = Mathf.FloorToInt(this.storedHemogenPacks);
            if (num == 0)
            {
                return null;
            }
            this.storedHemogenPacks -= (float)num;
            Thing thing = ThingMaker.MakeThing(ThingDefOf.HemogenPack, null);
            thing.stackCount = num;
            return thing;
        }

        private void EjectContents()
        {
            Thing thing = this.TakeOutHemogen();
            if (thing == null)
            {
                return;
            }
            GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
        }

        private void RebuildCables()
        {
            this.CableConnection.RebuildCables(this.Platforms, delegate (Thing thing)
            {
                Building_HoldingPlatform building_hgE = thing as Building_HoldingPlatform;
                return building_hgE != null && building_hgE.Occupied;
            });
        }


        public float storedHemogenPacks;
        public bool unloadingEnabled = true;
        private bool initalized;
        private const int HaulingThreshold = 7;
        private const float MaxCapacity = 10f;
        private CompFacility facilityComp;
        private CompPowerTrader powerComp;
        private CompCableConnection cableConnection;
        private Sustainer workingSustainer;
    }
}
