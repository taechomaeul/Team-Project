using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private ShowScript showScript;
    private ShowRecord showRecord;
    public List<string> completeColliderNames;

    void Awake()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        showRecord = GameObject.Find("ActionFunction").GetComponent<ShowRecord>();
    }

    /// <summary>
    /// 이미 본 일지를 확인하는 배열인 CheckRecord를 이용하여 Collider 오브젝트를 off하는 함수
    /// </summary>
    /// <param name="checkRecord">이미 본 일지를 확인하는 배열</param>
    public void OffRecordCollider(bool[] checkRecord)
    {
        GameObject rCollider = GameObject.Find("RecordGroup");
        GameObject[] recordObject = new GameObject[rCollider.transform.childCount]; //10

        int recordStart;
        if (rCollider.transform.GetChild(0).name.Contains("누명자"))
        {
            recordStart = 0;
        }
        else
        {
            recordStart = showRecord.checkRecordComplete.Length - recordObject.Length; //15-10
        }

        Debug.Log($"RecordStart : {recordStart}");
        int index = 0;
        for (int i = recordStart; i < recordObject.Length + recordStart; i++)
        {
            recordObject[index] = rCollider.transform.GetChild(index).gameObject;
            if (checkRecord[i]) //true라면 ColliderObject를 끈다
            {
                recordObject[index].SetActive(false);
            }
            index++;
        }
    }

    /// <summary>
    /// 이미 본 스크립트를 확인하는 배열인 CheckRecord를 이용하여 Collider 오브젝트를 off하는 함수
    /// </summary>
    /// <param name="checkScript">이미 본 스클립트를 확인하는 배열</param>
    public void OffScriptCollider(bool[] checkScript)
    {
        GameObject sCollider = GameObject.Find("ScriptCollider");
        GameObject[] scriptObject = new GameObject[sCollider.transform.childCount];

        for (int i = 0; i < scriptObject.Length; i++)
        {
            string cName = sCollider.transform.GetChild(i).GetComponent<ScriptColliderInfo>().colliderName; //collider 이름으로
            int index = showScript.GetIndex(cName); //index를 불러온 뒤
            //-> 해당 script의 인덱스는 IDX입니다. (START_IDX  X)
            scriptObject[i] = sCollider.transform.GetChild(i).gameObject;

            if (checkScript[index]) //해당 인덱스에 있는 체크 값이 true라면 ColliderObject를 끈다
            {
                scriptObject[i].SetActive(false);
            }
        }
    }

    public void AddMoveCollider()
    {
        GameObject aCollider = GameObject.Find("MoveCollider");
        GameObject[] animObject = new GameObject[aCollider.transform.childCount];

        //리스트에 추가

        for (int i = 0; i < animObject.Length; i++)
        {
            completeColliderNames.Add(aCollider.transform.GetChild(i).name);
        }
    }

    public void OffAnimationSceneCollider(bool[] checkAnim)
    {
        GameObject aCollider = GameObject.Find("MoveCollider");
        GameObject[] animObject = new GameObject[aCollider.transform.childCount];

        //리스트에 추가

        for (int i=0; i<animObject.Length; i++)
        {
            
        }
    }

}
