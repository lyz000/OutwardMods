namespace Misc.ItemAction
{
    class ItemToCoinsAction : ItemActionBase
    {
        
        private ItemToCoinsAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
            : base(itemDisplayOptionPanel, 5001) 
        {
        }

        public static ItemToCoinsAction Instance
        {
            get;
            private set;
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

        public override bool DisplayAction()
        {
            return Settings.ItemToCoins.Value &&
                NotMerchantItem();
        }

        public override string GetActionText()
        {
            return $"To coins({SellPrice})";
        }

        protected override void Action()
        {
            if (ItemDisplay == null)
            {
                return;
            }

            if (Item == null || Item is Skill)
            {
                return;
            }

            var inventory = ItemPanel.LocalCharacter.Inventory;
            inventory.AddMoney(SellPrice);
            inventory.TakeCurrencySound();
            Item.RemoveQuantity(1);
            ItemPanel.CharacterUI.ShowInfoNotification($"+{SellPrice} coins, total {inventory.ItemCount(9000010)} coins.");
        }
    }
}
