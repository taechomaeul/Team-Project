using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    List<Resolution> resolutions = new();

    [Header("설정")]
    [Tooltip("오디오 믹서")]
    public AudioMixer audioMixer;

    [Header("값 설정 UI")]
    [Tooltip("BGM 볼륨")]
    public Slider bgmSlider;

    [Tooltip("SFX 볼륨")]
    public Slider sfxSlider;

    private void Awake()
    {
        // DontDestroyOnLoad
        var obj = FindObjectsOfType<SettingManager>();
        if (obj.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            LoadSettings();
            InitSettings();
    }
}

    void InitSettings()
    {
        resolutions.AddRange(Screen.resolutions);
        for(int i =0;i< resolutions.Count;i++)
        {
            Debug.Log($"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRate}hz");
        }
    }


    /// <summary>
    /// 최초 실행시 설정 불러오기
    /// </summary>
    void LoadSettings()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 1);
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(bgmSlider.value) * 20);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(sfxSlider.value) * 20);
    }

    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void SetBgmVolume()
    {
        float volume;
        if (bgmSlider.value != 0)
        {
            volume = bgmSlider.value;
        }
        else
        {
            volume = 0.0001f;
        }
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("bgmVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// SFX 볼륨 설정
    /// </summary>
    public void SetSfxVolume()
    {
        float volume;
        if (sfxSlider.value != 0)
        {
            volume = sfxSlider.value;
        }
        else
        {
            volume = 0.0001f;
        }
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

}
