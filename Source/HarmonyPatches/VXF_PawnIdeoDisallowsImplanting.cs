using HarmonyLib;
using RimWorld;
using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility.HarmonyPatches
{
    [HarmonyPatch(typeof(Xenogerm), nameof(Xenogerm.PawnIdeoDisallowsImplanting))]
    public static class VXF_PawnIdeoDisallowsImplanting
    {
        public static void Postfix(ref bool __result, Xenogerm __instance, Pawn selPawn)
        {
            if (__result && !selPawn.Ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Vanilla))
            {
                __result = !Util.CanPawnAcceptXenogerm(selPawn, __instance);
            }
        }
    }
}