using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("플레이어 HP Slider")]
    public Slider hpSlider;
    [Tooltip("영혼석 - 영혼의 무게 TEXT")]
    public Text soulHpText;
    [Tooltip("일지 Panel")]
    public GameObject journalPanel;
    [Tooltip("플레이어 사망 패널")]
    public GameObject playerDiePanel;

    private PlayerInfo plInfo;
    private GameManager gameManager;

    void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        soulHpText.text = $"{plInfo.soulHp}";
        hpSlider.value = (float) plInfo.curHp / plInfo.maxHp;

        if (Input.GetKeyDown(KeyCode.J)) //J를 누르면
        {
            journalPanel.SetActive(true); //일지 패널 활성화
            gameManager.PauseTheGame(); //게임 진행 중지
        }
    }

    public void OnPlayerDiePanel()
    {
        playerDiePanel.SetActive(true);
    }

    public void OffPlayerDiePanel()
    {
        playerDiePanel.SetActive(false);
    }

    public void ResetToLastSave()
    {
        gameManager.LoadGame();
        playerDiePanel.SetActive(false);
    }
}
