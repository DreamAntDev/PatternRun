using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if(MainUI.instance == null)
        {
            MainUI.instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    private void Start()
    {
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
}
