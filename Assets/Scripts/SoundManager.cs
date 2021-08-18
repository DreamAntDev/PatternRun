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
    Dictionary<SoundType, string> soundDictionary = new Dictionary<SoundType, string>();

    Dictionary<long, AudioSource> audioSourcePool = new Dictionary<long, AudioSource>();
    
    public enum SoundType
    {
        BG_Base,
        Get_Item,
        On_Point,
    }
    private void Awake()
    {
        if (SoundManager.instance != null)
        {
            Destroy(this);
            return;
        }

        SoundManager.instance = this;
        foreach(var data in soundData.soundList)
        {
            soundDictionary.Add(data.type, data.path);
        }
    }

    public void PlaySound(SoundType type, bool loop)
    {
        string path = string.Empty;
        if (soundDictionary.TryGetValue(type, out path) == false)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                var audioSource = new AudioSource();
                this.audioSourcePool.Add(audioSource.GetHashCode(), audioSource);
                if (loop == false)
                {
                    audioSource.PlayOneShot(clip);
                    StartCoroutine(DestroyAudioSource(clip.length, audioSource.GetHashCode()));
                }
                else
                {
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    private IEnumerator DestroyAudioSource(float time, int hash)
    {
        yield return new WaitForSeconds(time);
        AudioSource audioSource;
        if(this.audioSourcePool.TryGetValue(hash, out audioSource) == true)
        {
            this.audioSourcePool.Remove(hash);
            Destroy(audioSource);
        }
    }
}
