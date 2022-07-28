using HarmonyLib;
using System;
using static ItemDetailsDisplay;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ItemDetailsDisplay))]
    class ItemDetailsDisplay_Patcher
    {
        //[HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDetail), new Type[] { typeof(int) }), HarmonyPostfix]
        //static void RefreshDetail_Postfix(ItemDetailsDisplay __instance, int _rowIndex, DisplayedInfos _infoType)
        //{ 
        //    var itemDetailsDisplay = __instance;
        //    if (_infoType != DisplayedInfos.Durability)
        //    {
        //        return;
        //    }

        //    var m_lastItem = itemDetailsDisplay.m_lastItem;
        //    if (m_lastItem.IsPerishable && m_lastItem.CurrentDurability > 0)
        //    {
        //        var row = itemDetailsDisplay.GetRow(_rowIndex);
        //        if (m_lastItem.TypeDisplay == "Equipment")
        //        {
        //            row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), $"{m_lastItem.CurrentDurability}/{m_lastItem.MaxDurability}");
        //        }
        //        else
        //        {
        //            if (m_lastItem.PerishScript.DepletionRateModifier < .0001)
        //            {
        //                row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), $"[{ModUtil.GameTimeToDays(m_lastItem.CurrentDurability / m_lastItem.PerishScript.m_baseDepletionRate)}]");
        //            }
        //            else
        //            {
        //                row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), ModUtil.GameTimeToDays(m_lastItem.CurrentDurability / m_lastItem.PerishScript.DepletionRate));
        //            }
        //        }
        //    }
        //}

        [HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDisplay), new Type[] { typeof(IItemDisplay) }), HarmonyPostfix]
        static void RefreshDisplay_Postfix(ItemDetailsDisplay __instance, IItemDisplay _itemDisplay)
        {
            var itemDetailsDisplay = __instance;

            //if (_itemDisplay.RefItem.ItemID == 9000010)
            if (_itemDisplay == null || _itemDisplay.RefItem == null)
            {
                // if item is silver, NullReferenceException will occurs.
                return;
            }

            #region Add Price/Weight Ratio information
            if (Settings.DisplaySellPrice.Value)
            {
                if (_itemDisplay.RefItem.Value > 0 && _itemDisplay.RefItem.Weight > 0 && _itemDisplay.RefItem.IsSellable)
                {
                    var row = itemDetailsDisplay.GetRow(itemDetailsDisplay.m_detailRows.Count);
                    row.SetInfo("Sell", $"{ModUtil.GetEstimatedPrice(_itemDisplay)}");
                }
            }
            #endregion

            #region Add Durability information
            if (Settings.DisplayDurability.Value)
            {
                if (_itemDisplay.RefItem.IsPerishable && _itemDisplay.RefItem.CurrentDurability > 0 && !_itemDisplay.RefItem.DisplayedInfos.Contains(DisplayedInfos.Durability))
                {
                    var row = itemDetailsDisplay.GetRow(itemDetailsDisplay.m_detailRows.Count);
                    if (_itemDisplay.RefItem.TypeDisplay == "Equipment")
                    {
                        row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), $"{_itemDisplay.RefItem.CurrentDurability}/{_itemDisplay.RefItem.MaxDurability}");
                    }
                    else
                    {
                        if (_itemDisplay.RefItem.PerishScript.DepletionRateModifier < .0001)
                        {
                            row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), $"[{ModUtil.GameTimeToDays(_itemDisplay.RefItem.CurrentDurability / _itemDisplay.RefItem.PerishScript.m_baseDepletionRate)}]");
                        }
                        else
                        {
                            row.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_Durability"), ModUtil.GameTimeToDays(_itemDisplay.RefItem.CurrentDurability / _itemDisplay.RefItem.PerishScript.DepletionRate));
                        }
                    }
                }
            }
            #endregion
        }
    }
}
