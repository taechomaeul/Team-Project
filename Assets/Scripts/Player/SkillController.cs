using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("툴팁(F) 패널")]
    public GameObject toolTip;
    [Tooltip("메인 UI 화면에 나오는 스킬 UI 이미지 오브젝트")]
    public GameObject skillUI;

    private PlayerInfo plInfo;
    private SkillInfo skillInfo;
    private int langOffset;

    void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        skillInfo = GameObject.Find("ActionFunction").GetComponent<SkillInfo>();

        string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();
        if (lang.Equals("KR"))
        {
            langOffset = 0;
        }
        else if (lang.Equals("EN"))
        {
            langOffset = skillInfo.skills.Length / skillInfo.langArr.Length;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }

    }

    /// <summary>
    /// Player가 Collider안에 있을 때 F키를 누르면 
    /// 툴팁을 비활성화하고, 부딪힌 오브젝트의 뒤에서 숫자만 가져와 분류를 한 뒤(스킬1, 2, 3)
    /// 스킬 정보를 넘겨주고 UI 이미지를 바꾼다.
    /// </summary>
    /// <param name="other">Collider와 부딪한 물체(Player)</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //스킬 구분
            if (Input.GetKeyDown(KeyCode.F))
            {
                toolTip.SetActive(false);
                Debug.Log($"게임 오브젝트 이름 : {gameObject.name}");
                if (gameObject.name.Substring(gameObject.name.Length - 1).Equals("1"))
                {
                    plInfo.curSkill = skillInfo.skills[1 + langOffset];
                    skillUI.GetComponent<Image>().sprite = skillInfo.skills[1].thumnail;
                }
                else if (gameObject.name.Substring(gameObject.name.Length - 1).Equals("2"))
                {
                    plInfo.curSkill = skillInfo.skills[2 + langOffset];
                    skillUI.GetComponent<Image>().sprite = skillInfo.skills[2].thumnail;
                }
                else if (gameObject.name.Substring(gameObject.name.Length - 1).Equals("3"))
                {
                    plInfo.curSkill = skillInfo.skills[3 + langOffset];
                    skillUI.GetComponent<Image>().sprite = skillInfo.skills[3].thumnail;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toolTip.SetActive(false);
    }
}
