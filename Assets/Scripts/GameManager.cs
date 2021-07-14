using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public bool gameStart { get; set; }
    [SerializeField] Player player;

    [Header("- UI Board")]
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected TextMeshProUGUI bestScoreText;
    [SerializeField] protected TextMeshProUGUI totalScoreText;
    [SerializeField] protected GameObject deadUI;
    [SerializeField] protected TextMeshProUGUI deadScoreText;


    [Header("- Player")]
    [SerializeField] CommandInventory commandInventory;

    private bool isTouch = false;
    private List<string> chainList;
    private string chainText;


    private float scoreMeter = 0f;

    //Score
    private float previousMaxScore = 0f;
    private float totalScore = 0f;



    private void Awake()
    {
        instance = this;

       //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        previousMaxScore = PlayerPrefs.GetFloat("MaxScore", 0f);
        totalScore = PlayerPrefs.GetFloat("TotalScore", 0f);

        Init();
    }

    public void Init()
    {
        var title = MainUI.Instance.title;
        title.SetBestScore(previousMaxScore);
        title.SetTotalScore(totalScore);
        if(this.commandInventory == null)
        {
            var obj = new GameObject("CommandInventory");
            var inventory = obj.AddComponent<CommandInventory>();
            this.commandInventory = inventory;
        }
    }

    public void SetPatten(string state)
    {
        var actionName = GetEnableActionName(state);
        if (string.IsNullOrEmpty(actionName) == true)
            return;

        switch (actionName)
        {
            case "dash":
                // Dash
                player.Dash();
                Debug.Log("Dash Animation");
                break;

            case "sit":
                player.Sit();
                Debug.Log("Sit Animation");
                break;

            case "jump":
                // Jump
                player.Jump();
                Debug.Log("Jump Animation");
                break;

            case "attack":
                // Attack
                player.Attack();
                break;
        }
    }

    private string GetEnableActionName(string inputCommand)
    {
        string actionName = string.Empty;
        foreach (var cmd in commandInventory.enableCommandList)
        {
            foreach (var usingCmd in cmd.usingCommand)
            {
                if (usingCmd.Equals(inputCommand) == true)
                {
                    actionName = cmd.actionName;
                    return actionName;
                }
            }
        }
        return actionName;
    }
    public void GetCommandItem(NData.Item item,Vector3 worldPos)
    {
        if(this.commandInventory.enableCommandList.Contains(item) == false)
            this.commandInventory.enableCommandList.Add(item);

        MainUI.Instance.OnGetItem(worldPos, item.iconName, item.patternName);
    }

    public void GameEnd()
    {
        gameStart = false;
        player.Stop();
        MainUI.Instance.systemMessage.SetMessage("YOU DIED", string.Format("{0}m", scoreMeter), 5);
        MainUI.Instance.OnGameEnd();
        ScroeTransaction();
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ScroeTransaction()
    {
        if(scoreMeter > previousMaxScore)
        {
            PlayerPrefs.SetFloat("MaxScore", scoreMeter);
        }

        PlayerPrefs.SetFloat("TotalScore", totalScore + scoreMeter);
    }

    public void GameStart()
    {
        gameStart = true;
        player.StartMove();
    }


    private void Update()
    {
        if (gameStart)
        {
            TestMeter();
        }
    }

    public void TestMeter()
    {
        scoreMeter += Time.deltaTime;
        MainUI.Instance.inGameScore.SetScore(scoreMeter);
    }
    

    

}
