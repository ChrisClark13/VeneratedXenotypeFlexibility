using UnityEngine;
using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
    public class VxfMod : Mod
    {
        public static VxfSettings Settings;

        public VxfMod(ModContentPack contentPack) : base(contentPack)
        {
            Settings = GetSettings<VxfSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            Settings.PartialMatchThreshold = Mathf.Max(
                Mathf.Floor(listingStandard.SliderLabeled(
                    "PartialMatchThresholdDesc".Translate() + $" (>= {Settings.PartialMatchThreshold * 100}%)",
                    Settings.PartialMatchThreshold,
                    0.0001f,
                    1f
                ) * 100f) / 100f,
                0.0001f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "VeneratedXenotypeFlexibility.DisplayName".Translate();
        }
    }
}