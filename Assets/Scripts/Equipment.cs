using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Equipment : MonoBehaviour
{
    public List<GameObject> equipList = new List<GameObject>();
    private Dictionary<string, List<GameObject>> equipDictionary = new Dictionary<string, List<GameObject>>();
    private List<GameObject> currentEquipList = new List<GameObject>();

    public void Awake()
    {
        foreach(var item in equipList)
        {
            var partsName = item.transform.parent.name;
            List<GameObject> temp;
            if (equipDictionary.TryGetValue(partsName, out temp) == true)
            {
                temp.Add(item);
            }
            else
            {
                temp = new List<GameObject>();
                temp.Add(item);
                equipDictionary.Add(partsName, temp);
            }
        }
    }

    public void Equip(string name)
    {
        var partsName = GetItemPartsName(name);
        List<GameObject> tempList;
        if (equipDictionary.TryGetValue(partsName, out tempList) == true)
        {
            foreach (var obj in tempList)
            {
                if (obj.name.Equals(name) == true)
                {
                    obj.SetActive(true);
                    if (this.currentEquipList.Contains(obj) == false)
                        this.currentEquipList.Add(obj);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    public void UnEquip(string name)
    {
        var partsName = GetItemPartsName(name);
        List<GameObject> tempList;
        if (equipDictionary.TryGetValue(partsName, out tempList) == true)
        {
            foreach (var obj in tempList)
            {
                if (obj.name.Equals(name) == true)
                {
                    this.currentEquipList.Remove(obj);
                    obj.SetActive(false);
                }
                //else
                //{
                    
                //}
            }
        }

        // 같은 파츠에 장비중인 다른 아이템 착용
        foreach(var tempEquip in tempList)
        {
            if(this.currentEquipList.Contains(tempEquip) == true)
            {
                tempEquip.SetActive(true);
                break;
            }
        }
    }

    private string GetItemPartsName(string name)
    {

        var obj = this.equipList.Find(o => o.name.Equals(name));
        if (obj == null)
            return string.Empty;
        else
            return obj.transform.parent.name;
    }
}
