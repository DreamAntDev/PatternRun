using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public bool gameStart { get; set; }
    [SerializeField] Player player;


    //[Header("- UI Board")]
    //[SerializeField] protected TextMeshProUGUI scoreText;
    //[SerializeField] protected TextMeshProUGUI bestScoreText;
    //[SerializeField] protected TextMeshProUGUI totalScoreText;
    //[SerializeField] protected GameObject deadUI;
    //[SerializeField] protected TextMeshProUGUI deadScoreText;


    [Header("- Player")]
    /*[SerializeField] */CommandInventory commandInventory;
    [SerializeField] int continueCount;
    private int remainContinueCount;
    [SerializeField] GameObject continuePopup;
    //[SerializeField] GameObject[] items;

    [Header("- Trap")]
    [SerializeField] protected TrapSimulation trapSimulation;

    private bool isTouch = false;
    private List<string> chainList;
    private string chainText;
    public bool isPlay = false;

    float skillEventValue = 0;
    private float scoreMeter = 0f;

    //Score
    private float previousMaxScore = 0f;
    private float totalScore = 0f;

    public float weight = 25f;

    public float eventValue = 0;

    public bool isStop = false;
    private bool isTutorial = false;
    private string currentPatten;
    private string tutorialItemName;

    private Action<String> starttutorialLineAction;
    private Action endLineAction;


    private float playerStartPosX;
    Coroutine playerScoreCoroutine = null;
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
        SoundManager.Instance.PlaySound(SoundManager.SoundType.BG_Lobby, true,SoundManager.SoundLayer.BGM);
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
        this.remainContinueCount = this.continueCount;
    }

    public void ShowAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void TutorialLineAction(Action<String> action, Action action2)
    {
        starttutorialLineAction = action;
        endLineAction = action2;

    }

    public void SetPatten(string state)
    {
        var item = this.commandInventory.GetCommandItem(state);

        if (item == null)
            return;

        currentPatten = item.actionName;
        if (isTutorial && (!tutorialItemName.Equals(currentPatten)))
        {
            return;
        }

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
                this.player.Equip(item.equipName);
                // 장착아이템 다 쓴 경우
                if(this.commandInventory.isEnableItem(item) == false)
                {
                    this.player.UnEquipOnAnimEnd(item.equipName);
                }
            }
        }
    }

    public bool PlayerCollision(TrapTrigger trap)
    {
        var findItem = this.commandInventory.GetAutoActiveItem("Collision");
        if (findItem == null)
            return false;

        switch (findItem.actionName)
        {
            case "Guard":
                {
                    this.commandInventory.UseItem(findItem);
                    if (string.IsNullOrEmpty(findItem.equipName) == false)
                    {
                        this.player.Equip(findItem.equipName);
                        // 장착아이템 다 쓴 경우
                        if (this.commandInventory.isEnableItem(findItem) == false)
                        {
                            this.player.UnEquip(findItem.equipName);
                        }
                    }
                    return true;
                }
        }
        
        return false;
    }

    public void GetCommandItem(NData.Item item,Vector3 worldPos)
    {
        bool success = this.commandInventory.AddItem(item);
        if (success)
        {
            if (string.IsNullOrEmpty(item.equipName) == false)
            {
                GetPlayerItem(item.equipName);
            }
            if (item.trapCode != null && item.trapCode.Count > 0 )
            {
#if !UNITY_EDITOR
                if (!Convert.ToBoolean(PlayerPrefs.GetInt("Tutorial_"+item.name, 0)))
#endif
                    StartCoroutine(ItemCommandTutorial(item));
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Get_Item);
        }
    }

    IEnumerator ItemCommandTutorial(NData.Item item)
    {
        isTutorial = true;
        Stop();
        starttutorialLineAction(item.name);
        trapSimulation.TutorialTrap(item.name);
        currentPatten = string.Empty;
        tutorialItemName = item.actionName;
        while (!tutorialItemName.Equals(currentPatten))
        {
            yield return new WaitForSeconds(0.1f);
        }

        isTutorial = false;
        PlayerPrefs.SetInt("Tutorial_" + item.name, 1);
        endLineAction();
        Run();

        trapSimulation.SetTrap(item.trapCode.ToArray());
    }

    public void GetPlayerItem(string equipName)
    {
        player.Equip(equipName);
    }

    public void GameEnd()
    {
#if DEBUG
        if (PlayOption.PowerOverwhelming == true)
            return;
#endif
        StopCoroutine(playerScoreCoroutine);
        isPlay = false;
        player.Die();
        Stop();
        MainUI.Instance.systemMessage.SetMessage("YOU DIED", string.Format("{0}m", scoreMeter), 5, ShowContinuePopup);
        //MainUI.Instance.OnGameEnd();
    }

    public void Stop()
    {
        player.Stop();
        isStop = true;
    }

    public void Run()
    {
        player.StartMove();
        isStop = false;
    }

    public void ShowContinuePopup()
    {
        var obj = Instantiate(this.continuePopup);
        var continuePopup = obj.GetComponent<ContinuePopup>();
        continuePopup.Initialize(this.remainContinueCount);
        continuePopup.continueButton.onClick.AddListener(() =>
        {
            this.Continue();
            GameObject.Destroy(continuePopup.gameObject);
        });
        continuePopup.LobbyButton.onClick.AddListener(()=>
        {
            this.ReStart();
            GameObject.Destroy(continuePopup.gameObject);
        });
    }
    public void Continue()
    {
        if (this.remainContinueCount > 0)
        {
            remainContinueCount--;
            AdsManager.instance.AdsShow();
        }
    }
    public void ContinueAdComplete()
    {
        isPlay = true;
        this.player.ContinueRun();
        this.Run();
        this.playerScoreCoroutine = StartCoroutine(ScoreUpdateCoroutine());
    }
    public void ReStart()
    {
        MainUI.Instance.OnGameEnd();
        ScroeTransaction();
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
            this.playerStartPosX = player.transform.position.x;
            this.playerScoreCoroutine = StartCoroutine(ScoreUpdateCoroutine());
        }

    }

    IEnumerator ScoreUpdateCoroutine()
    {
        while(true)
        {
            TestMeter();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TestMeter()
    {
        if (isStop)
        {
            return;
        }

        scoreMeter = player.transform.position.x - this.playerStartPosX;
        //scoreMeter += Time.deltaTime * player.speed * (1 + eventValue);
        //Sample
        if(scoreMeter >= 30)
        {
            weight = scoreMeter;
            float value = 0.05f;
            float speed = 0;
            for(int i = 0; i < (int)(scoreMeter / 30); i++)
            {
                speed += value;
            }
            player.speed = 1+speed;
        }
        MainUI.Instance.inGameScore.SetScore(scoreMeter);
    }

    public float GetMeter()
    {
        return scoreMeter;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public float GetSpeed()
    {
        return player.speed;
    }
    

    

}
