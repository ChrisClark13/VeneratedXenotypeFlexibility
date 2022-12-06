using HarmonyLib;
using RimWorld;
using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility.HarmonyPatches
{
    [HarmonyPatch(typeof(Ideo), nameof(Ideo.IsPreferredXenotype))]
    public static class VXF_IsPreferredXenotype
    {
        static bool Postfix(bool result, Ideo __instance, Pawn pawn)
        {
            if (__instance.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Vanilla))
                return result;
            
            return result || Util.PreferredXenotypeCheck(__instance, pawn);
        }
    }
}
