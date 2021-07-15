using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
public class InputPad : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler, IPointerUpHandler, IPointerDownHandler
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
    private int dotPerLine = 3;
    void Awake()
    {
        this.inputCollider.enabled = false;
        this.inputRectTransform = inputCollider.GetComponent<RectTransform>();
        this.rectTransform = GetComponent<RectTransform>();
        this.dotPerLine = (int)Mathf.Sqrt(this.PointList.Count);
        var rect = this.rectTransform.rect;       
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, MainUI.Instance.GetUICamera(), out localPos);

        localPos.x = Mathf.Max(localPos.x, this.rectTransform.rect.xMin);
        localPos.x = Mathf.Min(localPos.x, this.rectTransform.rect.xMax);
        localPos.y = Mathf.Max(localPos.y, this.rectTransform.rect.yMin);
        localPos.y = Mathf.Min(localPos.y, this.rectTransform.rect.yMax);

        this.inputRectTransform.localPosition = localPos;
        if(userInput.Count > 0)
        {
            lineRenderer.positionCount = userInput.Count + 1;
            lineRenderer.SetPosition(userInput.Count, this.inputCollider.transform.position);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //inputCollider.enabled = true;
        //userInput.Clear();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //inputCollider.enabled = false;
        //OnInputComplete();
    }

    public void Input(GameObject obj)
    {
        //if(this.lineRenderer.gameObject.activeInHierarchy == false)
        //    this.lineRenderer.gameObject.SetActive(true);
        if(userInput.Count <=0)
        {
            userInput.Add(obj);
            lineRenderer.positionCount = userInput.Count;

            lineRenderer.SetPosition(userInput.Count - 1, obj.transform.position);
            return;
        }

        if (userInput.Find(o => obj.Equals(o)) == null)
        {
            var lastInputObject = userInput.Last();
            var lastInputIndex = this.PointList.FindIndex(o=>lastInputObject.Equals(o.obj));
            var currentObjectIndex = this.PointList.FindIndex(o => obj.Equals(o.obj));

            List<int> betweenIndexList = GetBetweenObjectList(lastInputIndex, currentObjectIndex);
            foreach(int idx in betweenIndexList)
            {
                if (userInput.Find(o => this.PointList[idx].obj.Equals(o)) != null)
                    continue;

                userInput.Add(this.PointList[idx].obj);
                lineRenderer.positionCount = userInput.Count;
                lineRenderer.SetPosition(userInput.Count - 1, this.PointList[idx].obj.transform.position);
            }

            userInput.Add(obj);
            lineRenderer.positionCount = userInput.Count;
            
            lineRenderer.SetPosition(userInput.Count-1, obj.transform.position);
        }
    }
    private List<int> GetBetweenObjectList(int beginIndex,int endIndex)
    {
        List<int> retList = new List<int>();

        Vector2Int begin = new Vector2Int(beginIndex % this.dotPerLine, (int)beginIndex / (int)this.dotPerLine);
        Vector2Int end = new Vector2Int(endIndex % this.dotPerLine, (int)endIndex / (int)this.dotPerLine);

        Vector2Int diff = end - begin;
        if (diff.x == 0 && diff.y == 0)//같은 위치(나올리 없음)
        {
            
        }
        else if (diff.x == 0 && diff.y != 0) // 세로로 중간값 체크
        {
            int moveValue = diff.y > 0 ? 1 : -1;
            int temp = begin.y;
            while(true)
            {
                temp += moveValue;
                if (temp == end.y)
                {
                    break;
                }
                int index = temp * this.dotPerLine + begin.x;
                retList.Add(index);
            }
        }
        else if(diff.x!=0 && diff.y == 0) // 가로로 중간값 체크
        {
            int moveValue = diff.x > 0 ? 1 : -1;
            int temp = begin.x;
            while (true)
            {
                temp += moveValue;
                if (temp == end.x)
                {
                    break;
                }
                int index = begin.y * this.dotPerLine + temp;
                retList.Add(index);
            }
        }
        else
        {
            if(Mathf.Abs(diff.x) == Mathf.Abs(diff.y)) // 대각선 중간값 체크
            {
                int moveXValue = diff.x > 0 ? 1 : -1;
                int moveYValue = diff.y > 0 ? 1 : -1;
                int tempX = begin.x;
                int tempY = begin.y;
                while(true)
                {
                    tempX += moveXValue;
                    tempY += moveYValue;
                    if(end.x==tempX && end.y==tempY)
                    {
                        break;
                    }
                    int index = tempY * this.dotPerLine + tempX;
                    retList.Add(index);
                }
            }
        }

        return retList;
    }
    public void OnInputComplete()
    {
        string inputString = string.Empty;
        if (userInput.Count <= 0)
            return;

        foreach(var obj in userInput)
        {
            var inputPoint = this.PointList.Find(point => point.obj.Equals(obj));
            inputString += inputPoint.value;
        }
        if (GameManager.instance != null)
        {
            GameManager.instance.SetPatten(inputString);
        }
        this.lineRenderer.positionCount = 0;
        //this.lineRenderer.gameObject.SetActive(false);
        userInput.Clear();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        inputCollider.enabled = false;
        OnInputComplete();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, MainUI.Instance.GetUICamera(), out localPos);
        this.inputRectTransform.localPosition = localPos;

        inputCollider.enabled = true;
        userInput.Clear();
    }
}
