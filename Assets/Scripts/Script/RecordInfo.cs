using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordInfo : MonoBehaviour
{
    public string recordName;

    public GameObject toolTipPanel;
    public GameObject recordPanel;
    public ShowScript showScript;
    public ActionFuntion actionFuntion;

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toolTipPanel.SetActive(false);
    }
}
