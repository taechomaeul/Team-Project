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

    [Header("연결 필수 / 지도")]
    [Tooltip("N번째 제단 텍스트")]
    public GameObject lastPosText;


    [Header("고정 변수")]
    private readonly int maxSoul = 666;

    private PlayerInfo plInfo;
    private SkillInfo skillInfo;
    private EnemyPrefab enemyPrefabInfo;


    void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        enemyPrefabInfo = GameObject.Find("ActionFunction").GetComponent<EnemyPrefab>();
        skillInfo = GameObject.Find("ActionFunction").GetComponent<SkillInfo>();
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
        int langOffset = 0;
        string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();
        if (lang.Equals("KR")) //현재
        {
            prefabNameText.text = enemyPrefabInfo.enemyPrefabNames_KR[plInfo.curPrefabIndex];
            if (plInfo.curSkill.skillIndex >= 5 && plInfo.curSkill.skillIndex < 10) //EN
            {
                langOffset = -5;
            }

            if (SaveManager.Instance.saveClass.GetLastSavePosition() == 0)
            {
                lastPosText.GetComponent<Text>().text = "최초 시작 지점";
            }
            else
            {
                lastPosText.GetComponent<Text>().text = SaveManager.Instance.saveClass.GetLastSavePosition() + "번째 제단";
            }
            
        }
        else if (lang.Equals("EN"))
        {
            prefabNameText.text = enemyPrefabInfo.enemyPrefabNames_EN[plInfo.curPrefabIndex];
            if (plInfo.curSkill.skillIndex < 5) //KR
            {
                langOffset = 5;
            }

            if (SaveManager.Instance.saveClass.GetLastSavePosition() == 0)
            {
                lastPosText.GetComponent<Text>().text = "First Starting Point";
            }
            else
            {
                lastPosText.GetComponent<Text>().text = "Save Point #" + SaveManager.Instance.saveClass.GetLastSavePosition();
            }
        }
        
        plAtkText.text = plInfo.plAtk.ToString();
        plMoveSpdText.text = plInfo.plMoveSpd.ToString();

        //RIGHT Page - 스킬 상세
        curSImage.GetComponent<Image>().sprite = plInfo.curSkill.thumnail;
        curSNameText.text = skillInfo.skills[plInfo.curSkill.skillIndex + langOffset].skillName;
        curSDescriptionText.text = skillInfo.skills[plInfo.curSkill.skillIndex + langOffset].skillDescription;
        //curSDescriptionText.text = plInfo.curSkill.skillDescription;

        
    }
}
