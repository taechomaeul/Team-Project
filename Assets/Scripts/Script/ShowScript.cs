using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ShowScript : MonoBehaviour
{
    public string scriptPath;
    public string recordPath;

    public List<Dictionary<string, object>> script;
    public List<Dictionary<string, object>> record;

    public int[] startIdxArr;
    public string[] pointArr;

    public Text scriptText;
    public Text recordNameText;
    public Text recordText;


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

    private void Update()
    {

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
    /// <param name="startIdx">현재 파트의 첫 시작 인덱스</param>
    /// <param name="endIndex">다음 파트의 첫 시작 인덱스</param>
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

        yield return new WaitForSeconds(2f);
    }

    public string GetScriptType(int index)
    {
        string type = script[index]["SYSTEM/SCRIPT"].ToString();
        return type;
    }

    public IEnumerator LoadRecordData(string colliName)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName));
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
                        recordText.text += (sText[j] + "\n");
                    }
                    //Debug.Log($"{recordText.text}");
                    break;
                }
            }

        }
        yield return new WaitForSeconds(2f);
    }

}