using System.Collections.Generic;
using RimWorld;
using Verse;
using System.Linq;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
    public static class Util
    {
        public static bool PreferredXenotypeCheck(Ideo ideo, Pawn pawn)
        {
            if (!ideo.PreferredXenotypes.Any() && !ideo.PreferredCustomXenotypes.Any() || pawn.genes == null)
                return false;

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Partial))
            {
                var pawnGeneDefs = GetGeneDefSet(pawn);
                foreach (var xt in ideo.PreferredXenotypes)
                {
                    var passedOnGenes = xt.genes.Where(g => g.passOnDirectly).ToHashSet();
                    if (passedOnGenes.Intersect(pawnGeneDefs).Count() / (float)passedOnGenes.Count >=
                        VxfMod.Settings.PartialMatchThreshold)
                        return true;
                }

                foreach (var cx in ideo.PreferredCustomXenotypes)
                {
                    var passedOnGenes = cx.genes.Where(g => g.passOnDirectly).ToHashSet();
                    if (passedOnGenes.Intersect(pawnGeneDefs).Count() / (float)passedOnGenes.Count >=
                        VxfMod.Settings.PartialMatchThreshold)
                        return true;
                }

                return false;
            }

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Full))
            {
                var pawnGeneDefs = GetGeneDefSet(pawn);
                return ideo.PreferredXenotypes.Any(xt =>
                           xt.genes.TrueForAll(g => !g.passOnDirectly || pawnGeneDefs.Contains(g))) ||
                       ideo.PreferredCustomXenotypes.Any(cx =>
                           cx.genes.TrueForAll(g => !g.passOnDirectly || pawnGeneDefs.Contains(g)));
            }

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Exact))
            {
                return pawn.genes.CustomXenotype == null
                    ? ideo.PreferredXenotypes.Contains(pawn.genes.Xenotype) || ideo.PreferredXenotypes.Any(xt =>
                    {
                        if (xt.inheritable)
                        {
                            return pawn.genes.Endogenes.TrueForAll(eg =>
                                       !eg.def.passOnDirectly || xt.genes.Contains(eg.def)) &&
                                   !pawn.genes.Xenogenes.Any(xg => !xg.def.passOnDirectly);
                        }

                        return pawn.genes.Xenogenes.TrueForAll(
                            xg => !xg.def.passOnDirectly || xt.genes.Contains(xg.def));
                    })
                    : ideo.PreferredCustomXenotypes.Any(cx => GeneUtility.PawnIsCustomXenotype(pawn, cx));
            }

            // if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Vanilla))
            // {
            //     return ideo.IsPreferredXenotype(pawn);
            // }

            return false;
        }

        public static HashSet<GeneDef> GetGeneDefSet(Pawn pawn, bool excludeNotPassedOnDirectly = true)
        {
            var geneDefs = pawn.genes.GenesListForReading.Select(g => g.def);
            return (excludeNotPassedOnDirectly ? geneDefs.Where(g => g.passOnDirectly) : geneDefs).ToHashSet();
        }

        public static bool CanPawnAcceptXenogerm(Pawn pawn, Xenogerm xenogerm)
        {
            var ideo = pawn.Ideo;
            if (!ideo.PreferredXenotypes.Any() && !ideo.PreferredCustomXenotypes.Any() || pawn.genes == null)
                return true;

            var xenogermGenes = xenogerm.GeneSet.GenesListForReading.Where(g => g.passOnDirectly).ToHashSet();
            var resultingGenes = xenogermGenes
                .Concat(pawn.genes.Endogenes.Select(g => g.def).Where(gd => gd.passOnDirectly))
                .ToHashSet();

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Partial))
            {
                foreach (var xt in ideo.PreferredXenotypes)
                {
                    var passedOnGenes = xt.genes.Where(g => g.passOnDirectly).ToHashSet();
                    if (passedOnGenes.Intersect(resultingGenes).Count() / (float)passedOnGenes.Count >=
                        VxfMod.Settings.PartialMatchThreshold)
                        return true;
                }

                foreach (var cx in ideo.PreferredCustomXenotypes)
                {
                    var passedOnGenes = cx.genes.Where(g => g.passOnDirectly).ToHashSet();
                    if (passedOnGenes.Intersect(resultingGenes).Count() / (float)passedOnGenes.Count >=
                        VxfMod.Settings.PartialMatchThreshold)
                        return true;
                }

                return false;
            }

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Full))
            {
                return ideo.PreferredXenotypes.Any(xt =>
                           xt.genes.TrueForAll(g => !g.passOnDirectly || resultingGenes.Contains(g))) ||
                       ideo.PreferredCustomXenotypes.Any(cx =>
                           cx.genes.TrueForAll(g => !g.passOnDirectly || resultingGenes.Contains(g)));
            }

            if (ideo.HasPrecept(UtilDefOf.VXF_XenotypeStrictness_Exact))
            {
                return ideo.PreferredXenotypes.Any(xt =>
                {
                    var relevantGenes = xt.inheritable ? resultingGenes : xenogermGenes;
                    var desiredGenes = xt.genes.Where(g => g.passOnDirectly).ToHashSet();
                    return desiredGenes.All(g => relevantGenes.Contains(g)) &&
                           relevantGenes.All(g => xt.genes.Contains(g));
                }) || ideo.PreferredCustomXenotypes.Any(cx =>
                {
                    var relevantGenes = cx.inheritable ? resultingGenes : xenogermGenes;
                    var desiredGenes = cx.genes.Where(g => g.passOnDirectly).ToHashSet();
                    return desiredGenes.All(g => relevantGenes.Contains(g)) &&
                           relevantGenes.All(g => cx.genes.Contains(g));
                });
            }

            return false;
        }
    }
}