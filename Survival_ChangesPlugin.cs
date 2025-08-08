using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace Survival_Changes
{
    public enum ConfigKey
    {
        PositiveEnergy,
        DefaultEnergyModfication,
        HealthHealingMultiplier,
        TravelEnergyConsumption,
        HandcarEnergyConsumption,
        TravelTimeMultiplier
    }


    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SurvivalChanges_BepInEx: BaseUnityPlugin
    {

        public static Dictionary<string, object> Config;

        public const string MyGUID = "tunguska.natesheltry.survival_changes.modplugin";
        public const string PluginName = "Survival Changes";
        public const string VersionString = "1.0.0.0";

        public static SurvivalChanges_BepInEx instance;
        public static Harmony harmony;
        public static Action<object> LogMessage;

        internal void LoadConfig()
        {
            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "SurvivalChanges.cfg"), true);
            Config = new Dictionary<string, object>();
            Config[$"{ConfigKey.PositiveEnergy}"] = config.Bind(
                "Resting",
                $"{ConfigKey.PositiveEnergy}",
                true,
                "Make resting restore energy by default.\nVanilla: false - Mod: true"
            );
            Config[$"{ConfigKey.DefaultEnergyModfication}"] = config.Bind(
                "Resting",
                $"{ConfigKey.DefaultEnergyModfication}",
                10f,
                "The amount your energy is modified by per hour when resting.\nVanilla: 50 - Mod: 10"
            );
            Config[$"{ConfigKey.HealthHealingMultiplier}"] = config.Bind(
                "Resting",
                $"{ConfigKey.HealthHealingMultiplier}",
                1f,
                "Multiplies how much health you heal when resting."
            );
            Config[$"{ConfigKey.TravelEnergyConsumption}"] = config.Bind(
                "Traveling",
                $"{ConfigKey.TravelEnergyConsumption}",
                100f,
                "The baseline energy cost per hour of travel.\nVanilla: 120 - Mod: 100"
            );
            Config[$"{ConfigKey.HandcarEnergyConsumption}"] = config.Bind(
                "Traveling",
                $"{ConfigKey.HandcarEnergyConsumption}",
                35f,
                "The baseline energy cost per hour of travel when using the Handcar.\nVanilla: 50 - Mod: 35"
            );
            Config[$"{ConfigKey.TravelTimeMultiplier}"] = config.Bind(
                "Traveling",
                $"{ConfigKey.TravelTimeMultiplier}",
                .75f,
                "Multiplier to be applied to travel time when moving between areas.\nVanilla: 1 - Mod: .75"
            );
        }

        public void Awake()
        {
            LoadConfig();
            instance = this;
            LogMessage = this.Logger.LogMessage;

            base.Logger.LogMessage("Survival Changes loaded!");

            SurvivalChanges_BepInEx.harmony = new Harmony(MyGUID);
            SurvivalChanges_BepInEx.harmony.PatchAll();
        }


    }
}
