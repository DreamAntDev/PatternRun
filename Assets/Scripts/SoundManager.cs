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

    class SoundCache
    {
        public string path { get; private set; }
        public AudioClip clip;

        public SoundCache(string path, AudioClip clip)
        {
            this.path = path;
            this.clip = clip;
        }
    }


    public NData.Sound soundData;
    Dictionary<SoundType, SoundCache> soundDictionary = new Dictionary<SoundType, SoundCache>();

    //Dictionary<long, AudioSource> audioSourcePool = new Dictionary<long, AudioSource>();
    Dictionary<int, GameObject> audioSourcePool = new Dictionary<int, GameObject>();
    Dictionary<SoundLayer, int> layerHashDictionary = new Dictionary<SoundLayer, int>();
    public enum SoundType // 순서가 변경되면 안됩니다.(ScriptableObject에서 Serialize되어 있는 값)
    {
        BG_Base,
        BG_Lobby,
        Get_Item,
        On_Point,
        Touch_To_Start,
        Crash_Trap,
        Sword_Attack,
    }

    public enum SoundLayer
    {
        None,
        BGM,
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
            if (data.requirePreload == true)
            {
                soundDictionary.Add(data.type, new SoundCache(data.path, Resources.Load<AudioClip>(data.path)));
            }
            else
            {
                soundDictionary.Add(data.type, new SoundCache(data.path, null));
            }
        }
    }

    public void PlaySound(SoundType type, bool loop = false, SoundLayer layer = SoundLayer.None)
    {
        SoundCache soundCache;
        if (soundDictionary.TryGetValue(type, out soundCache) == true)
        {
            AudioClip clip = soundCache.clip;

            if (clip == null)
            {
                clip = Resources.Load<AudioClip>(soundCache.path);
                soundCache.clip = clip;
            }
            if (clip != null)
            {
                var tempObj = new GameObject();
                var audioSource = tempObj.AddComponent<AudioSource>();
                var hash = tempObj.GetHashCode();
                this.audioSourcePool.Add(hash, tempObj);
                if (loop == false)
                {
                    audioSource.PlayOneShot(clip);
                    StartCoroutine(DestroyAudioSource(clip.length, tempObj.GetHashCode()));
                }
                else
                {
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                if(layer != SoundLayer.None)
                {
                    int removeHash;
                    if (this.layerHashDictionary.TryGetValue(layer, out removeHash) == true)
                    {
                        RemoveSound(removeHash);
                        this.layerHashDictionary[layer] = hash;
                    }
                    else
                    {
                        this.layerHashDictionary.Add(layer, hash);
                    }
                }
            }
        }
    }

    private IEnumerator DestroyAudioSource(float time, int hash)
    {
        yield return new WaitForSeconds(time);
        //AudioSource audioSource;
        RemoveSound(hash);
    }

    private void RemoveSound(int hash)
    {
        GameObject audioSource;
        if (this.audioSourcePool.TryGetValue(hash, out audioSource) == true)
        {
            this.audioSourcePool.Remove(hash);
            Destroy(audioSource);
        }
    }
}
