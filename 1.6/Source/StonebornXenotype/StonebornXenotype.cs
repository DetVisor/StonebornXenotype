using HarmonyLib;
using Verse;

namespace StonebornXenotype
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            Harmony harmony = new("det.stonebornxenotype");
            harmony.PatchAll();
        }
    }
}
