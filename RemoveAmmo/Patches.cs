using Il2Cpp;
using HarmonyLib;

namespace RemoveAmmo
{
    [HarmonyPatch(typeof(Il2Cpp.Container), "ShowItemsAfterSearch")]
    public class Container_ShowItemsAfterSearch_Patch
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            Main.ScanContainer(__instance);
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.GunItem), "AddRoundsToClip")]
    public class GunItem_AddRoundsToClip_Patch
    {
        private static void Postfix(Il2Cpp.GunItem __instance)
        {
            Main.ScanGun(__instance);
        }
    }
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Awake))]
    internal class GearItem_Awake_Patch
    {
        private static void Postfix(GearItem __instance)
        {
            Main.DeactivateLooseAmmo(__instance);
        }
    }
    
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.ManualStart))]
    internal class GearItem_ManualStart_Patch
    {
        private static void Postfix(GearItem __instance)
        {
            Main.DeactivateLooseAmmo(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LaunchSandbox))]
    internal class GameManager_LaunchSandbox_Patch
    {
        private static void Prefix()
        {
            Main.SaveSettings();
        }
    }

    [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.DeleteSaveFiles), new Type[] { typeof(string) })]
    internal class ModData_SaveGameSystem_DeleteSaveFiles
    {
        private static void Postfix(string name)
        {
            Main.DeleteSettings(name);
        }
    }
}

