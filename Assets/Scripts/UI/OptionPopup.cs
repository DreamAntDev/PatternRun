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

    public Toggle powerOverwhelmingToggle;

    // Start is called before the first frame update
    public void Initialize()
    {
        powerOverwhelmingToggle.gameObject.SetActive(false);
#if DEBUG
        powerOverwhelmingToggle.gameObject.SetActive(true);
        LoadOption(powerOverwhelmingToggle);
        PlayOption.PowerOverwhelming = powerOverwhelmingToggle.isOn;
        this.powerOverwhelmingToggle.onValueChanged.AddListener((bool b) => 
        {
            PlayOption.PowerOverwhelming = b;
            SaveOption(this.powerOverwhelmingToggle);
        });
#endif

        //Load
        if (HasSavedOption(this.inputPadColliderSizeSlider))
        {
            LoadOption(this.inputPadColliderSizeSlider);
        }
        if (this.inputPadColliderToggle)
        {
            LoadOption(this.inputPadColliderToggle);
        }

        //SetListner
        this.closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        this.inputPadColliderSizeSlider.onValueChanged.AddListener((float f)=>
        {
            SetInputColliderSize(f);
            SaveOption(this.inputPadColliderSizeSlider);

        });
        this.inputPadColliderToggle.onValueChanged.AddListener((bool b) =>
        {
            SetInputColliderDisplay(b);
            SaveOption(this.inputPadColliderToggle);
        });

        //applyOption
        SetInputColliderSize(this.inputPadColliderSizeSlider.value);
        SetInputColliderDisplay(this.inputPadColliderToggle.isOn);
    }

    void SetInputColliderSize(float value)
    {
        PlayOption.InputPadCollisionSize = 100 * value;
        inputPadColliderSizeText.text = ((int)(PlayOption.InputPadCollisionSize)).ToString();
        foreach (var obj in MainUI.Instance.inputPad.PointList)
        {
            //obj.obj.GetComponent<BoxCollider2D>().size = new Vector2(PlayOption.InputPadCollisionSize, PlayOption.InputPadCollisionSize);
            obj.obj.GetComponent<CircleCollider2D>().radius = PlayOption.InputPadCollisionSize;
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
            var radius = PlayOption.InputPadCollisionSize;
            Vector3[] drawPos = new Vector3[360];
            float x, y;
            for (int i = 0; i < 360; i++)
            {
                x = Mathf.Cos(Mathf.Deg2Rad * i) * radius;
                y = Mathf.Sin(Mathf.Deg2Rad * i) * radius;
                drawPos[i] = new Vector3(x, y, 0);
            }

            foreach (var obj in MainUI.Instance.inputPad.PointList)
            {
                var renderer = obj.obj.GetComponentInChildren<LineRenderer>(true);
                renderer.gameObject.SetActive(true);
                
                renderer.positionCount = 360;
                renderer.SetPositions(drawPos);

                //var xP = obj.obj.GetComponent<BoxCollider2D>().size.x / 2;
                //var yP = obj.obj.GetComponent<BoxCollider2D>().size.y / 2;
                //Vector3[] posArray = { new Vector3(-xP, xP, 0), new Vector3(xP, xP, 0), new Vector3(xP, -xP, 0), new Vector3(-xP, -xP, 0) };
                //renderer.SetPositions(posArray);
            }
        }
    }

    void SaveOption(Object optionObj)
    {
        if(optionObj is Toggle)
        {
            var toggle = optionObj as Toggle;
            PlayerPrefs.SetString(toggle.name, toggle.isOn.ToString());
        }
        else if(optionObj is Slider)
        {
            var slider = optionObj as Slider;
            PlayerPrefs.SetFloat(slider.name, slider.value);
        }
    }

    void LoadOption(Object optionObj)
    {
        if (optionObj is Toggle)
        {
            var toggle = optionObj as Toggle;
            string saveOption = PlayerPrefs.GetString(toggle.name, "false");
            toggle.isOn = bool.Parse(saveOption);
        }
        else if (optionObj is Slider)
        {
            var slider = optionObj as Slider;
            float val = PlayerPrefs.GetFloat(slider.name);
            slider.value = val;
        }
    }

    bool HasSavedOption(Object optionObj)
    {
        if (optionObj is Toggle)
        {
            var toggle = optionObj as Toggle;
            return PlayerPrefs.HasKey(toggle.name);
        }
        else if (optionObj is Slider)
        {
            var slider = optionObj as Slider;
            return PlayerPrefs.HasKey(slider.name);
        }
        return PlayerPrefs.HasKey(optionObj.name);
    }
}
