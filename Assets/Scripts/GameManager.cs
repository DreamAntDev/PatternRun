using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Player player;

    private bool isTouch = false;

    private List<string> chainList;
    private string chainText;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        chainText = string.Empty;
    }
    private void Start()
    {
        chainList = new List<string>();
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

            case "GDA":
            case "HEB":
            case "IFC":
                // Jump
                player.Jump();
                Debug.Log("Jump Animation");
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
        player.Stop();
    }
    public void InputStart()
    {
        isTouch = true;
        Debug.Log("Input Start --------------------");
    }



}
