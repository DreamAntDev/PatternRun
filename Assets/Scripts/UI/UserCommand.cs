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
    public void Start()
    {
        this.fakeItemBeginPos = fakeItem.GetComponent<RectTransform>().anchoredPosition3D;
    }
    public void Insert()
    {
        //fakeItem.gameObject.GetComponent<RectTransform>().anchoredPosition;
        fakeItem.gameObject.SetActive(true);
        fakeItem.GetComponent<RectTransform>().anchoredPosition3D = this.fakeItemBeginPos;
        var tween = fakeItem.GetComponent<UITween>();
        if (currentIndex != listItemList.Count)
            currentIndex++;

        var listItem = listItemList[currentIndex];
        var targetPos = listItem.GetComponent<RectTransform>().anchoredPosition3D;
        tween.endPos = targetPos;
        tween.CreateClip();
        tween.AnimPlay(TweenAnimEnd);
    }

    public void TweenAnimEnd()
    {
        var listItem = listItemList[currentIndex];
        listItem.gameObject.SetActive(true);
        this.fakeItem.gameObject.SetActive(false);
    }
}
