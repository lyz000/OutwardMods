using HarmonyLib;
using Misc.ItemAction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ItemDisplayOptionPanel))]
    class ItemDisplayOptionPanel_Patcher
    {
        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions), new Type[] { typeof(GameObject) }), HarmonyPostfix]
        static void GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {
            if (ItemToCoinsAction.GetInstance(__instance).DisplayAction())
            {
                __result = ItemToCoinsAction.GetInstance(__instance).PatchAction(__result);
            }
            if (RepairEquipmentAction.GetInstance(__instance).DisplayAction())
            {
                __result = RepairEquipmentAction.GetInstance(__instance).PatchAction(__result);
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed), new Type[] { typeof(int) }), HarmonyPrefix]
        static void ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            if (ItemToCoinsAction.GetInstance(__instance).ActionID == _actionID)
            {
                ItemToCoinsAction.GetInstance(__instance).PerformAction();
            }
            if (RepairEquipmentAction.GetInstance(__instance).ActionID == _actionID)
            {
                RepairEquipmentAction.GetInstance(__instance).PerformAction();
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText), new Type[] { typeof(int) }), HarmonyPrefix]
        private static bool ItemDisplayOptionPanel_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            if (ItemToCoinsAction.GetInstance(__instance).ActionID == _actionID)
            {
                __result = ItemToCoinsAction.GetInstance(__instance).GetActionText();
                return false;
            }
            if (RepairEquipmentAction.GetInstance(__instance).ActionID == _actionID)
            {
                __result = RepairEquipmentAction.GetInstance(__instance).GetActionText();
                return false;
            }
            return true;
        }

    }
}
