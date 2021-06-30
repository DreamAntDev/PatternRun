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
    /*
     * timeLen�� 0�ΰ�� Close�� �ϱ� ������ ������� �ʽ��ϴ�.
     */
    public void SetMessage(string text, string textSub = "",float timeLen = 0.0f)
    {
        this.gameObject.SetActive(true);

        this.text.text = text;
        this.textSub.text = textSub;
        if (curActiveCoroutine != null)
        {
            StopCoroutine(curActiveCoroutine);
        }
        if(timeLen>0.0f)
        {
            StartCoroutine(ActiveCoroutine(timeLen));
        }
    }

    IEnumerator ActiveCoroutine(float timeLen)
    {
        yield return new WaitForSeconds(timeLen);
        this.gameObject.SetActive(false);
        this.curActiveCoroutine = null;
    }
}
