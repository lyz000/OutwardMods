using HarmonyLib;
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
            if (ItemToCoinsAction.GetInstance(__instance).IsEnabled && ItemToCoinsAction.GetInstance(__instance).CanToCoins())
            {
                __result = ItemToCoinsAction.GetInstance(__instance).PatchAction(__result);
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed), new Type[] { typeof(int) }), HarmonyPrefix]
        static void ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            if (ItemToCoinsAction.GetInstance(__instance).ID == _actionID)
            {
                ItemToCoinsAction.GetInstance(__instance).PerformAction();
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText), new Type[] { typeof(int) }), HarmonyPrefix]
        private static bool ItemDisplayOptionPanel_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            if (ItemToCoinsAction.GetInstance(__instance).ID == _actionID)
            {
                __result = ItemToCoinsAction.GetInstance(__instance).getActionText(__instance);
                return false;
            }
            return true;
        }
    }

    internal class ItemToCoinsAction
    {

        public readonly int ID = 5000;

        public bool IsExpired
        {
            get;
            private set;
        }

        public ItemDisplayOptionPanel itemDisplayOptionPanel
        {
            get;
            private set;
        }

        public ItemDisplay itemDisplay
        {
            get
            {
                return itemDisplayOptionPanel.m_activatedItemDisplay;
            }
        }

        public Item item
        {
            get
            {
                return itemDisplay.RefItem;
            }
        }

        public int sellPrice
        {
            get
            {
                return ModUtil.getEstimatedPrice(itemDisplay);
            }
        }

        private ItemToCoinsAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
        { 
            this.itemDisplayOptionPanel = itemDisplayOptionPanel;
        }

        private static ItemToCoinsAction Instance;

        public static ItemToCoinsAction GetInstance(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            if (Instance == null)
            {
                Instance = new ItemToCoinsAction(itemDisplayOptionPanel);
                return Instance;
            }

            if (Instance.IsExpired)
            {
                Instance = new ItemToCoinsAction(itemDisplayOptionPanel);
            }
            return Instance;
        }

        public string getActionText(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            return $"To coins({ModUtil.getEstimatedPrice(itemDisplayOptionPanel.m_activatedItemDisplay)})";
        }

        public bool IsEnabled
        {
            get
            {
                return Settings.ItemToCoins.Value;
            }
        }

        public List<int> PatchAction(List<int> options)
        {
            options.Add(ID);
            return options;
        }

        public void PerformAction()
        {
            if (itemDisplay == null)
            {
                return;
            }

            if (item == null || item is Skill)
            {
                return;
            }

            var inventory = itemDisplayOptionPanel.LocalCharacter.Inventory;
            inventory.AddMoney(sellPrice);
            inventory.TakeCurrencySound();
            item.RemoveQuantity(1);
            itemDisplayOptionPanel.CharacterUI.ShowInfoNotification($"+{sellPrice} coins, total {inventory.ItemCount(9000010)} coins.");
            IsExpired = true;
        }

        public bool IsItemSafeToCoins()
        {
            return item != null
                //!item is WrittenNote &&
                //!item is Blueprint &&
                //!item is Building &&
                //!item is CraftingStation &&
                //!item is Currency &&
                //!item is EnchantmentRecipeItem &&
                //!item is Equipment &&
                //!item is Food &&
                //!item is InfuseConsumable &&
                //!item is Instrument &&
                //!item is ItemContainer &&
                //!item is ItemFragment &&
                //!item is Quest &&
                //!item is RecipeItem &&
                //!item is RunicLantern &&
                //!item is Skill
                //!item is Throwable &&
                //!item is WaterContainer &&
                //!item is WaterItem
                ;
        }

        public bool NotMerchantItem()
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

        public bool CanToCoins()
        {
            return NotMerchantItem() && IsItemSafeToCoins();
        }
    }
}
