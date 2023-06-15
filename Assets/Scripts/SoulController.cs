using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulController : MonoBehaviour
{

    public bool isDetail = false;
    public const float maxSoul = 666;

    public SoulInfo exampleSoul;
    public GameObject toolTip;
    public GameObject detailToolTip;

    public ActionFuntion actionFuntion;
    public PlayerInfo plInfo;

    private void Start()
    {
        toolTip.SetActive(false);

        exampleSoul = GetComponent<SoulInfo>();
        exampleSoul.havingHP = 30f;
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
                if (exampleSoul.havingHP == 0)
                {
                    Debug.Log("더이상 혼력을 추출할 수 없습니다!");
                    detailToolTip.SetActive(false);
                }
                else
                {
                    detailToolTip.SetActive(false);
                    actionFuntion.MoveSoulToStone(exampleSoul.havingHP);
                    if (plInfo.soulHp > maxSoul)
                    {
                        plInfo.soulHp = maxSoul;
                    }
                    exampleSoul.havingHP = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && isDetail)
            {
                //빙의하기
                Debug.Log("빙의하기!!!");
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
