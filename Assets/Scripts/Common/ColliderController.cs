using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private ShowScript showScript;
    private ShowRecord showRecord;
    //public List<string> completeColliderNames;

    void Awake()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        showRecord = GameObject.Find("ActionFunction").GetComponent<ShowRecord>();
        //completeColliderNames = new();
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

    /// <summary>
    /// 이미 지나간 MoveCollider 이름 저장
    /// </summary>
    /// <param name="colliderObjName">플레이어가 부딪힌 MoveCollider의 이름</param>
    public void AddMoveCollider(string colliderObjName)
    {
        SaveManager.Instance.saveClass.AddMoveCollider(colliderObjName);
        Debug.Log($"ADD GameObj name : {colliderObjName}");
        //SaveManager.Instance.saveClass.SetMoveSceneData(completeColliderNames);
    }

    public void OffMoveSceneCollider(List<string> cColliderNames)
    {
        GameObject aCollider = GameObject.Find("MoveCollider");
        GameObject[] animObject = new GameObject[aCollider.transform.childCount];


        Debug.Log($"MoveCollider Count : {aCollider.transform.childCount}\n aCollider.name : {aCollider.name}");

        for (int i=0; i < animObject.Length; i++)
        {
            animObject[i] = aCollider.transform.GetChild(i).gameObject;
            Debug.Log(animObject[i].name);
        }

        for (int i=0; i< cColliderNames.Count; i++)
        {
            for (int j=0; j < animObject.Length; j++)
            {
                if (cColliderNames[i].Equals(animObject[j].name))
                {
                    animObject[j].SetActive(false);
                    Debug.Log($"animObject : {animObject[j].name}");
                    Debug.Log($"Collidername : {cColliderNames[i]}");
                }
            }
            
        }
    }

}
