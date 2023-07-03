using UnityEngine;
using UnityEngine.UI;

public class SettingManagerConnect : MonoBehaviour
{
    SettingManager settings;
    GameManager gm;

    bool isChanged = false;

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



    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    settings = FindObjectOfType<SettingManager>();
    //}

    private void Awake()
    {
        settings = FindObjectOfType<SettingManager>();
        gm = FindObjectOfType<GameManager>();
        settings.InitUIObjectAndLoadValues(bgmSlider, sfxSlider, resolutionsDropdown, fullscreenToggle, brightnessSlider);
    }

    private void Update()
    {
        if (isChanged)
        {
            if (!panel.activeInHierarchy)
            {
                panel.SetActive(true);
                exitCheckPanel.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (isChanged)
        {
            gm.PauseTheGame();
            gameObject.SetActive(true);
            exitCheckPanel.SetActive(true);
        }
    }

    public void Btn_SettingApply()
    {
        settings.Btn_SettingApply();
        isChanged = false;
    }

    public void Btn_UndoSetting()
    {
        settings.Btn_UndoSetting();
        isChanged = false;
    }

    public void Slider_SetBgmVolume()
    {
        settings.Slider_SetBgmVolume();
        isChanged = true;
    }

    public void Slider_SetSfxVolume()
    {
        settings.Slider_SetSfxVolume();
        isChanged = true;
    }


    public void SetFullScreen(bool isFullScreen)
    {
        settings.SetFullScreen(isFullScreen);
        isChanged = true;
    }

    public void ResolutionChangeCheck()
    {
        isChanged = true;
    }

    public void Slider_SetBrightness()
    {
        settings.Slider_SetBrightness();
        isChanged = true;
    }
}