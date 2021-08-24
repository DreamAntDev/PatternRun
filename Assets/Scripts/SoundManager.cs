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
                Debug.LogError("SoundManager�� Scene�� �����ϴ�.");
            }
            return instance;
        }
    }

    public NData.Sound soundData;
    Dictionary<SoundType, string> soundDictionary = new Dictionary<SoundType, string>();

    //Dictionary<long, AudioSource> audioSourcePool = new Dictionary<long, AudioSource>();
    Dictionary<int, GameObject> audioSourcePool = new Dictionary<int, GameObject>();
    Dictionary<SoundLayer, int> layerHashDictionary = new Dictionary<SoundLayer, int>();
    public enum SoundType // ������ ����Ǹ� �ȵ˴ϴ�.(ScriptableObject���� Serialize�Ǿ� �ִ� ��)
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
            soundDictionary.Add(data.type, data.path);
        }
    }

    public void PlaySound(SoundType type, bool loop = false, SoundLayer layer = SoundLayer.None)
    {
        string path = string.Empty;
        if (soundDictionary.TryGetValue(type, out path) == true)
        {
            var clip = Resources.Load<AudioClip>(path);
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
