using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 세팅
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private float fadeVolume;

    [Header("오디오 소스")]
    [Tooltip("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;

    [Tooltip("SFX")]
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("BGM 페이드 비율")]
    [SerializeField][Range(0.1f, 1f)] private float fadeRatio;

    [Header("BGM")]
    [Tooltip("BGM 리스트")]
    [SerializeField] private List<AudioClip> clipList;



    private void Awake()
    {
        // 싱글톤 세팅
        //var obj = FindObjectsOfType<SoundManager>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            fadeVolume = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BGMChange(int clipIndex)
    {
        if (clipIndex < clipList.Count - 1 && clipIndex >= 0)
        {
            StartCoroutine(BGMChangerCoroutine(clipList[clipIndex]));
        }
        else
        {
            Debug.Log("잘못된 BGM 인덱스");
        }
    }

    IEnumerator BGMChangerCoroutine(AudioClip ac)
    {
        StopAllCoroutines();
        yield return StartCoroutine(BGMFadeOut());
        bgmAudioSource.clip = ac;
        bgmAudioSource.Play();
        bgmAudioSource.loop = true;
        yield return StartCoroutine(BGMFadeIn());

    }

    IEnumerator BGMFadeOut()
    {
        float tempValue = SettingManager.Instance.GetBGMFade();
        while (true)
        {
            if (tempValue <= 0.01f)
            {
                tempValue = 0.001f;
                SettingManager.Instance.SetBGMFade(tempValue);
                break;
            }
            tempValue = Mathf.Lerp(tempValue, 0.001f, fadeRatio);
            SettingManager.Instance.SetBGMFade(tempValue);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BGMFadeIn()
    {
        float tempValue = SettingManager.Instance.GetBGMFade();
        while (true)
        {
            if (tempValue >= 0.98f)
            {
                tempValue = 1f;
                SettingManager.Instance.SetBGMFade(tempValue);
                break;
            }
            tempValue = Mathf.Lerp(tempValue, 1f, fadeRatio);
            SettingManager.Instance.SetBGMFade(tempValue);
            yield return new WaitForSeconds(0.1f);
        }
    }
}