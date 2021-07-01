using UnityEngine;
using UnityEngine.EventSystems;

public class PattenEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private bool isEnter = false;

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnter)
        {
            isEnter = true;
            GameManager.instance.ChainTouch(eventData.pointerCurrentRaycast.gameObject.name);
            MainUI.Instance.AddInputPos(eventData.pointerCurrentRaycast.gameObject.transform.position);
        }
        else
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnter)
        {
            isEnter = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.Reset();
        MainUI.Instance.ResetInput();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.InputStart();
        GameManager.instance.ChainTouch(eventData.pointerCurrentRaycast.gameObject.name);
        MainUI.Instance.AddInputPos(eventData.pointerCurrentRaycast.gameObject.transform.position);
    }
}
