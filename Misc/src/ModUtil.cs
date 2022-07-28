using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
