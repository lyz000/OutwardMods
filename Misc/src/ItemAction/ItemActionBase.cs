using System.Collections.Generic;

namespace Misc.ItemAction
{
    abstract class ItemActionBase
    {
        public ItemDisplayOptionPanel ItemPanel
        {
            get;
            private set;
        }

        public int ActionID
        {
            get;
            private set;
        }

        public ItemDisplay ItemDisplay
        {
            get
            {
                return ItemPanel.m_activatedItemDisplay;
            }
        }

        public Item Item
        {
            get
            {
                return ItemDisplay.RefItem;
            }
        }

        public int SellPrice
        {
            get
            {
                return ModUtil.GetEstimatedPrice(ItemDisplay);
            }
        }

        public bool IsActionDone
        {
            get;
            protected set;
        }

        public ItemActionBase(ItemDisplayOptionPanel itemDisplayOptionPanel, int actionID)
        {
            this.ItemPanel = itemDisplayOptionPanel;
            this.ActionID = actionID;
        }

        public List<int> PatchAction(List<int> actions)
        {
            actions.Add(ActionID);
            return actions;
        }

        public void PerformAction()
        {
            if (ItemDisplay == null)
            {
                return;
            }

            if (Item == null || Item is Skill)
            {
                return;
            }

            Action();
            IsActionDone = true;
        }

        public bool NotMerchantItem()
        {
            if (Item == null)
            {
                return false;
            }

            if (Item.ParentContainer is MerchantPouch)
            {
                return false;
            }

            return true;
        }

        abstract public bool DisplayAction();

        abstract public string GetActionText();

        abstract protected void Action();
    }
}
