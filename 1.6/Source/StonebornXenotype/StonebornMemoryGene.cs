using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace StonebornXenotype;

public class StonebornMemoryGene : Gene
{
    public static ConcurrentDictionary<int, float> SkillLossFactors = new();
    public static ConcurrentDictionary<int, float> ThoughtDurationFactors = new();


    public StonebornMemoryModExtension modExtension;

    public float GetSkillLossFactor() => modExtension?.skillLossFactor ?? 1f;
    public float ThoughtDurationFactor() => modExtension?.thoughtDurationFactor ?? 1f;

    public static float GetSkillLossFactorForPawn(Pawn pawn) => pawn != null ? SkillLossFactors.TryGetValue(pawn.thingIDNumber, 1f) : 1f;

    public static float GetThoughtDurationFactorForPawn(Pawn pawn) => pawn != null ? ThoughtDurationFactors.TryGetValue(pawn.thingIDNumber, 1f) : 1f;

    public override void ExposeData()
    {
        base.ExposeData();
        if (modExtension != null || (Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit)) return;
        modExtension = def.GetModExtension<StonebornMemoryModExtension>();
        if (modExtension == null) return;
        Init();
    }

    public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
    {
        base.Notify_PawnDied(dinfo, culprit);
        SkillLossFactors.TryRemove(pawn.thingIDNumber, out _);
        ThoughtDurationFactors.TryRemove(pawn.thingIDNumber, out _);
    }


    public override void PostRemove()
    {
        base.PostRemove();
        if (SkillLossFactors.ContainsKey(pawn.thingIDNumber) && !Mathf.Approximately(GetSkillLossFactor(), SkillLossFactors.TryGetValue(pawn.thingIDNumber, GetSkillLossFactor())))
        {
            SkillLossFactors.AddOrUpdate(pawn.thingIDNumber, GetSkillLossFactor(), (_, oldValue) => oldValue / GetSkillLossFactor());
        }
        else
        {
            SkillLossFactors.TryRemove(pawn.thingIDNumber, out _);
        }

        if (ThoughtDurationFactors.ContainsKey(pawn.thingIDNumber) &&
            !Mathf.Approximately(ThoughtDurationFactor(), ThoughtDurationFactors.TryGetValue(pawn.thingIDNumber, ThoughtDurationFactor())))
        {
            ThoughtDurationFactors.AddOrUpdate(pawn.thingIDNumber, ThoughtDurationFactor(), (_, oldValue) => oldValue / ThoughtDurationFactor());
        }
        else
        {
            ThoughtDurationFactors.TryRemove(pawn.thingIDNumber, out _);
        }
    }

    public override void PostAdd()
    {
        base.PostAdd();
        modExtension = def.GetModExtension<StonebornMemoryModExtension>();
        Init();
    }

    public void Init()
    {
        SkillLossFactors.AddOrUpdate(pawn.thingIDNumber, GetSkillLossFactor(), (_, _) => GetSkillLossFactor());
        ThoughtDurationFactors.AddOrUpdate(pawn.thingIDNumber, ThoughtDurationFactor(), (_, _) => ThoughtDurationFactor());
    }
}
