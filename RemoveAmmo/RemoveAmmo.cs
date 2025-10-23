using MelonLoader;
using UnityEngine;
using Il2Cpp;

namespace RemoveAmmo
{
    internal class Main : MelonMod
    {
        internal static readonly List<string> rifleNames = new()
        {
            "GEAR_Rifle",
            "GEAR_Rifle_Barbs",
            "GEAR_Rifle_Curators",
            "GEAR_Rifle_Vaughns"
        };

        internal static readonly List<string> revolverNames = new()
        {
            "GEAR_Revolver"
        };

        internal static readonly List<string> rifleAmmoNames = new()
        {
            "GEAR_RifleAmmoSingle",
            "GEAR_RifleAmmoBox"
        };

        internal static readonly List<string> revolverAmmoNames = new()
        {
            "GEAR_RevolverAmmoSingle",
            "GEAR_RevolverAmmoBox"
        };

        internal static readonly List<string> protectedParents = new()
        {
            "Gear",
            "DesignGear"
        };

        internal static readonly List<string> protectedScenes = new List<string>()
        {
            "DontDestroyOnLoad",
            "HideAndDontSave"
        };

        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
            var harmony = new HarmonyLib.Harmony("DeleteAmmo");
            harmony.PatchAll();
        }

        internal static void DeactivateLooseAmmo(GearItem gi)
        {
            if ((DoRemoveRifleAmmo() && rifleAmmoNames.Contains(gi.name)) 
            || (DoRemoveRevolverAmmo() && revolverAmmoNames.Contains(gi.name)))
            {
                GameObject obj = gi.gameObject;
                if (obj.scene.name.Contains('_') || !Exempt(obj)) obj.SetActive(false);
            }
        }
        internal static void ScanContainer(Il2Cpp.Container container)
        {
            Il2CppSystem.Collections.Generic.List<Il2Cpp.GearItem> allItems = new();
            container.GetItems(allItems);

            foreach (Il2Cpp.GearItem gi in allItems)
            {
                if ((DoRemoveRifleAmmo() && rifleAmmoNames.Contains(gi.name)) 
                || (DoRemoveRevolverAmmo() && revolverAmmoNames.Contains(gi.name)))
                {
                    container.DestroyGear(gi);
                }
            }
        }
        internal static void ScanGun(Il2Cpp.GunItem gun)
        {
            if (gun.NumRoundsInClip() == 0) return;
            GameObject obj = gun.gameObject;
            if (protectedScenes.Contains(obj.scene.name)) return;
            if (Exempt(obj)) return;
            if ((DoRemoveRifleAmmo() && rifleNames.Contains(gun.name)) 
            || (DoRemoveRevolverAmmo() && revolverNames.Contains(gun.name)))
            {
                gun.EmptyClip();
            }
        }
        internal static bool Exempt(GameObject obj)
        {
            GameObject parent = obj.transform.parent.gameObject;
            if (protectedParents.Contains(parent.name)) return true;
            if (protectedScenes.Contains(obj.scene.name)) return true;
            if (parent.name == obj.scene.name) return true;
            return false;
        }
        internal static bool DoRemoveRifleAmmo()
        {
            return DoRemoveAmmo("RemoveRifleAmmo_");
        }
        internal static bool DoRemoveRevolverAmmo()
        {
            return DoRemoveAmmo("RemoveRevolverAmmo_");
        }
        internal static bool DoRemoveAmmo(string prefix)
        {
            if (ExperienceModeManager.GetCurrentExperienceModeType() != ExperienceModeType.Custom) return false;
            string saveKey = prefix + SaveGameSystem.GetCurrentSaveName();
            if (!PlayerPrefs.HasKey(saveKey)) return false;
            bool option = Convert.ToBoolean(PlayerPrefs.GetString(saveKey));
            return option;
        }
        internal static void SaveSettings()
        {
            string saveName = SaveGameSystem.GetCurrentSaveName();
            string removeRifleAmmo = Settings.options.removeRifleAmmo.ToString();
            string removeRevolverAmmo = Settings.options.removeRevolverAmmo.ToString();
            PlayerPrefs.SetString("RemoveRifleAmmo_" + saveName, removeRifleAmmo);
            PlayerPrefs.SetString("RemoveRevolverAmmo_" + saveName, removeRevolverAmmo);
            PlayerPrefs.Save();
        }

        internal static void DeleteSettings(string saveName)
        {
            if (saveName == null) return;
            if (saveName == SaveGameSlots.AUTOSAVE_SLOT_NAME) return;
            if (saveName == SaveGameSlots.QUICKSAVE_SLOT_PREFIX) return;
            PlayerPrefs.DeleteKey("RemoveRifleAmmo_" + saveName);
            PlayerPrefs.DeleteKey("RemoveRevolverAmmo_" + saveName);
        }
    }
}