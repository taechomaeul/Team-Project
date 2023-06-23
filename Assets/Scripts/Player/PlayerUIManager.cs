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

    [Header("연결 X")]
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        soulHpText.text = $"x{playerController.plInfo.soulHp}";
        hpSlider.value = playerController.plInfo.curHp / playerController.plInfo.maxHp;

        Debug.Log($"{playerController.plInfo.curHp} / {playerController.plInfo.maxHp}" + hpSlider.value);
    }
}
