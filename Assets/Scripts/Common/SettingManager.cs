using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    // 사운드 관련
    private float bgmVolume;
    private float sfxVolume;

    // 해상도 관련    
    private List<Resolution> resolutions = new();
    private int resolutionIndex;
    private FullScreenMode fullScreenMode;

    // 변경 전 값
    private float originBgmValue;
    private float originSfxValue;
    private int originResolutionIndex;
    private bool originFullscreenMode;
    private float originBrightness;

    [Header("설정")]
    [Tooltip("오디오 믹서")]
    public AudioMixer audioMixer;

    [Header("값 설정 UI")]
    [Tooltip("BGM 볼륨")]
    public Slider bgmSlider;

    [Tooltip("SFX 볼륨")]
    public Slider sfxSlider;

    [Tooltip("해상도")]
    public Dropdown resolutionsDropdown;
    [Tooltip("전체화면")]
    public Toggle fullscreenToggle;

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
        }
    }

    #region 시스템
    /// <summary>
    /// 최초 실행시 설정 불러오기
    /// </summary>
    private void LoadSettings()
    {
        LoadSoundSetting();
        LoadResolutionSetting();
    }

    /// <summary>
    /// 설정 내용 적용하는 확인 버튼용
    /// </summary>
    public void Btn_SettingApply()
    {
        SaveResolutionSetting(resolutionsDropdown.value);
        SaveSoundSetting();
        Debug.Log("저장");
    }

    /// <summary>
    /// 환경 설정 화면 열 때 동작
    /// 원래 값들 저장
    /// </summary>
    public void Btn_SetOriginValues()
    {
        originBgmValue = bgmVolume;
        originSfxValue = sfxVolume;
        originResolutionIndex = resolutionsDropdown.value;
        originFullscreenMode = fullscreenToggle.isOn;
        Debug.Log(bgmVolume);
        Debug.Log("origin 저장");
    }

    /// <summary>
    /// 변경 적용하지 않고 취소할 때 UI 값들 다시 원위치
    /// </summary>
    public void Btn_UndoSetting()
    {
        // 사운드
        bgmSlider.value = originBgmValue;
        sfxSlider.value = originSfxValue;
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(originBgmValue) * 20);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(originSfxValue) * 20);
        // 해상도
        resolutionsDropdown.value = originResolutionIndex;
        fullscreenToggle.isOn = originFullscreenMode;
        Debug.Log("origin 적용");
    }
    #endregion



    #region 사운드
    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void Slider_SetBgmVolume()
    {
        // 로그의 형태를 가지기 때문에 값으로 0 불가
        if (bgmSlider.value != 0)
        {
            bgmVolume = bgmSlider.value;
        }
        else
        {
            bgmVolume = 0.0001f;
        }
        // 수치 적용
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(bgmVolume) * 20);
    }

    /// <summary>
    /// SFX 볼륨 설정
    /// </summary>
    public void Slider_SetSfxVolume()
    {
        // 로그의 형태를 가지기 때문에 값으로 0 불가
        if (sfxSlider.value != 0)
        {
            sfxVolume = sfxSlider.value;
        }
        else
        {
            sfxVolume = 0.0001f;
        }
        // 수치 적용
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
    }

    /// <summary>
    /// 사운드 설정 적용
    /// </summary>
    private void SaveSoundSetting()
    {
        // 값 저장
        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 저장된 사운드 설정 불러오기
    /// </summary>
    private void LoadSoundSetting()
    {
        // 저장된 bgm 볼륨 불러옴, 값이 없으면 1로 설정
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 1);
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(bgmSlider.value) * 20);
        // 저장된 sfx 볼륨 불러옴, 값이 없으면 1로 설정
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
        audioMixer.SetFloat("bgmVolume", Mathf.Log10(sfxSlider.value) * 20);
    }
    #endregion


    #region 해상도
    /// <summary>
    /// 전체화면 체크 토글용 함수
    /// </summary>
    /// <param name="isFullScreen">토글 매개변수</param>
    public void SetFullScreen(bool isFullScreen)
    {
        fullScreenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    /// <summary>
    /// 선택한 해상도와 화면 모드로 변경
    /// </summary>
    /// <param name="index">선택한 해상도 인덱스</param>
    private void SaveResolutionSetting(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, fullScreenMode);
        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.SetInt("fullScreen", (int)fullScreenMode);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 해상도 UI 설정
    /// </summary>
    private void InitResolutionUI()
    {
        // 드랍다운 메뉴 항목 비우기
        resolutionsDropdown.options.Clear();

        // 드랍다운 메뉴 항목 채우기
        for (int i = 0; i < resolutions.Count; i++)
        {
            Dropdown.OptionData option = new()
            {
                text = $"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRate}hz"
            };
            resolutionsDropdown.options.Add(option);

            if (resolutionIndex == -1 && resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                resolutionIndex = i;
            }
        }

        resolutionsDropdown.value = resolutionIndex;
        resolutionsDropdown.RefreshShownValue();

        // 전체 화면이면 토글에도 체크
        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
    }

    /// <summary>
    /// 해상도 설정 불러오기
    /// </summary>
    private void LoadResolutionSetting()
    {
        // 모니터의 사용 가능한 해상도 불러오기
        resolutions.AddRange(Screen.resolutions);

        // 저장된 값이 없다면 기본 해상도와 화면 모드를 쓰게 될 것
        if (PlayerPrefs.HasKey("resolutionIndex") && PlayerPrefs.HasKey("fullScreen"))
        {
            resolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("fullScreen");
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullScreenMode);
        }
        else
        {
            resolutionIndex = -1;
        }

        // ui 표시 설정
        InitResolutionUI();
    }
    #endregion

}
