using RimWorld;
using Verse;

namespace StonebornXenotype;

[DefOf]
public static class StonebornDefOf
{
    public static HediffDef DV_MightyLiver;

    static StonebornDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(StonebornDefOf));
}
