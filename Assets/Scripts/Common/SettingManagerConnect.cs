using UnityEngine;
using UnityEngine.UI;

public class SettingManagerConnect : MonoBehaviour
{
    // 다른 스크립트
    SettingManager settings;
    GameManager gm;

    // 값 변경되었는지 체크
    bool isChanged = false;

    // 인스펙터
    [Header("환경설정 패널")]
    [Tooltip("환경설정 패널")]
    public GameObject panel;

    [Tooltip("환경설정 나가기 확인 패널")]
    public GameObject exitCheckPanel;

    [Header("값 설정 UI")]
    [Tooltip("BGM 볼륨")]
    public Slider bgmSlider;

    [Tooltip("SFX 볼륨 조절 슬라이더")]
    public Slider sfxSlider;

    [Tooltip("해상도")]
    public Dropdown resolutionsDropdown;

    [Tooltip("전체화면")]
    public Toggle fullscreenToggle;

    [Tooltip("밝기 조절 슬라이더")]
    public Slider brightnessSlider;

    [Tooltip("언어 선택 드랍다운")]
    public Dropdown languageDropdown;



    private void Awake()
    {
        settings = FindObjectOfType<SettingManager>();
        gm = FindObjectOfType<GameManager>();
        settings.InitUIObjectAndLoadValues(bgmSlider, sfxSlider, resolutionsDropdown, fullscreenToggle, brightnessSlider, languageDropdown);
    }

    private void Update()
    {
        // 저장 안된 변경 값이 있다면
        if (isChanged)
        {
            // 설정 화면이 닫혔다면
            if (!panel.activeInHierarchy)
            {
                // 설정 화면 열고 경고창 띄움
                panel.SetActive(true);
                exitCheckPanel.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        // 저장 안된 변경 값이 있다면
        if (isChanged)
        {
            // 게임 멈추고 일지 화면 열고 경고창 띄움
            gm.PauseTheGame();
            gameObject.SetActive(true);
            exitCheckPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 설정 저장 버튼용
    /// </summary>
    public void Btn_SettingApply()
    {
        settings.Btn_SettingApply();
        isChanged = false;
    }

    /// <summary>
    /// 설정값 변경 취소
    /// </summary>
    public void Btn_UndoSetting()
    {
        settings.Btn_UndoSetting();
        isChanged = false;
    }

    /// <summary>
    /// BGM 슬라이더용
    /// </summary>
    public void Slider_SetBgmVolume()
    {
        settings.Slider_SetBgmVolume();
        isChanged = true;
    }

    /// <summary>
    /// SFX 슬라이더용
    /// </summary>
    public void Slider_SetSfxVolume()
    {
        settings.Slider_SetSfxVolume();
        isChanged = true;
    }

    /// <summary>
    /// 전체화면 토글용
    /// </summary>
    /// <param name="isFullScreen">토글용</param>
    public void SetFullScreen(bool isFullScreen)
    {
        settings.SetFullScreen(isFullScreen);
        isChanged = true;
    }

    /// <summary>
    /// 해상도 변경 체크
    /// </summary>
    public void ResolutionChangeCheck()
    {
        isChanged = true;
    }

    /// <summary>
    /// 밝기 슬라이더용
    /// </summary>
    public void Slider_SetBrightness()
    {
        settings.Slider_SetBrightness();
        isChanged = true;
    }

    /// <summary>
    /// 언어 조절 드랍다운용
    /// </summary>
    /// <param name="index">드랍다운용</param>
    public void Dropdown_SetLanguage(int index)
    {
        settings.SetLanguage(index);
    }
}