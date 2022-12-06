using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
    public class VxfSettings : ModSettings
    {
        public float PartialMatchThreshold = 0.5f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref PartialMatchThreshold, "partialMatchThreshold");
            base.ExposeData();
        }
    }
}