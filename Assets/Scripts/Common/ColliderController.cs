using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private ShowScript showScript;
    private ShowRecord showRecord;

    void Awake()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        showRecord = GameObject.Find("ActionFunction").GetComponent<ShowRecord>();
    }

    public void OffRecordCollider(bool[] checkRecord)
    {
        GameObject[] recordObject = new GameObject[GameObject.Find("RecordGroup").transform.childCount];

        // **************************
        //bool[] checkRecord = showScript.GetCheckRecordComplete();
        // 이 부분에 bool값을 로드한 함수(누군가의 일지용)를 불러와주세요!
        //Debug.Log("checkRecord Length: " + checkRecord.Length);

        int recordStart;
        if (GameObject.Find("RecordGroup").transform.GetChild(0).name.Contains("누명자"))
        {
            recordStart = 0;
            Debug.Log($"1F RecordStart : {recordStart}");
        }
        else
        {
            recordStart = showRecord.record.Count - recordObject.Length;
            Debug.Log($"2F RecordStart : {recordStart}");
        }

        for (int i = recordStart; i < recordObject.Length + recordStart; i++)
        {
            //Debug.Log($"CheckRecord [{i}] (OFF RECORD COLLIDER) : {checkRecord[i]}");
            if (checkRecord[i]) //true라면 ColliderObject를 끈다
            {
                recordObject[i].SetActive(false);
            }
        }
    }

    public void OffScriptCollider(bool[] checkScript)
    {
        GameObject sCollider = GameObject.Find("ScriptCollider");
        GameObject[] scriptObject = new GameObject[sCollider.transform.childCount];

        // **************************
        //bool[] checkScript = showScript.GetCheckScriptComplete();
        // 이 부분에 bool값을 로드한 함수(스크립트용)를 불러와주세요! 

        for (int i = 0; i < scriptObject.Length; i++)
        {
            string cName = sCollider.transform.GetChild(i).GetComponent<ScriptColliderInfo>().colliderName; //collider 이름으로
            int index = showScript.GetIndex(cName); //index를 불러온 뒤
            //-> 해당 script의 인덱스는 IDX입니다. START_IDX 아닙니다!!

            if (checkScript[index]) //해당 인덱스에 있는 체크 값이 true라면 ColliderObject를 끈다
            {
                scriptObject[i].SetActive(false);
            }
        }
    }

}
