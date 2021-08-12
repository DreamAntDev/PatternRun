using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("SoundManager가 Scene에 없습니다.");
            }
            return instance;
        }
    }

    public NData.Sound soundData;
    AudioSource audioSource;
    AudioSource bgAudioSource;

    Dictionary<SoundType, string> soundDictionary = new Dictionary<SoundType, string>();
    public enum SoundType
    {
        BG_Base,
        Get_Item,
        On_Point,
    }
    private void Awake()
    {
        if (SoundManager.instance != null)
            return;

        SoundManager.instance = this;
        foreach(var data in soundData.soundList)
        {
            soundDictionary.Add(data.type, data.path);
        }
        this.audioSource = new AudioSource();
        this.bgAudioSource = new AudioSource();
    }

    public void PlaySound(SoundType type)
    {
        string path = string.Empty;
        if (soundDictionary.TryGetValue(type, out path) == false)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

    public void PlayBGSound(SoundType type)
    {
        string path = string.Empty;
        if (soundDictionary.TryGetValue(type, out path) == false)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                bgAudioSource.clip = clip;
                bgAudioSource.loop = true;
                bgAudioSource.Play();
            }
        }
    }
}
