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
                ModUtil.NotMerchantItem(item);
        }

        public override string GetActionText()
        {
            return $"Sell({SellPrice})";
        }

        protected override void Action()
        {
            if (itemDisplay == null)
            {
                return;
            }

            if (item == null || item is Skill)
            {
                return;
            }
  
            var inventory = itemPanel.LocalCharacter.Inventory;
            inventory.AddMoney(SellPrice);
            inventory.TakeCurrencySound();
            item.RemoveQuantity(1);
            itemPanel.CharacterUI.ShowInfoNotification($"+{SellPrice} coins, total {inventory.AvailableMoney} coins.");
        }
    }
}
