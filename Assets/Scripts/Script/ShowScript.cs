using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ShowScript : MonoBehaviour
{
    [Header("스크립트 파일 경로")]
    public string scriptPath;

    [Header("스크립트용 연결")]
    public Text scriptText;

    [Header("완료체크 확인 배열")]
    [Tooltip("스크립트 Collider 체크 변수/이미 보여줬다면 True, 아니라면 False")]
    public bool[] checkScriptComplete;

    private List<Dictionary<string, object>> script;
    [SerializeField]
    private int[] startIdxArr; //인덱스(파트)의 시작 인덱스만을 모은 배열
    [SerializeField]
    private string[] pointArr; //인덱스(파트) 이름만을 모은 배열
    [SerializeField]
    private string[] langArr; //언어 이름만을 모은 배열

    public bool isClick = false;

    void Awake()
    {
        script = CSVReader.Read(scriptPath);
        DOTween.Init();

        //배열 초기화
        langArr = new string[script.Count];

        //언어 배열 생성
        for (int i = 0; i < script.Count; i++)
        {
            langArr[i] = script[i]["Language"].ToString();
        }

        //중복제거
        langArr = langArr.Distinct().ToArray();


        //배열 초기화
        startIdxArr = new int[script.Count / langArr.Length];
        pointArr = new string[script.Count / langArr.Length];

        //StartIdx 배열 생성
        for (int i=0; i< script.Count / langArr.Length; i++)
        {           
            int result;
            if (int.TryParse(script[i]["START_IDX"].ToString(), out result))
            {
                startIdxArr[i] = int.Parse(script[i]["START_IDX"].ToString());
                pointArr[i] = script[i]["POINT"].ToString();
            }
            
        }
        //Debug.Log($"Length: {startIdxArr.Length}");

        //중복제거
        startIdxArr = startIdxArr.Distinct().ToArray();
        pointArr = pointArr.Distinct().ToArray();
        scriptText.text = "";

        //스크립트 체크 배열 함수 선언
        checkScriptComplete = new bool[startIdxArr.Length];
        //Debug.Log($"startIdxArr Length : {startIdxArr.Length}");

        if (SaveManager.Instance.saveClass.GetScriptData().Length == 0)
        {
            //스크립트 체크 배열 초기화
            //Debug.Log("스크립트 체크 배열 초기화");
            for (int i = 0; i < startIdxArr.Length; i++)
            {
                checkScriptComplete[i] = false;
            }
            SaveManager.Instance.saveClass.SetScriptData(checkScriptComplete);
        }

        Debug.Log("ShowScript Read Complete");
    }

    /// <summary>
    /// Point 인덱스를 불러오는 함수
    /// </summary>
    /// <param name="type">파트(Collider) 이름</param>
    /// <returns></returns>
    public int GetIndex(string part)
    {
        int index = 0;
        for (int i = 0 ; i < startIdxArr.Length; i++)
        {
            if (part.Equals(pointArr[i]))
            {
                index = i;
                break;
            }
        }

        return index;
    }

    /// <summary>
    /// 인덱스 시작 지점을 불러오는 함수
    /// </summary>
    /// <param name="index">Point(파트) 인덱스</param>
    /// <returns></returns>
    public int GetStartIndex(int index)
    {
        return startIdxArr[index];
    }

    /// <summary>
    /// 인덱스 종료 지점(다음 Point 인덱스의 첫 지점)을 불러오는 함수 
    /// </summary>
    /// <param name="index">Point(파트) 인덱스</param>
    /// <returns></returns>
    public int GetEndIndex(int index)
    {
        int e_Index;

        if (index == startIdxArr.Length - 1) { e_Index = script.Count / langArr.Length; }
        else { e_Index = startIdxArr[index + 1]; }

        return e_Index;
    }

    /// <summary>
    /// 스크립트 Collider 확인 배열을 받아오는 함수
    /// </summary>
    /// <param name="ccScriptArr"></param>
    public void GetCheckScriptArr(bool[] ccScriptArr)
    {
        checkScriptComplete = ccScriptArr;
    }

    public void LoadScript(int curIndex, int langOffset)
    {
        StartCoroutine(LoadScriptData(curIndex, langOffset));
    }

    public IEnumerator LoadScriptData(int curIndex, int langOffset)
    {
        yield return StartCoroutine(LoadScriptDataFromCSV(curIndex, langOffset));
    }

    /// <summary>
    /// 스크립트 대사를 한줄씩 읽어오는 함수
    /// </summary>
    /// <param name="curIndex">현재 인덱스</param>
    /// <returns></returns>
    public IEnumerator LoadScriptDataFromCSV(int curIndex, int langOffset)
    {
        scriptText.text = script[curIndex + langOffset]["CONTEXT"].ToString();
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
        Debug.Log($"Type: {GetScriptType(curIndex + langOffset)}");
        Debug.Log($"{scriptText.text}");

        yield return StartCoroutine(WaitSecondsFunction(1f));
    }

    /// <summary>
    /// 스크립트의 타입을 가져오는 함수
    /// </summary>
    /// <param name="index">스크립트 인덱스</param>
    /// <returns></returns>
    public string GetScriptType(int index)
    {
        string type = script[index]["SYSTEM/SCRIPT"].ToString();
        return type;
    }

    public void IsClicked()
    {
        isClick = true;
    }

    public IEnumerator WaitNSeconds(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }

    public IEnumerator WaitSecondsFunction(float time)
    {
        yield return StartCoroutine(WaitNSeconds(time));
    }

}