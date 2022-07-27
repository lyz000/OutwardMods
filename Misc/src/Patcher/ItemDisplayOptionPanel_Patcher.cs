using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc.Patcher
{
    [HarmonyPatch(typeof(ItemDisplayOptionPanel))]
    class ItemDisplayOptionPanel_Patcher
    {
        private static readonly ItemToCoinsAction itemToCoinsAction = new ItemToCoinsAction();

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions), new Type[] { typeof(GameObject) }), HarmonyPostfix]
        static void GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {
            if (itemToCoinsAction.IsEnabled && itemToCoinsAction.CanToCoins(__instance))
            {
                __result = itemToCoinsAction.PatchAction(__result);
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed), new Type[] { typeof(int) }), HarmonyPrefix]
        static void ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            if (itemToCoinsAction.ID == _actionID)
            {
                itemToCoinsAction.PerformAction(__instance);
            }
        }

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText), new Type[] { typeof(int) }), HarmonyPrefix]
        private static bool ItemDisplayOptionPanel_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            if (itemToCoinsAction.ID == _actionID)
            {
                __result = itemToCoinsAction.getActionText(__instance);
                return false;
            }
            return true;
        }
    }

    internal class ItemToCoinsAction
    {
        public readonly int ID = 4300;
        public string getActionText(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {

            return $"To Coins({ModUtil.getEstimatedPrice(itemDisplayOptionPanel.m_activatedItemDisplay)})";
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

        public void PerformAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            var isInventoryItem = IsItemInCharacterInventory(itemDisplayOptionPanel, out Item item);
            if (!isInventoryItem)
            {
                return;
            }

            if (item == null || item is Skill)
            {
                return;
            }

            var itemDisplay = itemDisplayOptionPanel.m_activatedItemDisplay;
            if (itemDisplay == null)
            {
                return;
            }

            var character = itemDisplayOptionPanel.LocalCharacter;
            var inventory = character.Inventory;
            var sellPrice = ModUtil.getEstimatedPrice(itemDisplay);
            inventory.AddMoney(sellPrice);
            inventory.TakeCurrencySound();
            character.Inventory.RemoveItem(item.ItemID, 1);
            itemDisplayOptionPanel.CharacterUI.ShowInfoNotification($"+{sellPrice} coins, total {inventory.ItemCount(9000010)} coins.");
        }

        public bool IsItemSafeToCoins(Item item)
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

        public bool IsItemInCharacterInventory(ItemDisplayOptionPanel itemDisplayOptionPanel, out Item item)
        {
            item = null;
            var itemDisplay = itemDisplayOptionPanel.m_activatedItemDisplay;
            if (itemDisplay == null)
            {
                return false;
            }

            item = itemDisplay.RefItem;
            var character = itemDisplayOptionPanel.LocalCharacter;
            var inventory = character.Inventory;
            return inventory.IsItemInBag(item.UID) || inventory.IsItemInPouch(item.UID);
        }

        public bool CanToCoins(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            if (IsItemInCharacterInventory(itemDisplayOptionPanel, out Item item))
            {
                if (IsItemSafeToCoins(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
