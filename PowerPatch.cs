using System;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace Free_Energy {
    [HarmonyPatch]
    [UsedImplicitly]
    public static class PowerPatch {
        private static readonly MethodInfo POWER_PLANT_GET_ENERGY = typeof(PowerPlant).GetMethod("GetEnergy", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod() {
            return typeof(PowerPlant).GetMethod("LoadFuel", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Prefix(ref PowerPlant __instance, ref bool __result, ref float ___m_nextFuelCheck) {
            if (Time.time < ___m_nextFuelCheck) {
                __result = false;
                return false;
            }
            foreach (var itemDefinition in __instance.Fuel) {
                if (itemDefinition != null) {
                    var num    = Math.Max(__instance.FuelItemPreloadCount, 1);
                    var energy = (float) POWER_PLANT_GET_ENERGY.Invoke(__instance, new object[] { itemDefinition });
                    __instance.StoredFuelEnergy += energy * __instance.FuelEfficiency * num;
                    __result                    =  true;
                    return false;
                }
            }
            ___m_nextFuelCheck = Time.time + UnityEngine.Random.Range(1.3f, 1.5f);
            __result           = false;
            return false;
        }
    }
}