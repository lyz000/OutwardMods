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
                ModUtil.NotMerchantItem(item) &&
                item.DurabilityRatio < 0.98f; // avoid display repair action when pick up new food
        }

        public override string GetActionText()
        {
            return "Repair";
        }

        protected override void Action()
        {
            var inventory = itemPanel.LocalCharacter.Inventory;
            var cost = 10;
            if (inventory.AvailableMoney < cost)
            {
                itemPanel.CharacterUI.ShowInfoNotification($"No enough coins({cost}).");
                return;
            }

            item.SetDurabilityRatio(1f);// for equipped
            if (item.ParentContainer != null)
            {
                item.ParentContainer.GetItemsFromID(item.ItemID).ForEach(item => item.SetDurabilityRatio(1f));// for stack
            }
            inventory.RemoveMoney(10);
            inventory.TakeCurrencySound();
            itemPanel.CharacterUI.ShowInfoNotification($"{item.Name} repaired, -10 coins.");
        }
    }
}
