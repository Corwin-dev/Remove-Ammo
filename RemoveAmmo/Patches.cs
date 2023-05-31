using Il2Cpp;
using HarmonyLib;

namespace RemoveAmmo
{
    [HarmonyPatch(typeof(Il2Cpp.Container), "ShowItemsAfterSearch")]
    public class Container_ShowItemsAfterSearch_Patch
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            RemoveAmmo.ScanContainer(__instance);
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.GunItem), "AddRoundsToClip")]
    public class GunItem_AddRoundsToClip_Patch
    {
        private static void Postfix(Il2Cpp.GunItem __instance)
        {
            RemoveAmmo.ScanGun(__instance);
        }
    }
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Awake))]
    internal class GearItem_Awake_Patch
    {
        private static void Postfix(GearItem __instance)
        {
            RemoveAmmo.DeactivateLooseAmmo(__instance);
        }
    }
    
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.ManualStart))]
    internal class GearItem_ManualStart_Patch
    {
        private static void Postfix(GearItem __instance)
        {
            RemoveAmmo.DeactivateLooseAmmo(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LaunchSandbox))]
    internal class GameManager_LaunchSandbox_Patch
    {
        private static void Prefix()
        {
            RemoveAmmo.SaveSettings();
        }
        private static void Postfix()
        {
            RemoveAmmo.ClearSettings();
        }
    }

    [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.DeleteSaveFiles), new Type[] { typeof(string) })]
    internal class ModData_SaveGameSystem_DeleteSaveFiles
    {
        private static void Postfix(string name)
        {
            RemoveAmmo.DeleteSettings(name);
        }
    }
}

