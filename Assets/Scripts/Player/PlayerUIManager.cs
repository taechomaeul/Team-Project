using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("플레이어 HP Slider")]
    public Slider hpSlider;
    [Tooltip("영혼석 - 영혼의 무게 TEXT")]
    public Text soulHpText;
    [Tooltip("일지 Panel")]
    public GameObject journalPanel;

    [Header("연결 X")]
    public PlayerController playerController;
    public GameManager gameManager;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        soulHpText.text = $"x{playerController.plInfo.soulHp}";
        hpSlider.value = (float) playerController.plInfo.curHp / playerController.plInfo.maxHp;

        if (Input.GetKeyDown(KeyCode.J))
        {
            journalPanel.SetActive(true);
            gameManager.PauseTheGame();
        }
    }
}
