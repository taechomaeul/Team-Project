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
    public ActionFunction actionFuntion;
    public PlayerInfo plInfo;

    private void Start()
    {
        plInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFunction>();
        thisSoul = GetComponent<SoulInfo>();

        toolTip = actionFuntion.fCommonPanel;

        if (!gameObject.transform.parent.name.Equals("Enemy1_TutorialDead"))
        {
            detailToolTip = actionFuntion.possessPanel;
        }

        toolTip.SetActive(false);

 
        if (gameObject.tag == "Enemy")
        {
            thisSoul.havingHP = (int) (transform.GetComponentInChildren<EnemyInfo>().stat.GetMaxHp() * enemySoulPercent);
        }
        else
        {
            thisSoul.havingHP = 30; //예시로 만들어놓은 프리팹 적용용
        }
        //Debug.Log($"시체에서 추출할 수 있는 영혼의 양 : {thisSoul.havingHP}");
        
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
            if (Input.GetKeyDown(KeyCode.F)) //Collider 안에 들어와있고 F버튼을 누른다면
            {
                //툴팁을 종료하고, 상세툴팁(빙의/흡수 등)을 켠다.
                toolTip.SetActive(false);
                detailToolTip.SetActive(true);
                isDetail = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && isDetail) //디테일이 켜진 채로 1번이 눌렸다면
            {
                //혼력(HP)을 흡수한다.
                if (thisSoul.havingHP == 0) //0일 경우에는 더이상 추출할 수 없다.
                {
                    Debug.Log("더이상 혼력을 추출할 수 없습니다!");
                    detailToolTip.SetActive(false);
                }
                else
                {
                    detailToolTip.SetActive(false);
                    actionFuntion.MoveSoulToStone(thisSoul.havingHP);
                    if (plInfo.soulHp > maxSoul) // 플레이어의 영혼석의 무게는 최대 무게를(666) 넘을 수 없다.
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