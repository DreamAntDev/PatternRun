using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionPopup : MonoBehaviour
{

    public Button closeButton;
    public Slider inputPadColliderSizeSlider;
    public Text inputPadColliderSizeText;
    // Start is called before the first frame update
    void Start()
    {
        this.closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        this.inputPadColliderSizeSlider.onValueChanged.AddListener(SetInputColliderSize);
    }

    void SetInputColliderSize(float value)
    {
        PlayOption.InputPadCollisionSize = 200 * value;
        inputPadColliderSizeText.text = ((int)(PlayOption.InputPadCollisionSize)).ToString();
        MainUI.Instance.inputPad.SetInputCollisionSize(PlayOption.InputPadCollisionSize);
    }
}
