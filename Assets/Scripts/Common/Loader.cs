using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public List<GameObject> loadObjectList = new List<GameObject>();

    private void Awake()
    {
        SetCameraSize();
        foreach (var obj in loadObjectList)
        {
            var newObj = Instantiate(obj);
        }
        SetUISize();
    }

    private void SetCameraSize()
    {
        if (Camera.main.targetTexture != null)
            Camera.main.targetTexture.Release();

        Camera.main.targetTexture = new RenderTexture(Screen.width, (int)(Screen.height * 0.6f),16,RenderTextureFormat.ARGB32);
        Camera.main.targetTexture.Create();
    }
    private void SetUISize()
    {
        MainUI.Instance.SetMainCameraTexture();
    }
}
