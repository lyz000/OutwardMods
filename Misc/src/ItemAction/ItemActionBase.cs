using System.Collections.Generic;

namespace Misc.ItemAction
{
    abstract class ItemActionBase
    {
        public ItemDisplayOptionPanel ItemDisplayOptionPanel
        {
            get;
            private set;
        }

        public int ActionID
        {
            get;
            private set;
        }

        public ItemDisplay itemDisplay
        {
            get
            {
                return ItemDisplayOptionPanel.m_activatedItemDisplay;
            }
        }

        public Item item
        {
            get
            {
                return itemDisplay.RefItem;
            }
        }

        public int sellPrice
        {
            get
            {
                return ModUtil.GetEstimatedPrice(itemDisplay);
            }
        }

        public bool IsActionDone
        {
            get;
            protected set;
        }

        public ItemActionBase(ItemDisplayOptionPanel itemDisplayOptionPanel, int actionID)
        {
            this.ItemDisplayOptionPanel = itemDisplayOptionPanel;
            this.ActionID = actionID;
        }

        public List<int> PatchAction(List<int> actions)
        {
            actions.Add(ActionID);
            return actions;
        }

        abstract public string GetActionText();

        abstract public void PerformAction();

        abstract public bool IsEnabled();
    }
}
