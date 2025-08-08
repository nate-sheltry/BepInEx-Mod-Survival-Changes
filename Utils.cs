using BepInEx.Configuration;

namespace Survival_Changes
{
    public static class Utils
    {
        public static object GetConfigKey(ConfigKey key)
        {
            if (!SurvivalChanges_BepInEx.Config.TryGetValue(key.ToString(), out var entryObj) || entryObj == null)
                return null;

            if (entryObj is ConfigEntry<bool> boolEntry)
                return boolEntry.Value;

            if (entryObj is ConfigEntry<int> intEntry)
                return intEntry.Value;

            if (entryObj is ConfigEntry<float> floatEntry)
                return floatEntry.Value;

            if (entryObj is ConfigEntry<string> stringEntry)
                return stringEntry.Value;

            return null;
        }

        public static bool GetConfigKeyCondition(ConfigKey key)
        {
            if (!SurvivalChanges_BepInEx.Config.TryGetValue(key.ToString(), out var entryObj) || entryObj == null)
                return false;

            if (entryObj is ConfigEntry<bool> boolEntry)
                return boolEntry.Value;

            return false;
        }
    }
}
