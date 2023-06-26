using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class ShowScript : MonoBehaviour
{
    public string scriptPath;
    public string recordPath;
    public string tipPath;

    public List<Dictionary<string, object>> script;
    public List<Dictionary<string, object>> record;
    public List<Dictionary<string, object>> tip;

    public int[] startIdxArr;
    public string[] pointArr;

    [Header("스크립트용 연결")]
    public Text scriptText;
    public Text recordNameText;
    public Text recordText;

    [Header("일지용 연결")]
    [Tooltip("일지 제목 Prefab")]
    public GameObject recordNamePrefab;
    [Tooltip("일지 제목 Prefab")]
    public GameObject tipNamePrefab;
    [Tooltip("누군가의 일지 Panel")]
    public GameObject someonePanel;
    [Tooltip("나의 일지 Panel")]
    public GameObject tipPanel;

    public bool isClick = false;

    void Start()
    {
        script = CSVReader.Read(scriptPath);
        DOTween.Init();

        startIdxArr = new int[script.Count];
        pointArr = new string[script.Count];

        //StartIdx 배열 생성
        for (int i=0; i< script.Count; i++)
        {           
            int result;
            if (int.TryParse(script[i]["START_IDX"].ToString(), out result))
            {
                startIdxArr[i] = int.Parse(script[i]["START_IDX"].ToString());
                pointArr[i] = script[i]["POINT"].ToString();
                //Debug.Log($"pointArr[{i}] : {pointArr[i]}");
            }
            
        }

        //중복제거
        startIdxArr = startIdxArr.Distinct().ToArray();
        pointArr = pointArr.Distinct().ToArray();
        scriptText.text = " ";
        recordText.text = " ";

        /*//전체 출력 (확인용)
        for (int i=0; i < startIdxArr.Length; i++)
        {
            int s_Index = startIdxArr[i];
            int e_Index;

            if (i == startIdxArr.Length-1) { e_Index = script.Count; }
            else { e_Index = startIdxArr[i + 1]; }

            StartCoroutine(LoadScriptData(s_Index, e_Index));
        }*/
    }


    public int GetIndex(string type)
    {
        int index = 0;

        for (int i=0; i<startIdxArr.Length; i++)
        {
            if (type.Equals(pointArr[i]))
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public int GetStartIndex(int index)
    {
        return startIdxArr[index];
    }

    public int GetEndIndex(int index)
    {
        int e_Index;

        if (index == startIdxArr.Length - 1) { e_Index = script.Count; }
        else { e_Index = startIdxArr[index + 1]; }

        return e_Index;
    }

    IEnumerator WaitNSeconds(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }

    IEnumerator WaitSecondsFunction(float time)
    {
        yield return StartCoroutine(WaitNSeconds(time));
    }


    public void LoadScript(int curIndex)
    {
        StartCoroutine(LoadScriptData(curIndex));
    }

    public IEnumerator LoadScriptData(int curIndex)
    {
        yield return StartCoroutine(LoadScriptDataFromCSV(curIndex));
    }

    /// <summary>
    /// 스크립트 대사를 읽어오는 함수
    /// </summary>
    /// <param name="curIndex">현재 인덱스</param>
    /// <returns></returns>
    public IEnumerator LoadScriptDataFromCSV(int curIndex)
    {
        scriptText.text = script[curIndex]["CONTEXT"].ToString();
        if (scriptText.text.Contains("/"))
        {
            string[] sText = scriptText.text.Split("/");
            scriptText.text = "";
            for (int j = 0; j < sText.Length; j++)
            {
                scriptText.text += (sText[j] + "\n");
            }
        }
        Debug.Log($"Type: {GetScriptType(curIndex)}");
        Debug.Log($"{scriptText.text}");

        yield return StartCoroutine(WaitSecondsFunction(1f));
    }

    public string GetScriptType(int index)
    {
        string type = script[index]["SYSTEM/SCRIPT"].ToString();
        return type;
    }


    public void LoadRecordName()
    {
        record = CSVReader.Read(recordPath);

        for (int i = 0; i < record.Count; i++)
        {
            string recordName = record[i]["RECORD_NAME"].ToString(); //이름만 불러온다.
            GameObject newRecord = Instantiate(recordNamePrefab); //prefab 생성
            //Debug.Log($"gameObject name : {EventSystem.current.currentSelectedGameObject.name}");
            if (EventSystem.current.currentSelectedGameObject.name.Contains("Someone")) //prefab 위치 고정 someonePanel
            {
                newRecord.transform.SetParent(someonePanel.transform);
                //Debug.Log($"NewRecord.Transform.parent : {someonePanel.transform}");
            }
            else if (gameObject.name.Contains("Tip")) //prefab 위치 고정 tipPanel
            {
                newRecord.transform.SetParent(tipPanel.transform);
                //Debug.Log($"NewRecord.Transform.parent : {tipPanel.transform}");
            }

            newRecord.name = recordName; //이름 변경
            newRecord.transform.GetChild(0).GetComponent<Text>().text = recordName; //내용 변경
        }
    }

    public void LoadTipName()
    {
        tip = CSVReader.Read(tipPath);

        for (int i = 0; i < tip.Count; i++)
        {
            string tipName = tip[i]["TIP_NAME"].ToString(); //이름만 불러온다.
            GameObject newRecord = Instantiate(tipNamePrefab); //prefab 생성
            //Debug.Log($"gameObject name : {EventSystem.current.currentSelectedGameObject.name}");
            if (EventSystem.current.currentSelectedGameObject.name.Contains("Tip")) //prefab 위치 고정 tipPanel
            {
                newRecord.transform.SetParent(tipPanel.transform);
                //Debug.Log($"NewRecord.Transform.parent : {tipPanel.transform}");
            }

            newRecord.name = tipName; //이름 변경
            newRecord.transform.GetChild(0).GetComponent<Text>().text = tipName; //내용 변경
        }
    }

    public IEnumerator LoadRecordData(string colliName)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName));
    }

    public IEnumerator LoadRecordData(string colliName, Text context)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName, context));
    }

    public IEnumerator LoadRecordDataFromCSV(string colliName)
    {
        record = CSVReader.Read(recordPath);
        recordNameText.text = colliName;
        for (int i = 0; i < record.Count; i++)
        {
            if (colliName.Equals(record[i]["RECORD_NAME"]))
            {
                recordText.text = record[i]["CONTEXT"].ToString();
                if (recordText.text.Contains("/"))
                {
                    string[] sText = recordText.text.Split("/");
                    recordText.text = " ";
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
                    //Debug.Log($"{recordText.text}");
                    break;
                }
            }

        }
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator LoadRecordDataFromCSV(string colliName, Text context)
    {
        record = CSVReader.Read(recordPath);
        context.text = colliName;
        for (int i = 0; i < record.Count; i++)
        {
            if (colliName.Equals(record[i]["RECORD_NAME"]))
            {
                context.text = record[i]["CONTEXT"].ToString();
                if (context.text.Contains("/"))
                {
                    string[] sText = context.text.Split("/");
                    context.text = " ";
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

    public IEnumerator LoadTipData(string colliName, Text context)
    {
        yield return StartCoroutine(LoadTipDataFromCSV(colliName, context));
    }

    public IEnumerator LoadTipDataFromCSV(string colliName, Text context)
    {
        tip = CSVReader.Read(tipPath);
        context.text = colliName;
        for (int i = 0; i < tip.Count; i++)
        {
            if (colliName.Equals(tip[i]["TIP_NAME"]))
            {
                context.text = tip[i]["CONTEXT"].ToString();
                if (context.text.Contains("/"))
                {
                    string[] sText = context.text.Split("/");
                    context.text = " ";
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
            for (int i = 0; i < record.Count; i++)
            {
                Destroy(someonePanel.transform.GetChild(i).gameObject);
            }
        }
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

    public void IsClicked()
    {
        isClick = true;
    }



}