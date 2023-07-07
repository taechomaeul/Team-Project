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
    [Tooltip("스킬 UI")]
    public GameObject curSkillImg;
    [Tooltip("스킬 쿨타임 UI")]
    public GameObject coolTimeImg;
    [Tooltip("일지 Panel")]
    public GameObject journalPanel;
    [Tooltip("플레이어 사망 패널")]
    public GameObject playerDiePanel;

    private PlayerController plController;
    private PlayerInfo plInfo;
    private GameManager gameManager;

    [SerializeField]
    private float curTime=0;
    [SerializeField]
    private bool isFirst = false;

    void Start()
    {
        plController = GameObject.Find("Player").GetComponent<PlayerController>();
        plInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (plController.isSkillCool && !isFirst)
        {
            isFirst = true;
            curTime = plInfo.curSkill.coolTime;
            coolTimeImg.SetActive(true);
            Debug.Log("Skill ON!");
        }
        else if (plController.isSkillCool)
        {
            SkillCoolTime();
        }
        
        soulHpText.text = $"{plInfo.soulHp}";
        hpSlider.value = (float) plInfo.curHp / plInfo.maxHp;
        curSkillImg.GetComponent<Image>().sprite = plInfo.curSkill.thumnail;

        if (Input.GetKeyDown(KeyCode.J)) //J를 누르면
        {
            journalPanel.SetActive(true); //일지 패널 활성화
            gameManager.PauseTheGame(); //게임 진행 중지
        }
    }

    /// <summary>
    /// 플레이어 사망 패널 켜는 함수
    /// </summary>
    public void OnPlayerDiePanel()
    {
        playerDiePanel.SetActive(true);
    }

    /// <summary>
    /// 플레이어 사망 패널 끄는 함수
    /// </summary>
    public void OffPlayerDiePanel()
    {
        playerDiePanel.SetActive(false);
    }

    /// <summary>
    /// 최근 저장 위치에서 부활하는 함수 (씬 재로드)
    /// </summary>
    public void ResetToLastSave()
    {
        gameManager.LoadGame();
        playerDiePanel.SetActive(false);
    }

    public void SkillCoolTime()
    {
        curTime -= Time.deltaTime;
        if (curTime < plInfo.curSkill.coolTime && curTime >= 0)
        {
            coolTimeImg.GetComponent<Image>().fillAmount = curTime / plInfo.curSkill.coolTime;
        }
        else
        {
            curTime = plInfo.curSkill.coolTime;
            coolTimeImg.GetComponent<Image>().fillAmount = 1f;
            coolTimeImg.SetActive(false);
            isFirst = false;
            //plController.isSkillCool = false;
        }
    }
}
