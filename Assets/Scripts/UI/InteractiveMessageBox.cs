using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractiveMessageBox : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;
    public Image icon;
    private Button button;

    private void Awake()
    {
        this.button = GetComponent<UnityEngine.UI.Button>();
    }

    public void ActiveButton(bool active, UnityEngine.Events.UnityAction func = null)
    {
        button.enabled = active;
        if (active == false)
        {
            button.onClick.RemoveAllListeners();
            return;
        }
        if(func != null)
        {
            button.onClick.AddListener(func);
        }
    }
    public void SetText(string text, bool activeIcon)
    {
        this.Text.text = text;
        this.icon.gameObject.SetActive(activeIcon);
    }
}
