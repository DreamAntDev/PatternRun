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

    float skillEventValue = 0;
    private float scoreMeter = 0f;

    //Score
    private float previousMaxScore = 0f;
    private float totalScore = 0f;

    public float weight = 25f;

    public float eventValue = 0;


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
            if (item.trapCode != null && item.trapCode.Count > 0)
            {
                trapSimulation.SetTrap(item.trapCode.ToArray());
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Get_Item);
        }
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

    private void Update()
    {
        //if (isPlay)
        //{
        //    TestMeter();
        //}
    }

    public void TestMeter()
    {
        scoreMeter = player.transform.position.x - this.playerStartPosX;
        //scoreMeter += Time.deltaTime * player.speed * (1 + eventValue);
        //Sample
        if(scoreMeter >= 30)
        {
            weight = scoreMeter;
            float value = 0.05f;
            float speed = 0;
            Debug.Log(scoreMeter / 30);
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
    

    

}
