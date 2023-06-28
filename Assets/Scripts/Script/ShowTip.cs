using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class ShowTip : MonoBehaviour
{
    public string tipPath;

    public List<Dictionary<string, object>> tip;

    [Header("완료체크 확인 배열")]
    [Tooltip("나의 일지(Tip) Collider 체크 변수/이미 보여줬다면 True, 아니라면 False")]
    public bool[] checkTipComplete;

    [Header("일지용 연결")]
    [Tooltip("일지 제목 Prefab")]
    public GameObject tipNamePrefab;
    [Tooltip("나의 일지 Panel")]
    public GameObject tipPanel;

    void Awake()
    {

        tip = CSVReader.Read(tipPath);
        DOTween.Init();

        checkTipComplete = new bool[tip.Count];

        //체크 배열 초기화
        for (int i = 0; i < tip.Count; i++)
        {
            checkTipComplete[i] = false;
        }

    }

    public void LoadTipName()
    {
        for (int i = 0; i < tip.Count; i++)
        {
            string tipName = tip[i]["TIP_NAME"].ToString(); //이름만 불러온다.
            GameObject newRecord = Instantiate(tipNamePrefab); //prefab 생성
            if (EventSystem.current.currentSelectedGameObject.name.Contains("Tip")) //prefab 위치 고정 tipPanel
            {
                newRecord.transform.SetParent(tipPanel.transform);
            }

            newRecord.name = tipName; //이름 변경
            newRecord.transform.GetChild(0).GetComponent<Text>().text = tipName; //내용 변경
        }
    }

    public IEnumerator LoadTipData(string colliName, Text context)
    {
        yield return StartCoroutine(LoadTipDataFromCSV(colliName, context));
    }

    public IEnumerator LoadTipDataFromCSV(string colliName, Text context)
    {
        context.text = colliName;
        for (int i = 0; i < tip.Count; i++)
        {
            if (colliName.Equals(tip[i]["TIP_NAME"]))
            {
                context.text = tip[i]["CONTEXT"].ToString();
                if (context.text.Contains("/"))
                {
                    string[] sText = context.text.Split("/");
                    context.text = "";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        if (j == sText.Length - 1)
                        {
                            context.text += sText[j]; // '/'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                        }
                        else
                        {
                            context.text += (sText[j] + "\n");
                        }
                    }
                    //Debug.Log($"{context.text}");
                    break;
                }
            }

        }
        yield return new WaitForSeconds(2f);
    }

    /// <summary>
    /// 일지에서 하위 메뉴버튼을 이동하면서 Tip의 중복생성을 막기 위한 함수
    /// </summary>
    public void DeleteTipContent()
    {
        if (tipPanel.transform.childCount != 0)
        {
            for (int i = 0; i < tip.Count; i++)
            {
                Destroy(tipPanel.transform.GetChild(i).gameObject);
            }
        }
    }

}