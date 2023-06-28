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

    public List<Dictionary<string, object>> script;

    public int[] startIdxArr;
    public string[] pointArr;

    [Header("완료체크 확인 배열")]
    [Tooltip("스크립트 Collider 체크 변수/이미 보여줬다면 True, 아니라면 False")]
    public bool[] checkScriptComplete;

    [Header("스크립트용 연결")]
    public Text scriptText;

    public bool isClick = false;

    void Awake()
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
            }
            
        }


        //중복제거
        startIdxArr = startIdxArr.Distinct().ToArray();
        pointArr = pointArr.Distinct().ToArray();
        scriptText.text = " ";

        checkScriptComplete = new bool[startIdxArr.Length];


        //체크 배열 초기화
        for (int i = 0; i < startIdxArr.Length; i++)
        {
            checkScriptComplete[i] = false;
        }

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

    public bool[] GetCheckScriptComplete()
    {
        return checkScriptComplete;
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
                if (j == sText.Length - 1)
                {
                    scriptText.text += sText[j]; // '/'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                }
                else
                {
                    scriptText.text += (sText[j] + "\n");
                }
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

    public void IsClicked()
    {
        isClick = true;
    }

}