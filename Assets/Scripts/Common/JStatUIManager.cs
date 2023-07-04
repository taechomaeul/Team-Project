using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JStatUIManager : MonoBehaviour
{
    [Header("연결 필수 / LEFT")]
    [Tooltip("빙의체 이미지")]
    public GameObject curPrefabImage;

    [Tooltip("플레이어 HP Slider")]
    public Slider hpSlider;
    [Tooltip("영혼석 HP Slider")]
    public Slider soulSlider;

    [Tooltip("플레이어 HP - 현재 혼력(HP) TEXT")]
    public Text plCurHpText;
    [Tooltip("영혼석 - 영혼의 무게 TEXT")]
    public Text soulHpText;

    [Header("연결 필수 / RIGHT")]
    [Tooltip("빙의한 몬스터 이름")]
    public Text prefabNameText;
    [Tooltip("능력치 - 공격력")]
    public Text plAtkText;
    [Tooltip("능력치 - 이동 속도")]
    public Text plMoveSpdText;

    [Tooltip("스킬 상세 - 스킬 이미지")]
    public GameObject curSImage;
    [Tooltip("스킬 상세 - 스킬 이름")]
    public Text curSNameText;
    [Tooltip("스킬 상세 - 스킬 설명")]
    public Text curSDescriptionText;

    [Header("고정 변수")]
    private readonly int maxSoul = 666;

    private PlayerInfo plInfo;
    private EnemyPrefab enemyPrefabInfo;

    void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        enemyPrefabInfo = GameObject.Find("ActionFunction").GetComponent<EnemyPrefab>();
    }

    void Update()
    {
        //LEFT Page
        curPrefabImage.GetComponent<Image>().sprite = enemyPrefabInfo.enemyImages[plInfo.curPrefabIndex];
        plCurHpText.text = $"{plInfo.curHp}";
        soulHpText.text = $"{plInfo.soulHp}";
        hpSlider.value = (float)plInfo.curHp / plInfo.maxHp;
        soulSlider.value = (float)plInfo.soulHp / maxSoul;

        //RIGHT Page - 능력치
        string lang = "EN"; //SettingManager에서 불러온 값
        if (lang.Equals("KR"))
        {
            prefabNameText.text = enemyPrefabInfo.enemyPrefabNames_KR[plInfo.curPrefabIndex];
        }
        else if (lang.Equals("EN"))
        {
            prefabNameText.text = enemyPrefabInfo.enemyPrefabNames_EN[plInfo.curPrefabIndex];
        }
        
        plAtkText.text = plInfo.plAtk.ToString();
        plMoveSpdText.text = plInfo.plMoveSpd.ToString();

        //RIGHT Page - 스킬 상세
        curSImage.GetComponent<Image>().sprite = plInfo.curSkill.thumnail;
        curSNameText.text = plInfo.curSkill.skillName;
        curSDescriptionText.text = plInfo.curSkill.skillDescription;
    }
}
