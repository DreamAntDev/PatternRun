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

    [Header("- Player")]
    /*[SerializeField] */CommandInventory _CommandInventory;
    [SerializeField] int continueCount;
    [SerializeField] GameObject continuePopup;
    //[SerializeField] GameObject[] items;

    [Header("- Trap")]
    [SerializeField] protected TrapSimulation trapSimulation;

    private int _RemainContinueCount;
    private bool _IsTouch = false;
    private List<string> _ChainList;
    private string _ChainText;
    public bool isPlay = false;

    float _SkillEventValue = 0;
    private float _ScoreMeter = 0f;

    //Score
    private float _PreviousMaxScore = 0f;
    private float _TotalScore = 0f;

    public float weight = 25f;

    public float eventValue = 0;

    public bool isStop = false;
    private bool _IsTutorial = false;
    private string _CurrentPatten;
    private string _TutorialItemName;

    private Action<String> _StarttutorialLineAction;
    private Action _EndLineAction;


    private float _PlayerStartPosX;
    Coroutine _PlayerScoreCoroutine = null;
    private void Awake()
    {
        instance = this;

       //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        _PreviousMaxScore = PlayerPrefs.GetFloat("MaxScore", 0f);
        _TotalScore = PlayerPrefs.GetFloat("TotalScore", 0f);

        Init();
        SoundManager.Instance.PlaySound(SoundManager.SoundType.BG_Lobby, true,SoundManager.SoundLayer.BGM);
    }

    public void Init()
    {
        var title = MainUI.Instance.title;
        title.SetBestScore(_PreviousMaxScore);
        title.SetTotalScore(_TotalScore);
        if(this._CommandInventory == null)
        {
            var obj = new GameObject("CommandInventory");
            var inventory = obj.AddComponent<CommandInventory>();
            this._CommandInventory = inventory;
        }
        this._RemainContinueCount = this.continueCount;
    }


    public void TutorialLineAction(Action<String> action, Action action2)
    {
        _StarttutorialLineAction = action;
        _EndLineAction = action2;

    }

    public void SetPatten(string state)
    {
        var item = this._CommandInventory.GetCommandItem(state);

        if (item == null)
            return;

        _CurrentPatten = item.actionName;
        if (_IsTutorial && (!_TutorialItemName.Equals(_CurrentPatten)))
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
            this._CommandInventory.UseItem(item);
            if(string.IsNullOrEmpty(item.equipName) == false)
            {
                this.player.Equip(item.equipName);
                // 장착아이템 다 쓴 경우
                if(this._CommandInventory.isEnableItem(item) == false)
                {
                    this.player.UnEquipOnAnimEnd(item.equipName);
                }
            }
        }
    }

    public bool PlayerCollision(TrapTrigger trap)
    {
        var findItem = this._CommandInventory.GetAutoActiveItem("Collision");
        if (findItem == null)
            return false;

        switch (findItem.actionName)
        {
            case "Guard":
                {
                    this._CommandInventory.UseItem(findItem);
                    if (string.IsNullOrEmpty(findItem.equipName) == false)
                    {
                        this.player.Equip(findItem.equipName);
                        // 장착아이템 다 쓴 경우
                        if (this._CommandInventory.isEnableItem(findItem) == false)
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
        bool success = this._CommandInventory.AddItem(item);
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
        _IsTutorial = true;
        Stop();
        _StarttutorialLineAction(item.name);
        trapSimulation.TutorialTrap(item.name);
        _CurrentPatten = string.Empty;
        _TutorialItemName = item.actionName;
        while (!_TutorialItemName.Equals(_CurrentPatten))
        {
            yield return new WaitForSeconds(0.1f);
        }

        _IsTutorial = false;
        PlayerPrefs.SetInt("Tutorial_" + item.name, 1);
        _EndLineAction();
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
        StopCoroutine(_PlayerScoreCoroutine);
        isPlay = false;
        player.Die();
        Stop();
        MainUI.Instance.systemMessage.SetMessage("YOU DIED", string.Format("{0}m", _ScoreMeter), 5, ShowContinuePopup);
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
        continuePopup.Initialize(this._RemainContinueCount);
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
        if (this._RemainContinueCount > 0)
        {
            _RemainContinueCount--;
            AdsManager.instance.AdsShow();
        }
    }
    public void ContinueAdComplete()
    {
        isPlay = true;
        this.player.ContinueRun();
        this.Run();
        this._PlayerScoreCoroutine = StartCoroutine(ScoreUpdateCoroutine());
    }
    public void ReStart()
    {
        MainUI.Instance.OnGameEnd();
        ScroeTransaction();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ScroeTransaction()
    {
        if(_ScoreMeter > _PreviousMaxScore)
        {
            PlayerPrefs.SetFloat("MaxScore", _ScoreMeter);
        }

        PlayerPrefs.SetFloat("TotalScore", _TotalScore + _ScoreMeter);
    }

    public void GameStart()
    {
        if (!isPlay)
        {
            isPlay = true;
            player.StartMove();
            trapSimulation.OnSimulation();
            this._PlayerStartPosX = player.transform.position.x;
            this._PlayerScoreCoroutine = StartCoroutine(ScoreUpdateCoroutine());
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

        _ScoreMeter = player.transform.position.x - this._PlayerStartPosX;
        //scoreMeter += Time.deltaTime * player.speed * (1 + eventValue);
        //Sample
        if(_ScoreMeter >= 30)
        {
            weight = _ScoreMeter;
            float value = 0.05f;
            float speed = 0;
            for(int i = 0; i < (int)(_ScoreMeter / 30); i++)
            {
                speed += value;
            }
            player.speed = 1+speed;
        }
        MainUI.Instance.inGameScore.SetScore(_ScoreMeter);
    }

    public float GetMeter()
    {
        return _ScoreMeter;
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
