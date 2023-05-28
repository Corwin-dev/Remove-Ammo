using MelonLoader;
using UnityEngine;
using HarmonyLib;
using Il2Cpp;

namespace ModNamespace
{
    public class RemoveAmmo : MelonMod
    {
        public static readonly List<string> gunNames = new()
        {
            "GEAR_Rifle",
            "GEAR_Rifle_Barbs",
            "GEAR_Rifle_Curators",
            "GEAR_Rifle_Vaughns",
            "GEAR_Revolver"
        };

        public static readonly List<string> ammoNames = new()
        {
            "GEAR_RifleAmmoSingle",
            "GEAR_RevolverAmmoSingle",
            "GEAR_RifleAmmoBox",
            "GEAR_RevolverAmmoBox"
        };

        public static readonly List<string> protectedParents = new()
        {
            "Gear",
            "DesignGear"
        };

        public override void OnInitializeMelon()
        {
            var harmony = new HarmonyLib.Harmony("com.delete.ammo");
            harmony.PatchAll();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName.EndsWith("_SANDBOX"))
            {
                GameObject[] activeObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject obj in activeObjects)
                {
                    if (ammoNames.Contains(obj.name))
                    {
                        GameObject parent = obj.transform.parent.gameObject;
                        if (parent == null || (!protectedParents.Contains(parent.name) && !sceneName.StartsWith(parent.name)))
                        {
                            obj.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.Container), "ShowItemsAfterSearch")]
    public class Container_ShowItemsAfterSearch_Patch
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            Il2CppSystem.Collections.Generic.List<Il2Cpp.GearItem> allItems = new();

            __instance.GetItems(allItems);
            foreach (Il2Cpp.GearItem gearItem in allItems)
            {
                if (RemoveAmmo.ammoNames.Contains(gearItem.name))
                {
                    __instance.DestroyGear(gearItem);
                }
                else if (RemoveAmmo.gunNames.Contains(gearItem.name))
                {
                    var gunItem = gearItem.transform.parent?.GetComponent<GunItem>();
                    gunItem?.EmptyClip();
                }
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.GunItem), "AddRoundsToClip")]
    public class GunItem_AddRoundsToClip_Patch
    {
        private static void Postfix(Il2Cpp.GunItem __instance)
        {
            GameObject obj = __instance.transform.parent.gameObject;
            string sceneName = obj.scene.name;
            GameObject parent = obj.transform.parent.gameObject;

            if (sceneName != null && (sceneName.EndsWith("_SANDBOX") || (!RemoveAmmo.protectedParents.Contains(parent.name) && !sceneName.StartsWith(parent.name))))
            {
                __instance.EmptyClip();
            }
        }
    }
}