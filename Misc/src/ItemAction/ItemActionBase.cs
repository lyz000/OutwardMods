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

        public ItemDisplay ItemDisplay_
        {
            get
            {
                return ItemPanel.m_activatedItemDisplay;
            }
        }

        public Item Item_
        {
            get
            {
                return ItemDisplay_.RefItem;
            }
        }

        public int SellPrice
        {
            get
            {
                return ModUtil.GetEstimatedPrice(ItemDisplay_);
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
            if (ItemDisplay_ == null)
            {
                return;
            }

            if (Item_ == null || Item_ is Skill)
            {
                return;
            }

            Action();
            IsActionDone = true;
        }

        abstract public bool DisplayAction();

        abstract public string GetActionText();

        abstract protected void Action();
    }
}
