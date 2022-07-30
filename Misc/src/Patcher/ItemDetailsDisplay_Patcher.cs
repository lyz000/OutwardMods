using System;
using HarmonyLib;
using static ItemDetailsDisplay;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ItemDetailsDisplay))]
    class ItemDetailsDisplay_Patcher
    {
        [HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDetail), new Type[] { typeof(int), typeof(DisplayedInfos) }), HarmonyPostfix]
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

        [HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDisplay), new Type[] { typeof(IItemDisplay) }), HarmonyPostfix]
        static void RefreshDisplay_Postfix(ItemDetailsDisplay __instance, IItemDisplay _itemDisplay)
        {
            var itemDetailsDisplay = __instance;

            if (_itemDisplay == null)
                return;

            var item = itemDetailsDisplay.m_lastItem;
            if (item == null)
                return;

            // if item is silver(9000010), NullReferenceException will occurs.

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
