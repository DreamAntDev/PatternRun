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

    public float weight = 25f;


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

        bool useSuccess = true;
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
                if (player.IsJumping() == false)
                {
                    player.Jump();
                    MainUI.Instance.OnJump();
                    Debug.Log("Jump Animation");
                }
                break;

            case "attack":
                // Attack
                player.Attack();
                MainUI.Instance.OnAttack();
                break;
            case "arrowShot":
                player.ArrowShot();
                break;
            default:
                useSuccess = false;
                break;
        }

        if (useSuccess)
        {
            this.commandInventory.UseItem(item);
            if(string.IsNullOrEmpty(item.equipName) == false)
            {
                // 장착아이템 다 쓴 경우
                if(this.commandInventory.isEnableItem(item) == false)
                {
                    this.player.UnEquip(item.equipName);
                }
            }
        }
    }

    public void GetCommandItem(NData.Item item,Vector3 worldPos)
    {
        bool success = this.commandInventory.AddItem(item);
        if (success)
        {
            Debug.Log("Item Success: " + item.name);
            if (string.IsNullOrEmpty(item.equipName) == false)
            {
                GetPlayerItem(item.equipName);
            }
            if (item.trapCode != null && item.trapCode.Count > 0)
            {
                trapSimulation.SetTrap(item.trapCode.ToArray());
            }
        }
    }

    public void GetPlayerItem(string equipName)
    {
        player.Equip(equipName);
    }

    public void GameEnd()
    {
        isPlay = false;
        player.Stop();
        MainUI.Instance.systemMessage.SetMessage("YOU DIED", string.Format("{0}m", scoreMeter), 5, ReStart);
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
        //Sample
        if(scoreMeter >= 30)
        {
            weight = scoreMeter;
        }
        MainUI.Instance.inGameScore.SetScore(scoreMeter);
    }

    public Player GetPlayer()
    {
        return player;
    }
    

    

}
