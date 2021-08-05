using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCommandListItem : MonoBehaviour
{
    public Image command;
    public Image icon;
    public TMPro.TextMeshProUGUI count;
    public TMPro.TextMeshProUGUI desc;

    public CommandInventory.Item item;

    private Canvas commandCanvas;
    private Canvas iconCanvas;

    private void Awake()
    {
        this.commandCanvas = command.GetComponent<Canvas>();
        this.iconCanvas = icon.GetComponent<Canvas>();
    }

    public void SetColorAlpha(float alphaValue,int commandOrder,int iconOrder)
    {
        var commandAlpha = alphaValue;
        var iconAhpha = 1.0f - alphaValue;

        var commandColor = this.command.color;
        commandColor.a = commandAlpha;
        this.command.color = commandColor;

        var iconColor = this.icon.color;
        iconColor.a = iconAhpha;
        this.icon.color = iconColor;

        this.commandCanvas.sortingOrder = commandOrder;
        this.iconCanvas.sortingOrder = iconOrder;
    }

    public void SetItem(CommandInventory.Item item)
    {
        if (this.item != item)
        {
            this.item = item;
            this.icon.sprite = MainUI.Instance.iconAtlas.GetSprite(item.itemData.iconName);
            this.command.sprite = MainUI.Instance.iconAtlas.GetSprite(item.itemData.patternName);
            this.gameObject.SetActive(true);
        }

        if (item.itemData.excuteCount > 0)
        {
            this.count.SetText(item.itemCount.ToString());
        }
        else
        {
            this.count.SetText(string.Empty);
        }

    }

    public void StartSwitchDisplay()
    {
        StartCoroutine(SwitchingCoroutine());
    }
    IEnumerator SwitchingCoroutine()
    {
        var increaser = command;
        var decreaser = icon;

        var tempColor = increaser.color;
        tempColor.a = 0.0f;
        increaser.color = tempColor;
        while(true)
        {
            var decColor = decreaser.color;
            decColor.a = Mathf.Lerp(decColor.a, 0.0f, Time.deltaTime*2);
            decreaser.color = decColor;

            var incColor = increaser.color;
            incColor.a = 1.0f - decColor.a;
            increaser.color = incColor;

            yield return null;
            if(decreaser.color.a<=0.01f && increaser.color.a>=0.99f)
            {
                var temp = decreaser;
                decreaser = increaser;
                increaser = temp;
                increaser.GetComponent<Canvas>().sortingOrder = 1;
                decreaser.GetComponent<Canvas>().sortingOrder = 0;
            }
        }
    }
}
