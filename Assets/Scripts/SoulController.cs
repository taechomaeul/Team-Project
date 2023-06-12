using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulController : MonoBehaviour
{

    public bool isDetail = false;

    public SoulInfo exampleSoul;
    public Transform cameraTransform;
    public GameObject toolTip;
    public GameObject detailToolTip;

    public ActionFuntion actionFuntion;

    private void Start()
    {
        toolTip.SetActive(false);

        exampleSoul = GetComponent<SoulInfo>();
        exampleSoul.havingHP = 30f;
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            toolTip.SetActive(true);
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
                    actionFuntion.MoveSoulToPlayer(exampleSoul.havingHP);
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
