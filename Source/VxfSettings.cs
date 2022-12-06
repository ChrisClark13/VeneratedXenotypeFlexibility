using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
    public class VxfSettings : ModSettings
    {
        public float partialMatchThreshold = 0.5f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref partialMatchThreshold, "partialMatchThreshold");
            base.ExposeData();
        }
    }
}