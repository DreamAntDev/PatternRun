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

    public Title title;
    public InGameScore inGameScore;

    public InteractiveMessageBox interactiveMessageBox;
    public SystemMessage systemMessage;
    public InputPad inputPad;
    public UserCommand userCommand;

    public Button optionButton;
    public GameObject optionPopup;

    public UnityEngine.U2D.SpriteAtlas iconAtlas;

    private Camera UICamera;

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
        this.title.gameObject.SetActive(true);
        this.inGameScore.gameObject.SetActive(false);
        this.interactiveMessageBox.SetText("Touch To Start", true);
        this.interactiveMessageBox.ActiveButton(true, () => OnGameStart());
        this.systemMessage.gameObject.SetActive(false);
    }
    public void OnGameStart()
    {
        this.title.gameObject.SetActive(false);
        this.inGameScore.gameObject.SetActive(true);
        this.interactiveMessageBox.SetText("", false);
        //this.interactiveMessageBox.ActiveButton(false);
        
        //MainUI.instance.OnGetItem(this.transform.position,"magic","pattern");
    }

    public void OnGameEnd()
    {
        this.interactiveMessageBox.SetText("Touch To ReStart", true);
        this.interactiveMessageBox.ActiveButton(true, () => GameManager.instance.ReStart());
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
