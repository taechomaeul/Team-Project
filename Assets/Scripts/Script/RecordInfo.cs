using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordInfo : MonoBehaviour
{
    [Header("연결 필수")]
    public string recordName;
    public GameObject toolTipPanel;
    public GameObject recordPanel;

    private ShowScript showScript;
    private ActionFuntion actionFuntion;
    private bool isConfirm = false;

    private void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toolTipPanel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
       if (Input.GetKeyDown(KeyCode.F))
        {
            recordPanel.SetActive(true);
            actionFuntion.PauseGameForAct();
            StartCoroutine(showScript.LoadRecordData(recordName));
            toolTipPanel.SetActive(false);
            isConfirm = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toolTipPanel.SetActive(false);
        if (isConfirm)
        {
            Destroy(gameObject); //확인한 일지 삭제
        }
    }
}
