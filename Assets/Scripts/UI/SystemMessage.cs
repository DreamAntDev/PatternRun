using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public TMPro.TextMeshProUGUI textSub;
    public Image bg;
    Coroutine curActiveCoroutine = null;
    System.Action closeFunc;
    /*
     * timeLen이 0인경우 Close를 하기 전까지 사라지지 않습니다.
     */
    public void SetMessage(string text, string textSub = "", float timeLen = 0.0f, System.Action closeFunc = null)
    {
        this.gameObject.SetActive(true);

        this.text.text = text;
        this.textSub.text = textSub;
        if (curActiveCoroutine != null)
        {
            this.closeFunc = null;
            StopCoroutine(curActiveCoroutine);
        }
        if(timeLen>0.0f)
        {
            this.closeFunc = closeFunc;
            StartCoroutine(ActiveCoroutine(timeLen));
        }
    }

    IEnumerator ActiveCoroutine(float timeLen)
    {
        yield return new WaitForSeconds(timeLen);
        this.gameObject.SetActive(false);
        this.curActiveCoroutine = null;

        if (this.closeFunc != null)
        {
            this.closeFunc();
            this.closeFunc = null;
        }
    }
}
