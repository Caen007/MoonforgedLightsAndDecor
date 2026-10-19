using HarmonyLib;
using UnityEngine;

namespace Moonforged.LightsAndDecor
{
    [HarmonyPatch(typeof(Bed), "Interact")]
    internal static class VikingBedInteractionPatch
    {
        private const float MaximumDistanceFromCollider = 0.5f;

        private static bool Prefix(Bed __instance, Humanoid human, ref bool __result)
        {
            if (__instance == null ||
                !__instance.gameObject.name.StartsWith("M_VikingBed(") &&
                __instance.gameObject.name != "M_VikingBed")
                return true;

            if (human == null)
                return true;

            Vector3 position = human.transform.position;
            float maximumSquaredDistance = MaximumDistanceFromCollider * MaximumDistanceFromCollider;
            bool foundPhysicalCollider = false;

            foreach (BoxCollider collider in __instance.GetComponentsInChildren<BoxCollider>())
            {
                if (collider == null || !collider.enabled || collider.isTrigger)
                    continue;

                foundPhysicalCollider = true;
                Vector3 nearest = collider.ClosestPoint(position);
                if ((nearest - position).sqrMagnitude <= maximumSquaredDistance)
                    return true;
            }

            if (!foundPhysicalCollider)
                return true;

            __result = false;
            return false;
        }
    }
}
