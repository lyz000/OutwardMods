namespace Misc.ItemAction
{
    class SellItemAction : ItemActionBase
    {
        
        private SellItemAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
            : base(itemDisplayOptionPanel, 5001) 
        {
        }

        public static SellItemAction Instance
        {
            get;
            private set;
        }

        public static SellItemAction GetInstance(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            if (Instance == null)
            {
                Instance = new SellItemAction(itemDisplayOptionPanel);
                return Instance;
            }

            if (Instance.IsActionDone)
            {
                Instance = new SellItemAction(itemDisplayOptionPanel);
            }
            return Instance;
        }

        public override bool DisplayAction()
        {
            return Settings.SellItem.Value &&
                NotMerchantItem();
        }

        public override string GetActionText()
        {
            if (Item_.ItemID == 9000010)// silver coin
            {
                return "Buy Gold Ingot(100)";
            }
            else
            {
                return $"Sell({SellPrice})";
            }
        }

        protected override void Action()
        {
            if (ItemDisplay_ == null)
            {
                return;
            }

            if (Item_ == null || Item_ is Skill)
            {
                return;
            }

            var inventory = ItemPanel.LocalCharacter.Inventory;
            if (Item_.ItemID == 9000010)// silver coin
            {
                if (inventory.AvailableMoney < 100)
                {
                    ItemPanel.CharacterUI.ShowInfoNotification($"Requires 100 Coins!");
                    return;
                }
                inventory.RemoveMoney(100);
                inventory.TakeCurrencySound();
                ItemPanel.CharacterUI
                    .ShowInfoNotification($"-100 Coins and +1 Gold Ingot, total {inventory.AvailableMoney} coins and {inventory.ItemCount(6300030)} Gold Ingot.");
            }
            else
            {
                inventory.AddMoney(SellPrice);
                inventory.TakeCurrencySound();
                Item_.RemoveQuantity(1);
                ItemPanel.CharacterUI.ShowInfoNotification($"+{SellPrice} Coins, total {inventory.AvailableMoney} Coins.");
            }
        }
    }
}
