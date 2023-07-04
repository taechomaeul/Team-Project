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

    [SerializeField]
    private string[] langArr; //언어 이름만을 모은 배열

    void Awake()
    {
        tip = CSVReader.Read(tipPath);
        DOTween.Init();

        //배열 초기화
        langArr = new string[tip.Count];

        //언어 배열 생성
        for (int i = 0; i < tip.Count; i++)
        {
            langArr[i] = tip[i]["Language"].ToString();
        }

        //중복제거
        langArr = langArr.Distinct().ToArray();

        checkTipComplete = new bool[tip.Count / langArr.Length];
        if (SaveManager.Instance.saveClass.GetTipData().Length == 0)
        {
            //체크 배열 초기화
            for (int i = 0; i < checkTipComplete.Length; i++)
            {
                checkTipComplete[i] = false;
            }
            SaveManager.Instance.saveClass.SetTipData(checkTipComplete);
        }

    }

    /// <summary>
    /// 나의 일지(Tip) 이름 버튼을 불러오는 함수
    /// 버튼 Prefab을 팁 개수만큼 만들어서 위치를 알맞게 고정시켜준다.
    /// </summary>
    public void LoadTipName()
    {
        //int index = 0;
        for (int i = 0; i < tip.Count; i++)
        {
            string lang = "EN"; //SettingManager에서 끌어올 수 있게 만들어줌

            if (lang.Equals(tip[i]["Language"]))
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
            string lang = "EN"; //SettingManager에서 끌어올 수 있게 만들어줌

            if (lang.Equals(tip[i]["Language"]))
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
            for (int i = 0; i < checkTipComplete.Length; i++)
            {
                Destroy(tipPanel.transform.GetChild(i).gameObject);
            }
        }
    }

}