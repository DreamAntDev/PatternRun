using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionPopup : MonoBehaviour
{

    public Button closeButton;
    public Slider inputPadColliderSizeSlider;
    public Text inputPadColliderSizeText;

    public Toggle inputPadColliderToggle;
    // Start is called before the first frame update
    void Start()
    {
        this.closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        this.inputPadColliderSizeSlider.onValueChanged.AddListener(SetInputColliderSize);
        this.inputPadColliderToggle.onValueChanged.AddListener(SetInputColliderDisplay);
    }

    void SetInputColliderSize(float value)
    {
        PlayOption.InputPadCollisionSize = 200 * value;
        inputPadColliderSizeText.text = ((int)(PlayOption.InputPadCollisionSize)).ToString();
        foreach (var obj in MainUI.Instance.inputPad.PointList)
        {
            obj.obj.GetComponent<BoxCollider2D>().size = new Vector2(PlayOption.InputPadCollisionSize, PlayOption.InputPadCollisionSize);
        }
        DisplayInputPadCollider();
    }
    void SetInputColliderDisplay(bool active)
    {
        PlayOption.InputPadCollisionDisplay = active;
        DisplayInputPadCollider();
    }
    void DisplayInputPadCollider()
    {
        if (PlayOption.InputPadCollisionDisplay == false)
        {
            foreach (var obj in MainUI.Instance.inputPad.PointList)
            {
                var renderer = obj.obj.GetComponentInChildren<LineRenderer>(true);
                renderer.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var obj in MainUI.Instance.inputPad.PointList)
            {
                var renderer = obj.obj.GetComponentInChildren<LineRenderer>(true);
                renderer.gameObject.SetActive(true);
                var xP = obj.obj.GetComponent<BoxCollider2D>().size.x / 2;
                var yP = obj.obj.GetComponent<BoxCollider2D>().size.y / 2;
                Vector3[] posArray = { new Vector3(-xP, xP, 0), new Vector3(xP, xP, 0), new Vector3(xP, -xP, 0), new Vector3(-xP, -xP, 0) };
                renderer.SetPositions(posArray);
            }
        }
    }
}
