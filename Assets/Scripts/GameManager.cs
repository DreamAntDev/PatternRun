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
        MoveAnimation(chainText);
        isTouch = false;
        chainList.Clear();
    }

    public void MoveAnimation(string state)
    {
        switch (state)
        {
            case "AB":
            case "DC":
                // Dash
                player.Dash();
                Debug.Log("Dash Animation");
                break;

            case "DA":
            case "CB":
                // Jump
                player.Jump();
                Debug.Log("Jump Animation");
                break;

            case "ABCD":
            case "BCDA":
            case "CDAB":
            case "DCBA":
                // Rolling
                player.Rolling();
                Debug.Log("Rolling Animation");
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
