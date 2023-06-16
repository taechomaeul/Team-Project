using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    public Slider hpSlider;
    public Text soulHpText;
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        soulHpText.text = $"x{playerController.plInfo.soulHp}";
        hpSlider.value = playerController.plInfo.curHp / playerController.plInfo.maxHp;
    }
}
