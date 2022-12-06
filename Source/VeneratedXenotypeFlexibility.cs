using HarmonyLib;
using Verse;

namespace ChrisClark13.VeneratedXenotypeFlexibility
{
  [StaticConstructorOnStartup]
  public static class VeneratedXenotypeFlexibility
  {
    static VeneratedXenotypeFlexibility()
    {
      harmonyInstance = new Harmony("ChrisClark13.VeneratedXenotypeFlexibility");
      harmonyInstance.PatchAll();
    }

    public static Harmony harmonyInstance;
  }
}
