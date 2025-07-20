using HarmonyLib;
using RimWorld;

namespace StonebornXenotype.HarmonyPatches;

[HarmonyPatch(typeof(Thought), nameof(Thought.DurationTicks), MethodType.Getter)]
public static class ThoughtDurationHarmonyPatch
{
    [HarmonyPostfix]
    public static int ThoughtDurationHarmonyPatchPostfix(int __result, Thought __instance)
    {
        return (int)(__result * StonebornMemoryGene.GetThoughtDurationFactorForPawn(__instance.pawn));
    }
}

[HarmonyPatch(typeof(Thought_Memory), nameof(Thought_Memory.DurationTicks), MethodType.Getter)]
public static class ThoughtMemoryDurationHarmonyPatch
{
    [HarmonyPostfix]
    public static void ThoughtMemoryDurationHarmonyPatchPostfix(ref int __result, Thought_Memory __instance)
    {
        __result = (int)(__result * StonebornMemoryGene.GetThoughtDurationFactorForPawn(__instance.pawn));
    }
}
