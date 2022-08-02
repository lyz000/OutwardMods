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
                ModUtil.NotMerchantItem(Item_);
        }

        public override string GetActionText()
        {
            return $"Sell({SellPrice})";
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
            inventory.AddMoney(SellPrice);
            inventory.TakeCurrencySound();
            Item_.RemoveQuantity(1);
            ItemPanel.CharacterUI.ShowInfoNotification($"+{SellPrice} coins, total {inventory.AvailableMoney} coins.");
        }
    }
}
