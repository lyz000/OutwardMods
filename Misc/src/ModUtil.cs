using System;
using BepInEx.Logging;

namespace Misc
{
    class ModUtil
    {
        public static ManualLogSource Logger;

        public static string GameTimeToDays(double p_gametime)
        {
            string str = "";
            int days = (int)(p_gametime / 24);
            if (days > 0) str = $"{days}d, ";
            int hours = (int)(p_gametime % 24);
            str += $"{hours}h";
            if (days == 0)
            {
                hours = (int)Math.Ceiling(p_gametime % 24);
                str = $"{hours}h";
                if (hours <= 1)
                {
                    int minutes = (int)(p_gametime * 60);
                    str = $"{minutes} min";
                    if (minutes == 0)
                    {
                        int seconds = (int)(p_gametime * 3600);
                        str = $"{seconds} sec";
                    }
                }
            }
            return str;
        }

        public static int GetEstimatedPrice(IItemDisplay itemDisplay)
        {
            var item = itemDisplay.RefItem;
            if (!item.IsSellable)
            {
                return 1;
            }

            int sellPrice;
            if (item.ItemID == 6300030)
            {
                // if item is Gold Ingot
                sellPrice = 100;
            }
            else
            {
                sellPrice = Convert.ToInt32(Math.Round(itemDisplay.RefItem.RawCurrentValue * Item.DEFAULT_SELL_MODIFIER, 0));
            }

            return sellPrice;
        }

        public static void SetRowInfo(ItemDetailsDisplay itemDetailsDisplay, string dataName, string dataValue)
        {
            var row = itemDetailsDisplay.m_detailRows.Find(r => r.m_lblDataName.text == dataName);
            if (row == null)
            {
                row = itemDetailsDisplay.GetRow(itemDetailsDisplay.m_detailRows.Count);
            }
            row.SetInfo(dataName, dataValue);
            if (row.IsDisplayed)
                return;
            row.Show();
        }

        public static void AddDurabiliityInfo(ItemDetailsDisplay itemDetailsDisplay, Item item)
        {
            if (item.IsPerishable && item.CurrentDurability > 0)
            {
                if (item.TypeDisplay == "Equipment")
                {
                    ModUtil.SetRowInfo(itemDetailsDisplay,
                        LocalizationManager.Instance.GetLoc("ItemStat_Durability"),
                        $"{Math.Ceiling(item.CurrentDurability)}/{item.MaxDurability}");
                }
                else
                {
                    if (item.PerishScript.DepletionRateModifier < .0001)
                    {
                        ModUtil.SetRowInfo(itemDetailsDisplay,
                            LocalizationManager.Instance.GetLoc("ItemStat_Durability"),
                            $"[{ModUtil.GameTimeToDays(item.CurrentDurability / item.PerishScript.m_baseDepletionRate)}]");
                    }
                    else
                    {
                        ModUtil.SetRowInfo(itemDetailsDisplay,
                            LocalizationManager.Instance.GetLoc("ItemStat_Durability"),
                            ModUtil.GameTimeToDays(item.CurrentDurability / item.PerishScript.DepletionRate));
                    }
                }
            }
        }

        public static bool NotMerchantItem(Item item)
        {
            if (item == null)
            {
                return false;
            }

            if (item.ParentContainer is MerchantPouch)
            {
                return false;
            }

            return true;
        }

        public static void ShowStashPanel(int triggerPlayerID, int targetPlayerID)
        {
            var triggerCharacter = Settings.GetCharacterShareData(triggerPlayerID).getCharacter();
            var targetStash = Settings.GetCharacterShareData(targetPlayerID).getCharacter().Stash;
            triggerCharacter.CharacterUI.StashPanel.SetStash(targetStash);
            targetStash.ShowContent(triggerCharacter);
        }
    }
}
