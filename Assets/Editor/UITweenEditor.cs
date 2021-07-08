using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UITween))]
public class UITweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawComponent();
    }
    private void DrawComponent()
    {
        UITween tween = target as UITween;
        AnimationCurve curveX = EditorGUILayout.CurveField("Animation Curve X", tween.animationTransformXCurve, GUILayout.Width(170f), GUILayout.Height(62f));
        AnimationCurve curveY = EditorGUILayout.CurveField("Animation Curve Y", tween.animationTransformYCurve, GUILayout.Width(170f), GUILayout.Height(62f));
        if (GUI.changed)
        {
            tween.animationTransformXCurve = curveX;
            tween.animationTransformYCurve = curveY;
        }
    }
}
