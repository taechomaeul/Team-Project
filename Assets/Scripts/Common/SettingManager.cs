using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    #region 수치 조절용 변수들
    // 사운드 관련
    private float bgmVolume;
    private float sfxVolume;

    // 해상도 관련    
    private List<Resolution> resolutions = new();
    private int resolutionIndex;
    private FullScreenMode fullScreenMode;

    // 밝기 관련
    private Light lightComponent;

    // 언어
    private int languageIndex;

    // 변경 전 값
    private float originBgmValue;
    private float originSfxValue;
    private int originResolutionIndex;
    private bool originFullscreenMode;
    private float originBrightness;
    private int originLanguageIndex;
    #endregion


    #region UI 요소
    // BGM 조절 슬라이더
    private Slider bgmSlider;

    // SFX 조절 슬라이더
    private Slider sfxSlider;

    // 해상도 조절 드랍다운
    private Dropdown resolutionsDropdown;

    // 전체화면 조절 토글
    private Toggle fullscreenToggle;

    // 밝기 조절 슬라이더
    private Slider brightnessSlider;

    // 언어 선택 드랍다운
    private Dropdown languageDropdown;
    #endregion

    // 인스펙터
    [Header("설정")]
    [Tooltip("오디오 믹서")]
    public AudioMixer audioMixer;

    [Tooltip("밝기용 라이트")]
    public GameObject brightnessLight;




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
            DontDestroyOnLoad(brightnessLight);
            lightComponent = brightnessLight.GetComponent<Light>();
        }
    }


    #region 시스템
    /// <summary>
    /// SettingManagerConnect에서 UI요소 초기화
    /// </summary>
    /// <param name="bgmSlider">BGM 슬라이더</param>
    /// <param name="sfxSlider">SFX 슬라이더</param>
    /// <param name="resolutionsDropdown">해상도 드랍다운</param>
    /// <param name="fullscreenToggle">전체화면 체크 토글</param>
    /// <param name="brightnessSlider">밝기 슬라이더</param>
    /// <param name="languageDropdown">언어 드랍다운</param>
    public void InitUIObjectAndLoadValues(
        Slider bgmSlider, Slider sfxSlider, Dropdown resolutionsDropdown,
        Toggle fullscreenToggle, Slider brightnessSlider, Dropdown languageDropdown)
    {
        this.bgmSlider = bgmSlider;
        this.sfxSlider = sfxSlider;
        this.resolutionsDropdown = resolutionsDropdown;
        this.fullscreenToggle = fullscreenToggle;
        this.brightnessSlider = brightnessSlider;
        this.languageDropdown = languageDropdown;
        LoadSettings();
        SetOriginValues();
    }

    /// <summary>
    /// 최초 실행시 설정 불러오기
    /// </summary>
    private void LoadSettings()
    {
        LoadSoundSetting();
        LoadResolutionSetting();
        LoadBrightnessSetting();
        LoadLanguageSetting();
        Debug.Log("설정 값 불러오기 완료");
    }

    /// <summary>
    /// 환경 설정 화면 열 때 동작
    /// 원래 값들 저장
    /// </summary>
    private void SetOriginValues()
    {
        // 사운드
        originBgmValue = bgmVolume;
        originSfxValue = sfxVolume;
        // 해상도
        originResolutionIndex = resolutionsDropdown.value;
        originFullscreenMode = fullscreenToggle.isOn;
        // 밝기
        originBrightness = brightnessSlider.value;
        // 언어
        originLanguageIndex = languageIndex;
        Debug.Log("origin 저장");
    }

    /// <summary>
    /// 설정 내용 적용하는 확인 버튼용
    /// </summary>
    public void Btn_SettingApply()
    {
        SaveSoundSetting();
        SaveResolutionSetting(resolutionsDropdown.value);
        SaveBrightnessSetting();
        SaveLanguageSetting();
        Debug.Log("설정 값 저장");
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
        // 밝기
        brightnessSlider.value = originBrightness;
        // 언어
        languageDropdown.value = originLanguageIndex;
        Debug.Log("origin 적용");
    }
    #endregion


    #region 사운드
    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    public void Slider_SetBgmVolume()
    {
        // 오디오믹서의 설정 값이 로그의 형태를 가지기 때문에 볼륨 값으로 0 불가
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
        // 오디오믹서의 설정 값이 로그의 형태를 가지기 때문에 볼륨 값으로 0 불가
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
    /// 해상도 UI 설정
    /// </summary>
    private void InitResolutionUI()
    {
        // 드랍다운 메뉴 항목 비우기
        resolutionsDropdown.options.Clear();

        // 드랍다운 메뉴 항목 채우기
        for (int i = 0; i < resolutions.Count; i++)
        {
            // UI 조절에 시간이 없는 관계로 16:9 비율만
            if (resolutions[i].width / 16 * 9 == resolutions[i].height)
            {
                Debug.Log($"{resolutions[i].width} x {resolutions[i].height}");
                Dropdown.OptionData option = new()
                {
                    text = $"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRate}hz"
                };
                resolutionsDropdown.options.Add(option);
            }

            if (resolutionIndex == -1 && resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                resolutionIndex = i;
            }
        }
        // 현재 해상도로 드랍다운 인덱스 설정
        resolutionsDropdown.value = resolutionIndex;
        // 드랍다운 새로고침
        resolutionsDropdown.RefreshShownValue();

        // 전체 화면이면 토글에도 체크
        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
    }

    /// <summary>
    /// 선택한 해상도와 화면 모드로 변경
    /// </summary>
    /// <param name="index">선택한 해상도 인덱스</param>
    private void SaveResolutionSetting(int index)
    {
        // 선택한 해상도, 화면 모드로 변경
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, fullScreenMode);
        // 값 저장
        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.SetInt("fullScreen", (int)fullScreenMode);
        PlayerPrefs.Save();
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


    #region 밝기
    /// <summary>
    /// 밝기 설정
    /// </summary>
    public void Slider_SetBrightness()
    {
        lightComponent.intensity = brightnessSlider.value;
    }

    /// <summary>
    /// 밝기 설정 적용
    /// </summary>
    public void SaveBrightnessSetting()
    {
        // 값 저장
        PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
        PlayerPrefs.Save();
    }

    /// <summary>
    ///  저장된 밝기 설정 불러오기
    /// </summary>
    public void LoadBrightnessSetting()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
    }
    #endregion


    #region 언어
    /// <summary>
    /// 언어 선택
    /// </summary>
    /// <param name="index">언어 선택 드랍다운 인덱스</param>
    public void SetLanguage(int index)
    {
        languageIndex = index;
    }

    /// <summary>
    /// 설정된 언어 인덱스 가져오기
    /// </summary>
    /// <returns>언어 인덱스</returns>
    public int GetCurrentLanguageIndex()
    {
        return languageIndex;
    }

    /// <summary>
    /// 설정된 언어 인덱스 string으로 가져오기
    /// </summary>
    /// <returns>언어 string</returns>
    public string GetCurrentLanguageIndexToString()
    {
        return languageIndex switch
        {
            0 => "kr",
            1 => "en",
            _ => "kr",
        };
    }

    /// <summary>
    /// 언어 선택용 드랍다운 초기화
    /// </summary>
    private void InitLanguageUI()
    {
        languageDropdown.value = languageIndex;
        languageDropdown.RefreshShownValue();
    }

    /// <summary>
    /// 언어 설정 저장
    /// </summary>
    private void SaveLanguageSetting()
    {
        PlayerPrefs.SetInt("language", languageIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 저장된 언어 설정 불러오기
    /// </summary>
    private void LoadLanguageSetting()
    {
        languageIndex = PlayerPrefs.GetInt("language", 0);
        InitLanguageUI();
    }
    #endregion
}