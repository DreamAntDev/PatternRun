using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainUI : MonoBehaviour
{
    private static MainUI instance;
    public static MainUI Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("MainUI가 Scene에 없습니다.");
            }
            return instance;
        }
    }

    public GameObject titleObject;
    public GameObject InGameObject;

    public InteractiveMessageBox interactiveMessageBox;
    public SystemMessage systemMessage;
    public InputPad inputPad;

    public Button optionButton;
    public GameObject optionPopup;

    private Camera UICamera;
    List<Vector3> userInput = new List<Vector3>();

    private void Awake()
    {
        if(MainUI.instance == null)
        {
            MainUI.instance = this;
            this.UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    private void Start()
    {
        this.optionButton.onClick.AddListener(()=>this.optionPopup.SetActive(true));
        OnTitle();
    }

    public void OnTitle()
    {
        this.titleObject.SetActive(true);
        this.InGameObject.SetActive(false);
        this.interactiveMessageBox.SetText("Touch To Start", true);
        this.interactiveMessageBox.ActiveButton(true, () => OnGameStart());
        this.systemMessage.gameObject.SetActive(false);
    }
    public void OnGameStart()
    {
        this.titleObject.SetActive(false);
        this.InGameObject.SetActive(true);
        this.interactiveMessageBox.SetText("", false);
        //this.interactiveMessageBox.ActiveButton(false);
        this.interactiveMessageBox.ActiveButton(true, () => OnGameEnd());
    }

    public void OnGameEnd()
    {
        this.systemMessage.SetMessage("YOU DIED", "111111m", 5);
    }

    //최적화 예정
    public void AddInputPos(Vector3 pos)
    {
        userInput.Add(pos);
        //lineRenderer.SetPositions(userInput.ToArray());
    }
    public void ResetInput()
    {
        userInput.Clear();
        //lineRenderer.SetPositions(userInput.ToArray());
    }
    public void UpdateLastInput()
    {
        
    }

    public Camera GetUICamera()
    {
        return this.UICamera;
    }

    public void OnGetItem(Vector3 worldPos)
    {

    }
}
