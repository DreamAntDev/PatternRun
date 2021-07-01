using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
public class InputPad : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [System.Serializable]
    public struct Point
    {
        public GameObject obj;
        public string value;
    }
    public List<Point> PointList = new List<Point>();
    public BoxCollider2D inputCollider;
    public LineRenderer lineRenderer;

    private RectTransform inputRectTransform;
    private RectTransform rectTransform;

    private List<GameObject> userInput = new List<GameObject>();
    void Awake()
    {
        this.inputCollider.enabled = false;
        this.inputRectTransform = inputCollider.GetComponent<RectTransform>();
        this.rectTransform = GetComponent<RectTransform>();
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, MainUI.Instance.GetUICamera(), out localPos);
        this.inputRectTransform.localPosition = localPos;
        if(userInput.Count > 0)
        {
            lineRenderer.positionCount = userInput.Count + 1;
            lineRenderer.SetPosition(userInput.Count, this.inputCollider.transform.position);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        inputCollider.enabled = true;
        userInput.Clear();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        inputCollider.enabled = false;
        OnInputComplete();
    }

    public void Input(GameObject obj)
    {
        if (userInput.Find(o => obj.Equals(o)) == null)
        {
            userInput.Add(obj);
            lineRenderer.positionCount = userInput.Count;
            
            lineRenderer.SetPosition(userInput.Count-1, obj.transform.position);
        }
    }

    public void OnInputComplete()
    {
        string inputString = string.Empty;
        foreach(var obj in userInput)
        {
            var inputPoint = this.PointList.Find(point => point.obj.Equals(obj));
            inputString += inputPoint.value;
        }
        Debug.Log(inputString);
        this.lineRenderer.positionCount = 0;
        userInput.Clear();
    }
}
