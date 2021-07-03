using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UITween : MonoBehaviour
{
    [SerializeField]
    private bool autoStart = false;
    [SerializeField]
    private UnityEngine.WrapMode wrapMode = WrapMode.Default;

    //public Vector3 beginPos;
    public Vector3 endPos;
    public float playTime = 1.0f;
    [HideInInspector]
    public AnimationCurve animationTransformXCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    [HideInInspector]
    public AnimationCurve animationTransformYCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    private Animation anim;
    private System.Action endAction;
    private void Awake()
    {
        anim = this.gameObject.AddComponent<Animation>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(autoStart)
            CreateClip();
    }

    private void SetRectTransformCurve(AnimationClip clip, string componentName, AnimationCurve inputCurve, float begin, float factor)
    {
        AnimationCurve newCurve = new AnimationCurve();
        foreach (var keyframe in inputCurve.keys)
        {
            newCurve.AddKey(new Keyframe(keyframe.time*this.playTime, keyframe.value * factor + begin, keyframe.inTangent, keyframe.outTangent));
        }
        clip.SetCurve("", typeof(RectTransform), componentName, newCurve);
    }

    public void StartAnim()
    {
        anim.Play();
    }

    public void CreateClip()
    {
        if(anim.GetClip("UITween") !=null)
        {
            anim.RemoveClip("UITween");
        }   

        var clip = new AnimationClip();
        clip.legacy = true;
        Vector3 thisPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        SetRectTransformCurve(clip, "m_AnchoredPosition.x", this.animationTransformXCurve, thisPos.x, endPos.x- thisPos.x);
        SetRectTransformCurve(clip, "m_AnchoredPosition.y", this.animationTransformYCurve, thisPos.y, endPos.y- thisPos.y);

        clip.wrapMode = wrapMode;

        anim.AddClip(clip, "UITween");

        if (autoStart == true)
            AnimPlay();
    }
    public void AnimPlay(System.Action onEndEvent = null)
    {
        var clip = anim.GetClip("UITween");
        var endEvent = new AnimationEvent();
        endEvent.time = this.playTime;
        endEvent.functionName = "OnAnimEnd";
        this.endAction = onEndEvent;
        clip.AddEvent(endEvent);
        anim.Play("UITween");
    }

    public void OnAnimEnd()
    {
        if(this.endAction != null)
        {
            this.endAction();
        }
    }
}
