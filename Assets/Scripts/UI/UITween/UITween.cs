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

    public Vector3 beginPos;
    public Vector3 endPos;
    [HideInInspector]
    public AnimationCurve animationTransformXCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    [HideInInspector]
    public AnimationCurve animationTransformYCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    private Animation anim;
    private void Awake()
    {
        anim = this.gameObject.AddComponent<Animation>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var clip = new AnimationClip();
        clip.legacy = true;
        Vector3 diff = endPos - beginPos;
        SetRectTransformCurve(clip, "m_AnchoredPosition.x", this.animationTransformXCurve,diff.x);
        SetRectTransformCurve(clip, "m_AnchoredPosition.y", this.animationTransformYCurve,diff.y);

        clip.wrapMode = wrapMode;

        anim.AddClip(clip, clip.name);
        if(autoStart == true)
            anim.Play(clip.name);
    }

    private void SetRectTransformCurve(AnimationClip clip, string componentName, AnimationCurve inputCurve, float factor)
    {
        AnimationCurve newCurve = new AnimationCurve();
        foreach (var keyframe in inputCurve.keys)
        {
            newCurve.AddKey(new Keyframe(keyframe.time, keyframe.value * factor, keyframe.inTangent, keyframe.outTangent));
        }
        clip.SetCurve("", typeof(RectTransform), componentName, newCurve);
    }

    public void StartAnim()
    {
        anim.Play();
    }
}
