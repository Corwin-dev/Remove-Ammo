﻿using ModSettings;

namespace RemoveAmmo
{
    internal class RemoveAmmoSettings : ModSettingsBase
    {
        [Name("Remove Rifle Ammunition")]
        [Description("Removes uncrafted rifle ammunition from the world")]
        public bool removeRifleAmmo = false;

        [Name("Remove Revolver Ammunition")]
        [Description("Removes uncrafted revolver ammunition from the world")]
        public bool removeRevolverAmmo = false;
    }
    internal static class Settings
    {
        internal static RemoveAmmoSettings options = new();

        public static void OnLoad()
        {
            options = new RemoveAmmoSettings();
            options.AddToCustomModeMenu(Position.BelowGear);
        }
    }
}