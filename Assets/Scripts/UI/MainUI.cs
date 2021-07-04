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
                Debug.LogError("MainUI�� Scene�� �����ϴ�.");
            }
            return instance;
        }
    }

    public GameObject titleObject;
    public GameObject InGameObject;

    public InteractiveMessageBox interactiveMessageBox;
    public SystemMessage systemMessage;
    public InputPad inputPad;
    public UserCommand userCommand;

    public Button optionButton;
    public GameObject optionPopup;

    public UnityEngine.U2D.SpriteAtlas iconAtlas;

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
        //MainUI.instance.OnGetItem(this.transform.position,"magic","pattern");
    }

    public void OnGameEnd()
    {
        this.systemMessage.SetMessage("YOU DIED", "111111m", 5);
    }

    public Camera GetUICamera()
    {
        return this.UICamera;
    }

    public void OnGetItem(Vector3 worldPos, string iconName, string patternName)
    {
        this.userCommand.Insert(worldPos, iconName, patternName);
    }
}
