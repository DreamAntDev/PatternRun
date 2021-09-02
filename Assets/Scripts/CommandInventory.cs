using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandInventory : MonoBehaviour
{
    public class Item
    {
        public NData.Item itemData;
        public int itemCount = 0;
        public Item(NData.Item item)
        {
            this.itemData = item;
            this.itemCount = item.excuteCount;
        }
    }

    private List<Item> commandItemList = new List<Item>();
    private List<Item> autoActiveItemList = new List<Item>();

    public bool AddItem(NData.Item item)
    {
        if(item.type==NData.Item.Type.Command)
        {
            var innerItem = commandItemList.Find(o => o.itemData.Equals(item));
            if(innerItem == null)
            {
                commandItemList.Add(new Item(item));
            }
            else
            {
                if (item.stackable == true)
                {
                    innerItem.itemCount += item.excuteCount;
                }
            }
            UpdateUI();
            return true;
        }
        else if(item.type == NData.Item.Type.AutoActive)
        {
            var innerItem = autoActiveItemList.Find(o => o.itemData.Equals(item));
            if (innerItem == null)
            {
                autoActiveItemList.Add(new Item(item));
            }
            else
            {
                if (item.stackable == true)
                {
                    innerItem.itemCount += item.excuteCount;
                }
            }
            UpdateUI();
            return true;
        }

        return false;
    }

    public NData.Item GetCommandItem(string command)
    {
        string actionName = string.Empty;
        foreach (var item in this.commandItemList)
        {
            foreach (var usingCmd in item.itemData.usingCommand)
            {
                if (usingCmd.Equals(command) == true)
                {
                    return item.itemData;
                }
            }
        }
        return null;
    }

    public NData.Item GetAutoActiveItem(string triggerName)
    {
        var findItem = this.autoActiveItemList.Find(o => o.itemData.triggerName.Equals(triggerName));
        if (findItem == null)
            return null;

        return findItem.itemData;
    }

    public void UseItem(NData.Item useItem)
    {
        List<Item> targetInventoryList = null;
        Item innerItem = this.commandItemList.Find(o => o.itemData.Equals(useItem));
        if(innerItem != null)
        {
            targetInventoryList = this.commandItemList;
        }
        else
        {
            innerItem = this.autoActiveItemList.Find(o => o.itemData.Equals(useItem));
            if(innerItem != null)
            {
                targetInventoryList = this.autoActiveItemList;
            }
            else
            {
                // 인벤에 아이템이 없는데 사용함
                return;
            }
        }

        if (innerItem.itemData.excuteCount > 0)
        {
            if (innerItem.itemCount > 0)
            {
                innerItem.itemCount--;
                if(innerItem.itemCount == 0)
                {
                    targetInventoryList.Remove(innerItem);
                }
            }
            else // 카운트가 없는데 사용함...
            {
                return;
            }
        }
        UpdateUI();
        return;
    }

    public bool isEnableItem(NData.Item item)
    {
        if(this.commandItemList.Find(o=>o.itemData.Equals(item)) != null)
        {
            return true;
        }

        if (this.autoActiveItemList.Find(o => o.itemData.Equals(item)) != null)
        {
            return true;
        }

        return false;
    }

    public void UpdateUI()
    {
        MainUI.Instance.userCommand.UpdateList(this.commandItemList);
    }
}
