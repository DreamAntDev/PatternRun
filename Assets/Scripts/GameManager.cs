using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

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
    [SerializeField] GameObject[] items;

    [Header("- Trap")]
    [SerializeField] protected TrapSimulation trapSimulation;

    private bool isTouch = false;
    private List<string> chainList;
    private string chainText;
    public bool isPlay = false;


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

    public void ShowAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void SetPatten(string state)
    {
        var item = this.commandInventory.GetCommandItem(state);
        if (item == null)
            return;

        switch (item.actionName)
        {
            case "dash":
                // Dash
                player.Dash();
                Debug.Log("Dash Animation");
                break;

            case "sit":
                player.Sit();
                MainUI.Instance.OnSit();
                Debug.Log("Sit Animation");
                break;

            case "jump":
                // Jump
                player.Jump();
                MainUI.Instance.OnJump();
                Debug.Log("Jump Animation");
                break;

            case "attack":
                // Attack
                player.Attack();
                MainUI.Instance.OnAttack();
                break;
        }
    }

    public void GetCommandItem(NData.Item item,Vector3 worldPos)
    {
        bool success = this.commandInventory.AddItem(item);
        if (success)
        {
            GetPlayerItem(item.iconName);
            trapSimulation.GetItem(item.iconName);
        }
    }

    public void GetPlayerItem(string iconName)
    {
        switch (iconName)
        {
            case "attack":
                items[0].SetActive(true);
                break;
            case "shield":
                items[1].SetActive(true);
                break;
        }
    }

    public void GameEnd()
    {
        isPlay = false;
        player.Stop();
        MainUI.Instance.systemMessage.SetMessage("YOU DIED", string.Format("{0}m", scoreMeter), 5);
        MainUI.Instance.OnGameEnd();
        ScroeTransaction();
    }

    public void ReStart()
    {
        AdsManager.instance.AdsShow();
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
        if (!isPlay)
        {
            isPlay = true;
            player.StartMove();
            trapSimulation.OnSimulation();
        }

    }


    private void Update()
    {
        if (isPlay)
        {
            TestMeter();
        }
    }

    public void TestMeter()
    {
        scoreMeter += Time.deltaTime;
        MainUI.Instance.inGameScore.SetScore(scoreMeter);
    }

    public Player GetPlayer()
    {
        return player;
    }
    

    

}
