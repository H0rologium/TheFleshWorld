using RimWorld;
using TheFlesh.CompProperties;
using Verse;
using Verse.Sound;

namespace TheFlesh.Abilities
{
    public class AbilityEffect_OrganTransmute : CompAbilityEffect
    {
        public new CompProperties_OrganTransmute Props
        {
            get
            {
                //Currently no XML properties to return, may add in future.
                return (CompProperties_OrganTransmute)this.props;
            }
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return false;
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return this.Valid(target, false);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Thing thingg = target.Thing;
            Pawn pawnn = target.Pawn;
            if (pawnn == null && thingg == null)
            {
                return false;
            }
            //Only one target can be chosen so we don't need to worry about simultaneous targets being Pawns and Things
            if (target.HasThing && pawnn == null)
            {
                if (!(thingg.Spawned && !thingg.Suspended && thingg.HasThingCategory(ThingCategoryDefOf.MeatRaw) && (thingg.stackCount > 9)))
                {
                    //Log.Error($"Thing did not meet criteria! Spawned: {thingg.Spawned} Suspended: {thingg.Suspended} Category: {thingg.HasThingCategory(ThingCategoryDefOf.MeatRaw)} Stackcount: {(thingg.stackCount > 9)}");
                    if (throwMessages)
                    {
                        Messages.Message("AbilityOrganShapeDeniedThing".Translate(), thingg, MessageTypeDefOf.RejectInput, true);
                    }
                    return false;
                }
                //Skip Pawn if Thing
                return true;
            }

            if (pawnn != null)
            {
                if (!TheFleshTools.isInfectible(pawnn)) return false;
                if ((double)pawnn.BodySize > 2.5)
                {
                    if (throwMessages)
                    {
                        Messages.Message("MessageSlaughterTooBig".Translate(pawnn.Named("PAWN")), pawnn, MessageTypeDefOf.RejectInput, true);
                    }
                    return false;
                }
            }
            
            return true;
        }

        private Thing PickResult()
        {
            string[] choices = [BodyPartDefOf.Heart.defName,BodyPartDefOf.Lung.defName,ThingDefOf.MealNutrientPaste.defName,ThingDefOf.MealFine.defName,ThingDefOf.MealNutrientPaste.defName];
            IntRange orgChoice = new IntRange(0, choices.Length);
            return ThingMaker.MakeThing(ThingDef.Named(choices[orgChoice.RandomInRange]),null);
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Thing thingg = target.Thing;
            Pawn pawn = target.Pawn;
            Map map;
            IntVec3 positionHeld;

            if (target.HasThing && pawn == null)
            {
                map = thingg.MapHeld;
                positionHeld = thingg.PositionHeld;
                base.Apply(target, dest);
                int cnt = thingg.stackCount;
                
                EffecterDefOf.MeatExplosion.Spawn(positionHeld, map, 1f).Cleanup();
                FleshbeastUtility.MeatSplatter(AbilityEffect_OrganTransmute.FilthAmt.RandomInRange, positionHeld, map, FleshbeastUtility.MeatExplosionSize.Large);
                if (thingg.Faction != null && thingg.Faction != Faction.OfPlayer && thingg.Faction.def.humanlikeFaction && !thingg.Faction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony))
                {
                    thingg.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
                }
                thingg.Destroy(DestroyMode.Vanish);
                Thing resultingOrgan = PickResult();
                resultingOrgan.stackCount = 1;
                GenSpawn.Spawn(resultingOrgan, positionHeld, map, WipeMode.Vanish);
                //'Side-effect'
                IntRange bonusChance = new IntRange(0, 4);
                if (bonusChance.RandomInRange >= 3)
                {
                    Pawn fs = PawnGenerator.GeneratePawn(PawnKindDefOf.Fingerspike, Faction.OfHoraxCult, null);
                    GenSpawn.Spawn(fs, CellFinder.StandableCellNear(positionHeld, map, 2f), map);
                    SoundDefOf.FleshmassBirth.PlayOneShot(fs);
                }
                return;
            }

            //Shhh! Pawn interaction is technically a secret interaction!
            if (pawn != null)
            {
                map = pawn.MapHeld;
                positionHeld = pawn.PositionHeld;
                base.Apply(target, dest);
                pawn.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Psychic, 99999f, 0f, -1f, this.parent.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)), null);
                if (pawn.Faction != null && pawn.HomeFaction != null && pawn.HomeFaction != Faction.OfPlayer && pawn.HomeFaction.def.humanlikeFaction && !pawn.Faction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony))
                {
                    pawn.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -100, true, true, HistoryEventDefOf.MemberKilled, null);
                }
                Corpse corpse = pawn.Corpse;
                if (corpse != null)
                {
                    corpse.Destroy(DestroyMode.Vanish);
                }
                EffecterDefOf.MeatExplosion.Spawn(positionHeld, map, 1f).Cleanup();
                FleshbeastUtility.MeatSplatter(AbilityEffect_OrganTransmute.FilthAmt.RandomInRange, positionHeld, map, FleshbeastUtility.MeatExplosionSize.Large);
                Pawn offspring = PawnGenerator.GeneratePawn(InternalDefOf.tfNewborn, Faction.OfHoraxCult, null);
                GenSpawn.Spawn(offspring, CellFinder.StandableCellNear(parent.pawn.Position, parent.pawn.Map, 2f), parent.pawn.Map);
                FilthMaker.TryMakeFilth(parent.pawn.Position, parent.pawn.Map, ThingDefOf.Filth_Blood, count: 30);
                SoundDefOf.FleshmassBirth.PlayOneShot(offspring);
                return;
            }
        }

        private static readonly IntRange FilthAmt = new IntRange(2, 3);

    }
}
