using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InGameScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    public void SetScore(float score)
    {
        text.SetText(string.Format("{0} m", score));
    }

}
