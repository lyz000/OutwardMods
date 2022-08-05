using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Misc.ItemAction;

namespace Misc.Patch
{
    [HarmonyPatch(typeof(ItemDisplayOptionPanel))]
    class ItemDisplayOptionPanel_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions))]
        [HarmonyPatch(new Type[] { typeof(GameObject) })]
        static void GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {
            if (SellItemAction.GetInstance(__instance).DisplayAction())
            {
                __result = SellItemAction.GetInstance(__instance).PatchAction(__result);
            }
            if (RepairEquipmentAction.GetInstance(__instance).DisplayAction())
            {
                __result = RepairEquipmentAction.GetInstance(__instance).PatchAction(__result);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed))]
        [HarmonyPatch(new Type[] { typeof(int) })]
        static void ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            if (SellItemAction.GetInstance(__instance).ActionID == _actionID)
            {
                SellItemAction.GetInstance(__instance).PerformAction();
            }
            if (RepairEquipmentAction.GetInstance(__instance).ActionID == _actionID)
            {
                RepairEquipmentAction.GetInstance(__instance).PerformAction();
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText), new Type[] { typeof(int) }), HarmonyPrefix]
        private static bool ItemDisplayOptionPanel_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            if (SellItemAction.GetInstance(__instance).ActionID == _actionID)
            {
                __result = SellItemAction.GetInstance(__instance).GetActionText();
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
