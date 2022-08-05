using System;
using HarmonyLib;
using static ItemDetailsDisplay;

namespace Misc.Patch
{
    [HarmonyPatch(typeof(ItemDetailsDisplay))]
    class ItemDetailsDisplay_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDetail))]
        [HarmonyPatch(new Type[] { typeof(int), typeof(DisplayedInfos) })]
        static void RefreshDetail_Postfix(ItemDetailsDisplay __instance, int _rowIndex, DisplayedInfos _infoType)
        {
            var itemDetailsDisplay = __instance;
            if (_infoType != DisplayedInfos.Durability)
            {
                return;
            }

            var item = itemDetailsDisplay.m_lastItem;
            if (item == null)
            {
                return;
            }

            if (Settings.DisplayDurability.Value)
            {
                ModUtil.AddDurabiliityInfo(itemDetailsDisplay, item);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDisplay))]
        [HarmonyPatch(new Type[] { typeof(IItemDisplay) })]
        static void RefreshDisplay_Postfix(ItemDetailsDisplay __instance, IItemDisplay _itemDisplay)
        {
            var itemDetailsDisplay = __instance;

            if (_itemDisplay == null)
                return;

            var item = itemDetailsDisplay.m_lastItem;
            if (item == null)
                return;

            // if item is silver(9000010), NullReferenceException occurs.

            if (Settings.DisplaySellPrice.Value)
            {
                if (item.Value > 0 && item.Weight > 0 && item.IsSellable)
                {
                    ModUtil.SetRowInfo(itemDetailsDisplay, "Sell", $"{ModUtil.GetEstimatedPrice(_itemDisplay)}");
                }
            }

            if (Settings.DisplayDurability.Value)
            {
                ModUtil.AddDurabiliityInfo(itemDetailsDisplay, item);
            }
        }
    }
}
