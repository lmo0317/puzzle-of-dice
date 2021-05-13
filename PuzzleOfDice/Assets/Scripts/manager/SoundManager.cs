using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager g_Instance;
    public AudioClip[] bgmClipList;
    public AudioClip[] effectClipList;
    public AudioSource bgmSource;

    void Start()
    {
        g_Instance = this;
        ChangeMusicState();
    }

    void Update()
    {

    }

    public void PlayEffectSound(string strName)
    {
        if (CMainData.Sound)
        {
            for (int i = 0; i < effectClipList.Length; ++i)
            {
                if (effectClipList[i] && effectClipList[i].name == strName)
                {
                    //audio.loop = true;
                    //audio.volume = 0.01f;
                    audio.PlayOneShot(effectClipList[i]);
                }
            }
        }
    }

    public void PlayBGM(string strName, bool bLoop)
    {
        for (int i = 0; i < bgmClipList.Length; ++i)
        {
            if (bgmClipList[i] && bgmClipList[i].name == strName)
            {
                bgmSource.clip = bgmClipList[i];
                bgmSource.loop = bLoop;
                if (CMainData.Music)
                {
                    bgmSource.Play();
                }
            }
        }
    }

    public bool IsPlayingBGM()
    {
        return bgmSource.isPlaying;
    }

    public void ChangeMusicState()
    {
        if (!CMainData.Music)// && IsPlayingBGM())
        {
            bgmSource.Stop();
        }
        else if (!IsPlayingBGM())
        {
            bgmSource.Play();
        }
    }
}
