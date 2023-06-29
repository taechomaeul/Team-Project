using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulController : MonoBehaviour
{
    [Header("기타 변수")]
    public bool isDetail = false;
    
    [Header("고정 변수")]
    public readonly float enemySoulPercent = 0.3f;
    public readonly int maxSoul = 666;

    [Header("연결 필수")]
    public GameObject toolTip;
    public GameObject detailToolTip;

    [Header("연결 X")]
    public SoulInfo thisSoul;
    public ActionFuntion actionFuntion;
    public PlayerInfo plInfo;

    private void Start()
    {
        toolTip.SetActive(false);

        thisSoul = GetComponent<SoulInfo>();

        if (gameObject.tag == "Enemy")
        {
            thisSoul.havingHP = (int) (transform.GetComponentInChildren<EnemyInfo>().stat.GetMaxHp() * enemySoulPercent);
        }
        else
        {
            thisSoul.havingHP = 30; //예시로 만들어놓은 프리팹 적용용
        }
        //Debug.Log($"시체에서 추출할 수 있는 영혼의 양 : {thisSoul.havingHP}");
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                toolTip.SetActive(false);
                detailToolTip.SetActive(true);
                isDetail = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && isDetail)
            {
                if (thisSoul.havingHP == 0)
                {
                    Debug.Log("더이상 혼력을 추출할 수 없습니다!");
                    detailToolTip.SetActive(false);
                }
                else
                {
                    detailToolTip.SetActive(false);
                    actionFuntion.MoveSoulToStone(thisSoul.havingHP);
                    if (plInfo.soulHp > maxSoul)
                    {
                        plInfo.soulHp = maxSoul;
                    }
                    thisSoul.havingHP = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && isDetail)
            {
                Debug.Log(transform.parent.gameObject, gameObject);
                actionFuntion.ChangePrefab(other.gameObject, transform.parent.gameObject); //PlayerModel, Enemy
                detailToolTip.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toolTip.SetActive(false);
        detailToolTip.SetActive(false);
        isDetail = false;
    }

}
