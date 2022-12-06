using RimWorld;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
    [DefOf]
    public static class UtilDefOf
    {
        public static PreceptDef VXF_XenotypeStrictness_Partial;
        public static PreceptDef VXF_XenotypeStrictness_Full;
        public static PreceptDef VXF_XenotypeStrictness_Exact;
        public static PreceptDef VXF_XenotypeStrictness_Vanilla;

        static UtilDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof (UtilDefOf));
    }
}