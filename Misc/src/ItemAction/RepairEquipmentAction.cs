using System.Linq;

namespace Misc.ItemAction
{
    class RepairEquipmentAction : ItemActionBase
    {
        private RepairEquipmentAction(ItemDisplayOptionPanel itemDisplayOptionPanel)
             : base(itemDisplayOptionPanel, 5002)
        {
        }

        public static RepairEquipmentAction Instance
        {
            get;
            private set;
        }

        public static RepairEquipmentAction GetInstance(ItemDisplayOptionPanel itemDisplayOptionPanel)
        {
            if (Instance == null)
            {
                Instance = new RepairEquipmentAction(itemDisplayOptionPanel);
                return Instance;
            }

            if (Instance.IsActionDone)
            {
                Instance = new RepairEquipmentAction(itemDisplayOptionPanel);
            }
            return Instance;
        }

        public override bool DisplayAction()
        {
            return Settings.RepairEquipment.Value &&
                NotMerchantItem() &&
                Item.DurabilityRatio < 0.98f; // avoid display repair option when pick up food
        }

        public override string GetActionText()
        {
            return "Repair";
        }

        protected override void Action()
        {
            var inventory = ItemPanel.LocalCharacter.Inventory;
            var cost = 10;
            if (inventory.AvailableMoney < cost)
            {
                ItemPanel.CharacterUI.ShowInfoNotification($"No enough coins({cost}).");
                return;
            }

            Item.SetDurabilityRatio(1f);// for equipped
            inventory.GetOwnedItems(Item.ItemID).ForEach((item) => item.SetDurabilityRatio(1f));// for stack
            inventory.RemoveMoney(10);
            inventory.TakeCurrencySound();
            ItemPanel.CharacterUI.ShowInfoNotification($"{Item.GetLocalizedName()} repaired, -10 coins.");
        }
    }
}
