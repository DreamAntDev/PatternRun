using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCommand : MonoBehaviour
{
    public UIFakeItem fakeItem;
    public List<UserCommandListItem> listItemList;

    //int currentIndex = -1;
    //Vector3 fakeItemBeginPos = Vector3.zero;
    //RectTransform rectTransform;

    float currentAlpha = 1.0f;
    float targetAlpha = 0.0f;
    int commandOrder = 1;
    int iconOrder = 0;
    public void Start()
    {
        //this.fakeItemBeginPos = fakeItem.GetComponent<RectTransform>().anchoredPosition3D;
        //this.rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha , Time.deltaTime*2);
        foreach (var slot in listItemList)
        {
            if(slot.gameObject.activeInHierarchy == true)
            {
                slot.SetColorAlpha(currentAlpha, commandOrder, iconOrder);
            }
        }
        if(currentAlpha<=0.01f)
        {
            commandOrder = 0;
            iconOrder = 1;
            targetAlpha = 1.0f;
        }
        else if(currentAlpha >= 0.99f)
        {
            commandOrder = 1;
            iconOrder = 0;
            targetAlpha = 0.0f;
        }
    }

    //public void Insert(Vector3 worldPos, string iconName, string patternName)
    //{
    //    if (currentIndex != listItemList.Count - 1)
    //        currentIndex++;
    //    else // ¸Ô±â ½ÇÆÐ
    //        return;
    //    //InsertUsingTween(worldPos,iconName,patternName);
    //    SetPattern(iconName, patternName);
    //}

    //private void InsertUsingTween(Vector3 worldPos, string iconName, string patternName)
    //{
    //    //fakeItem.gameObject.GetComponent<RectTransform>().anchoredPosition;
    //    //var cam = MainUI.Instance.GetUICamera();
    //    var cam = Camera.main;

    //    var screenPos = cam.WorldToScreenPoint(worldPos);
    //    Vector2 localRectTrans;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPos, MainUI.Instance.GetUICamera(), out localRectTrans);

    //    fakeItem.gameObject.SetActive(true);
    //    fakeItem.icon.sprite = MainUI.Instance.iconAtlas.GetSprite(iconName);
    //    fakeItem.GetComponent<RectTransform>().anchoredPosition = localRectTrans;
    //    //fakeItem.GetComponent<RectTransform>().anchoredPosition3D = this.fakeItemBeginPos;
    //    var tween = fakeItem.GetComponent<UITween>();

    //    var listItem = listItemList[currentIndex];
    //    var targetPos = listItem.GetComponent<RectTransform>().anchoredPosition3D;
    //    tween.endPos = targetPos;
    //    tween.CreateClip();
    //    tween.AnimPlay(() => SetPattern(iconName, patternName));
    //}

    //public void SetPattern(string iconName,string patternName)
    //{
    //    var listItem = listItemList[currentIndex];

    //    listItem.gameObject.SetActive(true);
    //    this.fakeItem.gameObject.SetActive(false);
    //}

    public void UpdateList(List<CommandInventory.Item> itemList)
    {
        int i = 0;
        for (i = 0; i < itemList.Count && i < this.listItemList.Count; i++)
        {
            this.listItemList[i].SetItem(itemList[i]);
        }
        for(; i<this.listItemList.Count;i++)
        {
            this.listItemList[i].gameObject.SetActive(false);
        }
    }

    public void ClearCommand()
    {
        //this.currentIndex = -1;
        foreach(var item in listItemList)
        {
            item.gameObject.SetActive(false);
        }
    }
}
