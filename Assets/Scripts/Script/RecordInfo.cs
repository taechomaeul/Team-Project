using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordInfo : MonoBehaviour
{
    [Header("연결 필수")]
    public string recordName;
    public GameObject toolTipPanel;
    public GameObject recordPanel;

    //[SerializeField]
    private ShowRecord showRecord;
    //[SerializeField]
    private ActionFuntion actionFuntion;
    private bool isConfirm = false;

    private void Start()
    {
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        showRecord = GameObject.Find("ActionFunction").GetComponent<ShowRecord>();
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
            StartCoroutine(showRecord.LoadRecordData(recordName));
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

            //Record 체크 완료
            int recordIndex = showRecord.curCheckIndex;
            showRecord.checkRecordComplete[recordIndex] = true;
            Debug.Log($"CheckRecordComplete[{recordIndex}] : {showRecord.checkRecordComplete[recordIndex]}");
        }
    }
}
