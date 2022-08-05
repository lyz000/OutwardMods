using System.Collections.Generic;

namespace Misc.ItemAction
{
    abstract class ItemActionBase
    {
        public ItemDisplayOptionPanel itemPanel
        {
            get;
            private set;
        }

        public ItemDisplay itemDisplay
        {
            get
            {
                return itemPanel.m_activatedItemDisplay;
            }
        }

        public Item item
        {
            get
            {
                return itemDisplay.RefItem;
            }
        }

        public int ActionID
        {
            get;
            private set;
        }

        public int SellPrice
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
            this.itemPanel = itemDisplayOptionPanel;
            this.ActionID = actionID;
        }

        public List<int> PatchAction(List<int> actions)
        {
            actions.Add(ActionID);
            return actions;
        }

        public void PerformAction()
        {
            if (itemDisplay == null)
            {
                return;
            }

            if (item == null || item is Skill)
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
