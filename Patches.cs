using HarmonyLib;
using UnityEngine;

namespace Survival_Changes
{
    [HarmonyPatch(typeof(PlayerSurvival), "GetHealthEnergyDelta", MethodType.Normal)]
    internal static class Patch_1
    {
        [HarmonyPrefix]
        private static bool Prefix(
            int hours,
            ref float healthD,
            ref float energyD,
            PlayerSurvival __instance
        )
        {
            healthD = 0f;
            energyD = 0f;
            CharacterStatus myStatus = GameManager.Inst.PlayerControl.SelectedPC.MyStatus;
            bool flag = false;
            Vector3 position = GameManager.Inst.PlayerControl.SelectedPC.transform.position;
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Campfire"))
            {
                Campfire component = gameObject.GetComponent<Campfire>();
                if (Vector3.Distance(position, gameObject.transform.position) < 3f && component != null && component.IsOn)
                    flag = true;
            }
            if (__instance.GetEatenCalories() <= 0f)
            {
                float num = 0f;
                if (!flag)
                {
                    if(Utils.GetConfigKey(ConfigKey.DefaultEnergyModfication) is float energyMod)
                        num = energyMod * (float)hours;
                    else
                        num = 50f * (float)hours;
                    if (Utils.GetConfigKey(ConfigKey.PositiveEnergy) is bool condition && condition)
                        energyD += num * (1/GameManager.Inst.Constants.HCEnergySpendMultiplier);
                    else
                        energyD -= num * GameManager.Inst.Constants.HCEnergySpendMultiplier;
                }
                else if (Utils.GetConfigKey(ConfigKey.PositiveEnergy) is bool condition && condition)
                {
                    if (Utils.GetConfigKey(ConfigKey.DefaultEnergyModfication) is float energyMod)
                        num = energyMod * (float)hours;
                    else
                        num = 50f * (float)hours;
                    energyD += num * (1/GameManager.Inst.Constants.HCEnergySpendMultiplier);
                }
            }
            else if (myStatus.Health / (myStatus.MaxHealth * 1f) < 1f)
            {
                if(Utils.GetConfigKey(ConfigKey.HealthHealingMultiplier) is float healingMod)
                    healthD = 15f * healingMod * (float)hours;
                else
                    healthD = 15f * (float)hours;
                if (myStatus.Health + healthD > myStatus.MaxHealth * 1f)
                    healthD = myStatus.MaxHealth * 1f - myStatus.Health;
            }
            float num2 = __instance.GetEatenCalories();
            if (flag)
                num2 *= 2f;
            energyD += num2;
            //SurvivalChanges_BepInEx.LogMessage($"Energy Delta: {energyD}");
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerSurvival), "CompleteResting", MethodType.Normal)]
    internal static class Patch_2
    {
        [HarmonyPrefix]
        private static bool Prefix(
        int hours,
        PlayerSurvival __instance
        )
        {
            if (GameManager.Inst.PlayerControl.SelectedPC.ActionState == HumanActionStates.Resting)
                GameManager.Inst.PlayerControl.OnToggleResting();
            CharacterStatus myStatus = GameManager.Inst.PlayerControl.SelectedPC.MyStatus;
            __instance.CompleteAllRestores();
            __instance.ClearAllStatBoosts(hours);
            bool flag = false;
            Vector3 position = GameManager.Inst.PlayerControl.SelectedPC.transform.position;
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Campfire"))
            {
                Campfire component = gameObject.GetComponent<Campfire>();
                if (Vector3.Distance(position, gameObject.transform.position) < 3f && component != null && component.IsOn)
                    flag = true;
            }
            bool flag2 = false;
            float num2;
            float num3;
            __instance.GetHealthEnergyDelta(hours, out num2, out num3);
            if (myStatus.Health < myStatus.MaxHealth * 1f)
            {
                myStatus.Health = Mathf.Clamp(myStatus.Health + num2, 0f, myStatus.MaxHealth * 1f);
                flag2 = true;
            }
            //SurvivalChanges_BepInEx.LogMessage($"Energy Restored: {num3}");
            myStatus.SetEnergy(myStatus.Energy + num3);
            __instance.SetEatenCalories(0f);
            if (!flag2)
                GameManager.Inst.UIManager.SetConsoleText(GameManager.Inst.UIManager.GetTranslatedMessage("{92}") + ": " + num3.ToString(), true);
            else
                GameManager.Inst.UIManager.SetConsoleText(GameManager.Inst.UIManager.GetTranslatedMessage("{93}") + ": " + num3.ToString(), true);
            if (myStatus.Energy > 0f)
                myStatus.Stamina = myStatus.MaxStamina;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerSurvival), "CompleteTraveling", MethodType.Normal)]
    internal static class Patch_3
    {
        [HarmonyPrefix]
        private static bool Prefix(
        float hours,
        bool isHandcar,
        PlayerSurvival __instance
        )
        {
            CharacterStatus myStatus = GameManager.Inst.PlayerControl.SelectedPC.MyStatus;
            myStatus.SetEnergy(Mathf.Clamp(myStatus.Energy + __instance.GetEatenCalories(), 0f, myStatus.MaxEnergy));
            __instance.SetEatenCalories(0f);
            Debug.Log("Travelling, now energy is " + myStatus.Energy.ToString());
            float num = 1;
            if (Utils.GetConfigKey(ConfigKey.TravelEnergyConsumption) is float travelEnergyMod)
            {
                num = travelEnergyMod;
                //SurvivalChanges_BepInEx.LogMessage($"TravelEnergyMod: {travelEnergyMod}");
            }
            else
                num = 120f;
            if (isHandcar)
            {
                //SurvivalChanges_BepInEx.LogMessage($"UsingHandCar: {isHandcar}");
                if (Utils.GetConfigKey(ConfigKey.HandcarEnergyConsumption) is float handcarEnergyMod)
                {
                    num = handcarEnergyMod;
                    //SurvivalChanges_BepInEx.LogMessage($"HandcarEnergyMod: {handcarEnergyMod}");
                }
                else
                    num = 50f;
            }
            float num2 = 1f;
            if (GameManager.Inst.PlayerControl.SelectedPC.MySkills.UnlockedPerks.Contains(SkillPerks.PhysiqueEnergyUse))
            {
                num2 = 0.75f;
            }
            float num3 = hours * num * GameManager.Inst.Constants.HCEnergySpendMultiplier * num2;
            //SurvivalChanges_BepInEx.LogMessage($"EnergyCost: {num3} - Hours: {hours} - Cost: {num} - SkillModifier: {num2} - Modifier: {GameManager.Inst.Constants.HCEnergySpendMultiplier}");
            myStatus.SetEnergy(Mathf.Clamp(myStatus.Energy - num3, 0f, myStatus.MaxEnergy));
            Debug.Log("Travelling, delta is " + num3.ToString() + " now energy is " + myStatus.Energy.ToString());
            if (myStatus.Energy > 0f)
            {
                myStatus.Stamina = myStatus.MaxStamina;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(WorldManager), "GetTravellingTime", MethodType.Normal)]
    internal static class Patch_4
    {
        [HarmonyPostfix]
        private static void Postfix(
            ref float __result
        )
        {
            if (Utils.GetConfigKey(ConfigKey.TravelTimeMultiplier) is float travelMult)
            {
                //SurvivalChanges_BepInEx.LogMessage($"TravelTimePreFix: {__result}");
                __result = __result * travelMult;
                //SurvivalChanges_BepInEx.LogMessage($"TravelTimePostFix: {__result}");
            }
        }
    }
}