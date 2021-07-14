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
        chainText = string.Empty;
    }
    private void Start()
    {
        chainList = new List<string>();

        previousMaxScore = PlayerPrefs.GetFloat("MaxScore", 0f);
        totalScore = PlayerPrefs.GetFloat("TotalScore", 0f);

        Init();
    }

    public void Init()
    {
        bestScoreText.text = previousMaxScore.ToString();
        totalScoreText.text = totalScore.ToString();
    }

    public void Reset()
    {
        var distinctList = chainList.Distinct();

        foreach (var x in distinctList)
        {
            chainText += x;
        }
        SetPatten(chainText);
        isTouch = false;
        chainList.Clear();
    }

    public void SetPatten(string state)
    {
        Debug.Log(state);
        switch (state)
        {
            case "ABC":
            case "DEF":
            case "GHI":
                // Dash
                player.Dash();
                Debug.Log("Dash Animation");
                break;

            case "ADG":
            case "BEH":
            case "CFI":
                player.Sit();
                Debug.Log("Sit Animation");
                break;

            case "GDA":
            case "HEB":
            case "IFC":
                // Jump
                player.Jump();
                Debug.Log("Jump Animation");
                break;

            case "AEI":
                // Attack
                player.Attack();
                break;
        }
        chainText = string.Empty;
    }

    public void ChainTouch(string currentString)
    {
        if (isTouch)
        {
            chainList.Add(currentString);
        }
    }

    public void GameEnd()
    {
        gameStart = false;
        deadScoreText.text = scoreMeter.ToString();
        player.Stop();
        deadUI.SetActive(true);
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

    public void InputStart()
    {
        isTouch = true;
        Debug.Log("Input Start --------------------");
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
        scoreText.text = String.Format("{0} m", scoreMeter.ToString());
    }
    

    

}
