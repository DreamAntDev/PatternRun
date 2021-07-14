using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI totalScore;

    public void SetBestScore(float score)
    {
        this.bestScore.SetText(string.Format("{0} m", score));
    }
    public void SetTotalScore(float score)
    {
        this.totalScore.SetText(string.Format("{0} m", score));
    }
}
