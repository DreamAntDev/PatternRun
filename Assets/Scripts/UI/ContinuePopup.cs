using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinuePopup : MonoBehaviour
{
    public TextMeshProUGUI remainCountText;
    public Button continueButton;
    public TextMeshProUGUI continueButtonText;
    public Button LobbyButton;
    public TextMeshProUGUI LobbyButtonText;
    public NData.Language language;
    // Start is called before the first frame update
    void Start()
    {
        var camera = GameObject.Find("UICamera").GetComponent<Camera>();
        this.GetComponent<Canvas>().worldCamera = camera;
    }

    public void Initialize(int remainCount)
    {
        if (remainCount <= 0)
        {
            this.continueButton.gameObject.SetActive(false);
        }
        remainCountText.SetText(string.Format(language.GetLanguage("ContinuePopup_RemainCount"), remainCount));
        continueButtonText.SetText(language.GetLanguage("ContinuePopup_Continue"));
        LobbyButtonText.SetText(language.GetLanguage("ContinuePopup_Lobby"));
    }
}
