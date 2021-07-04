using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCommand : MonoBehaviour
{
    public UIFakeItem fakeItem;
    public List<UserCommandListItem> listItemList;
    int currentIndex = -1;
    Vector3 fakeItemBeginPos = Vector3.zero;
    RectTransform rectTransform;
    public void Start()
    {
        this.fakeItemBeginPos = fakeItem.GetComponent<RectTransform>().anchoredPosition3D;
        this.rectTransform = GetComponent<RectTransform>();
    }
    public void Insert(Vector3 worldPos, string iconName, string patternName)
    {
        //fakeItem.gameObject.GetComponent<RectTransform>().anchoredPosition;

        var screenPos = RectTransformUtility.WorldToScreenPoint(MainUI.Instance.GetUICamera(), worldPos);
        Vector2 localRectTrans;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPos, MainUI.Instance.GetUICamera(), out localRectTrans);

        fakeItem.gameObject.SetActive(true);
        fakeItem.icon.sprite = MainUI.Instance.iconAtlas.GetSprite(iconName);
        fakeItem.GetComponent<RectTransform>().anchoredPosition = localRectTrans;
        //fakeItem.GetComponent<RectTransform>().anchoredPosition3D = this.fakeItemBeginPos;
        var tween = fakeItem.GetComponent<UITween>();
        if (currentIndex != listItemList.Count-1)
            currentIndex++;

        var listItem = listItemList[currentIndex];
        var targetPos = listItem.GetComponent<RectTransform>().anchoredPosition3D;
        tween.endPos = targetPos;
        tween.CreateClip();
        tween.AnimPlay(() => TweenAnimEnd(iconName, patternName));
    }

    public void TweenAnimEnd(string iconName,string patternName)
    {
        var listItem = listItemList[currentIndex];
        listItem.icon.sprite = MainUI.Instance.iconAtlas.GetSprite(iconName);
        listItem.command.sprite = MainUI.Instance.iconAtlas.GetSprite(patternName);
        listItem.gameObject.SetActive(true);
        this.fakeItem.gameObject.SetActive(false);
    }
}