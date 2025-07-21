using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace StonebornXenotype.HarmonyPatches;

[HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Interval))]
public static class SkillLossHarmonyPatch
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> SkillLossHarmonyPatchTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        bool patched = false;
        foreach (CodeInstruction instruction in instructions)
        {
            yield return instruction;
            if (instruction.opcode == OpCodes.Stloc_0 && !patched)
            {
                patched = true;
                yield return new CodeInstruction(OpCodes.Ldloc_0);
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return CodeInstruction.Call(typeof(SkillLossHarmonyPatch), nameof(GetSkillLossFactor));
                yield return new CodeInstruction(OpCodes.Mul);
                yield return new CodeInstruction(OpCodes.Stloc_0);
            }
        }
        if (!patched)
        {
            Verse.Log.Error("Stoneborn failed to patch the Skill Loss for Memory Gene.");
        }
    }

    public static float GetSkillLossFactor(SkillRecord skill) => StonebornMemoryGene.GetSkillLossFactorForPawn(skill.Pawn);
}
