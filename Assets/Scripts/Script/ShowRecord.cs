using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class ShowRecord : MonoBehaviour
{
    public string recordPath;

    public List<Dictionary<string, object>> record;

    public int curCheckIndex;

    [Header("완료체크 확인 배열")]
    [Tooltip("누군가의 일지(Record) Collider 체크 변수/이미 보여줬다면 True, 아니라면 False")]
    public bool[] checkRecordComplete;

    [Header("스크립트용 연결")]
    [Tooltip("일지 제목 출력 텍스트")]
    public Text recordNameText;
    [Tooltip("일지 내용 출력 텍스트")]
    public Text recordText;

    [Header("일지용 연결")]
    [Tooltip("일지 제목 Prefab")]
    public GameObject recordNamePrefab;
    [Tooltip("누군가의 일지 Panel")]
    public GameObject someonePanel;
    [Tooltip("누군가의 일지 오른쪽 패널 텍스트")]
    public Text recordContext;

    [SerializeField]
    private string[] langArr; //언어 이름만을 모은 배열

    void Awake()
    {
        record = CSVReader.Read(recordPath);
        DOTween.Init();
        recordText.text = "";

        //배열 초기화
        langArr = new string[record.Count];

        //언어 배열 생성
        for (int i = 0; i < record.Count; i++)
        {
            langArr[i] = record[i]["Language"].ToString();
        }

        //중복제거
        langArr = langArr.Distinct().ToArray();

        checkRecordComplete = new bool[record.Count / langArr.Length];
        if (SaveManager.Instance.saveClass.GetRecordData().Length == 0)
        {
            //체크 배열 초기화
            for (int i = 0; i < checkRecordComplete.Length; i++)
            {
                checkRecordComplete[i] = false;
            }
            SaveManager.Instance.saveClass.SetRecordData(checkRecordComplete);
        }
        Debug.Log("ShowRecord Read Complete");
    }

    /// <summary>
    /// 일지 Collider 확인 배열을 넘기기 위한 함수
    /// </summary>
    /// <returns></returns>
    public bool[] GetCheckRecordComplete()
    {
        return checkRecordComplete;
    }

    /// <summary>
    /// 누군가의 일지(Record) 이름 버튼을 불러오는 함수
    /// 버튼 Prefab을 일지 개수만큼 만들어서 위치를 알맞게 고정시켜준다.
    /// </summary>
    public void LoadRecordName()
    {
        for (int i = 0; i < record.Count; i++)
        {
            //string lang = "EN"; //SettingManager에서 끌어올 수 있게 만들어줌
            string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();

            if (lang.Equals(record[i]["Language"]))
            {
                string recordName = record[i]["RECORD_NAME"].ToString(); //이름만 불러온다.

                GameObject newRecord = Instantiate(recordNamePrefab); //prefab 생성
                if (EventSystem.current.currentSelectedGameObject.name.Contains("Someone")) //prefab 위치 고정 someonePanel
                {
                    newRecord.transform.SetParent(someonePanel.transform);
                }

                newRecord.name = recordName; //이름 변경
                if (recordName.Contains(":"))
                {
                    string[] sText = recordName.Split(":");
                    recordName = "";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        if (j == sText.Length - 1)
                        {
                            recordName += (": " + sText[j]); // ':'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                        }
                        else
                        {
                            recordName += (sText[j] + "\n");
                        }
                    }
                }
                newRecord.transform.GetChild(0).GetComponent<Text>().text = recordName; //내용 변경
            }
        }
    }

    /// <summary>
    /// 일지 Collider 확인 배열을 받아오는 함수
    /// </summary>
    /// <param name="ccRecordArr">ColliderController의 일지 체크 배열</param>
    public void GetCheckRecordArr(bool[] ccRecordArr)
    {
        checkRecordComplete = ccRecordArr;
    }

    public IEnumerator LoadRecordData(string colliName)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName));
    }

    public IEnumerator LoadRecordData(string colliName, Text context)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName, context));
    }

    /// <summary>
    /// 메인화면에서 일지 보여주는 용도의 함수
    /// </summary>
    /// <param name="colliName">Collider(POINT) 이름</param>
    /// <returns>일지 이름과 일치하는 Context(전체 텍스트) 반환</returns>
    public IEnumerator LoadRecordDataFromCSV(string colliName)
    {
        recordNameText.text = colliName;
        //string lang = "EN"; //SettingManager에서 끌어올 수 있게 만들어줌
        string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();

        for (int i = 0; i < record.Count; i++)
        {
            if (colliName.Equals(record[i]["RECORD_NAME"]))
            {
                //checkindex 넘겨주기 위해서 고정
                curCheckIndex = i;

                int langOffset = 0;
                if (lang.Equals("KR"))
                {
                    langOffset = 0;
                }
                else if (lang.Equals("EN"))  //EN 이면 offset 더해주기
                {
                    langOffset = checkRecordComplete.Length;
                }

                recordNameText.text = record[i + langOffset]["RECORD_NAME"].ToString();
                recordText.text = record[i + langOffset]["CONTEXT"].ToString();
                Debug.Log(record[i + langOffset]["RECORD_NAME"].ToString());
                Debug.Log(record[i + langOffset]["CONTEXT"].ToString());

                if (recordNameText.text.Contains(":"))
                {
                    string[] sText = recordNameText.text.Split(":");
                    recordNameText.text = "";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        if (j == sText.Length - 1)
                        {
                            recordNameText.text += (": " + sText[j]); // ':'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                        }
                        else
                        {
                            recordNameText.text += (sText[j] + "\n");
                        }
                    }
                }

                if (recordText.text.Contains("/"))
                {
                    string[] sText = recordText.text.Split("/");
                    recordText.text = "";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        if (j == sText.Length - 1)
                        {
                            recordText.text += sText[j]; // '/'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                        }
                        else
                        {
                            recordText.text += (sText[j] + "\n");
                        }
                    }
                }
                
            }

        }
        yield return new WaitForSeconds(2f);
    }

    /// <summary>
    /// 일지(J)에서 내용을 보여주기 위한 용도의 함수
    /// </summary>
    /// <param name="colliName">Collider(POINT) 이름</param>
    /// <param name="context">일지 출력 텍스트</param>
    /// <returns>context에 일지 이름과 일치하는 전체 텍스트 입력 후 반환</returns>
    public IEnumerator LoadRecordDataFromCSV(string colliName, Text context)
    {
        context.text = colliName;
        Debug.Log($"ColliName : {colliName}");
        for (int i = 0; i < record.Count; i++)
        {
            if (colliName.Equals(record[i]["RECORD_NAME"]))
            {
                context.text = record[i]["CONTEXT"].ToString();

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
    /// 일지에서 하위 메뉴버튼을 이동하면서 Record의 중복생성을 막기 위한 함수
    /// </summary>
    public void DeleteRecordContent()
    {
        if (someonePanel.transform.childCount != 0)
        {
            for (int i = 0; i < checkRecordComplete.Length; i++)
            {
                Destroy(someonePanel.transform.GetChild(i).gameObject);
            }
        }
    }

    public void ResetContext()
    {
        recordContext.text = "";
    }

}