namespace Misc.ItemAction
{
    class ItemToCoinsAction : ItemActionBase
    {
        public static ItemToCoinsAction Instance
        {
            get;
            private set;
        }

        public ItemToCoinsAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
            : base(itemDisplayOptionPanel, 5001) 
        {
        }

        public static ItemToCoinsAction GetInstance(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            if (Instance == null)
            {
                Instance = new ItemToCoinsAction(itemDisplayOptionPanel);
                return Instance;
            }

            if (Instance.IsActionDone)
            {
                Instance = new ItemToCoinsAction(itemDisplayOptionPanel);
            }
            return Instance;
        }

        public override string GetActionText()
        {
            return $"To coins({sellPrice})";
        }

        public override bool IsEnabled()
        {
            return Settings.ItemToCoins.Value;
        }

        public override void PerformAction()
        {
            if (itemDisplay == null)
            {
                return;
            }

            if (item == null || item is Skill)
            {
                return;
            }

            var inventory = ItemDisplayOptionPanel.LocalCharacter.Inventory;
            inventory.AddMoney(sellPrice);
            inventory.TakeCurrencySound();
            item.RemoveQuantity(1);
            ItemDisplayOptionPanel.CharacterUI.ShowInfoNotification($"+{sellPrice} coins, total {inventory.ItemCount(9000010)} coins.");
            IsActionDone = true;
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
