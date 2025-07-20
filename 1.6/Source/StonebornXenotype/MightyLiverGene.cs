using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StonebornXenotype;

public class MightyLiverGene : Gene
{
    public override void PostRemove()
    {
        if (GetHediff() is { } firstHediffOfDef)
            pawn.health.RemoveHediff(firstHediffOfDef);
        base.PostRemove();
    }

    public override void PostAdd()
    {
        base.PostAdd();
        ApplyHediff();
    }

    public override void Tick()
    {
        base.Tick();
        if (!pawn.Spawned || !pawn.IsHashIntervalTick(GenTicks.TickLongInterval) || Rand.Chance(0.75f) || GetHediff() != null) return;
        if (Rand.Chance(0.5f)) ApplyHediff();
    }

    public Hediff GetHediff() => pawn.health.hediffSet.GetFirstHediffOfDef(StonebornDefOf.DV_MightyLiverHediff);

    public void ApplyHediff()
    {
        if (GetHediff() != null || GetLiver() is not { } liver) return;
        Hediff liverHediff = HediffMaker.MakeHediff(StonebornDefOf.DV_MightyLiverHediff, pawn, liver);
        pawn.health.AddHediff(liverHediff);
    }

    public BodyPartRecord GetLiver()
    {
        foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet?.GetNotMissingParts() ?? new List<BodyPartRecord>())
        {
            if (notMissingPart.def.tags.Contains(BodyPartTagDefOf.BloodFiltrationLiver))
                return notMissingPart;
        }

        return null;
    }
}
