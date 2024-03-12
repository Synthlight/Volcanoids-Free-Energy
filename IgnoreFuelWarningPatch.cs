using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;

namespace Free_Energy;

// Eliminate the low coal threshold.
[HarmonyPatch]
[UsedImplicitly]
public static class IgnoreFuelWarningPatch1 {
    [HarmonyTargetMethod]
    [UsedImplicitly]
    public static MethodBase TargetMethod() {
        return typeof(E04_Energy).GetMethod(nameof(E04_Energy.UpdateProgress), BindingFlags.Public | BindingFlags.Instance);
    }

    [HarmonyPrefix]
    [UsedImplicitly]
    public static bool Prefix(ref float ___m_lowFuelCount) {
        ___m_lowFuelCount = -100;
        return true;
    }
}

// Eliminate the no coal check.
[HarmonyPatch]
[UsedImplicitly]
public static class IgnoreFuelWarningPatch2 {
    private static readonly GUID COAL = GUID.Parse("e89c0cdbf40a07f4ebb20d2865717a52");

    [HarmonyTargetMethod]
    [UsedImplicitly]
    public static MethodBase TargetMethod() {
        return typeof(ItemHelper).GetMethod(nameof(ItemHelper.HasAnyItem), BindingFlags.Public | BindingFlags.Static);
    }

    [HarmonyPrefix]
    [UsedImplicitly]
    public static bool Prefix(ref List<ItemDefinition> items, ref bool __result) {
        if (items?.Count == 1 && items[0].AssetId == COAL) {
            __result = true;
            return false;
        }
        return true;
    }
}