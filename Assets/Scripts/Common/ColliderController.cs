using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private ShowScript showScript;

    void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();

        
        OffRecordCollider();
        OffScriptCollider();
    }

    
    void Update()
    {
        
    }

    public void OffRecordCollider()
    {
        GameObject[] recordObject = new GameObject[GameObject.Find("RecordGroup").transform.childCount];

        bool[] checkRecord = showScript.GetCheckRecordComplete();
        //Debug.Log("checkRecord Length: " + checkRecord.Length);

        for (int i = 0; i < recordObject.Length; i++)
        {
            //Debug.Log($"CheckRecord(OFF RECORD COLLIDER : {checkRecord[i]}");
            if (checkRecord[i]) //true라면 ColliderObject를 끈다
            {
                recordObject[i].SetActive(false);
            }
        }
    }

    public void OffScriptCollider()
    {
        GameObject[] scriptObject = new GameObject[GameObject.Find("ScriptCollider").transform.childCount];

        showScript.GetCheckScriptComplete();

        for (int i = 0; i < scriptObject.Length; i++)
        {
            if (showScript.checkScriptComplete[i]) //true라면 ColliderObject를 끈다
            {
                scriptObject[i].SetActive(false);
            }
        }
    }

}
