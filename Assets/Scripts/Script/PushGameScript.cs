using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PushGameScript : MonoBehaviour
{

    [Header("연결 필수")]
    [Tooltip("Collider 이름")]
    public string colliderName;
    [Tooltip("스크립트 출력 패널")]
    public GameObject scriptPanel;

    [Header("스크립트용 연결")]
    [Tooltip("스크립트 출력 텍스트")]
    public Text scriptText;

    [Header("데이터 연결")]
    public string scriptPath;
    public List<Dictionary<string, object>> script;

    [Header("---연결 X---")]
    private int index;
    private int curIndex;
    private int nextIndex;
    private readonly float moveSpd = 10f;
    public bool isShowed = false;
    private bool isStay = false;
    private PlayerInfo plInfo;

    private int[] startIdxArr;
    private string[] pointArr;
    private int langOffset;


    private void Awake()
    {
        script = CSVReader.Read(scriptPath);

        startIdxArr = new int[script.Count];
        pointArr = new string[script.Count];

        //StartIdx 배열 생성
        for (int i = 0; i < script.Count; i++)
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
        scriptText.text = "";

        //string lang = "EN"; //settingManager에서 끌어올 수 있게 만들어줌
        string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();

        if (lang.Equals("KR"))
        {
            langOffset = 0;
        }
        else if (lang.Equals("EN"))
        {
            langOffset = 63;
        }

        plInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (isStay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (curIndex < nextIndex)
                {
                    LoadScript(curIndex, langOffset);
                    curIndex++;
                }
                else
                {
                    scriptPanel.SetActive(false);

                    RestartGame();
                    isShowed = true;
                }
            }
        }
    }

    /// <summary>
    /// 스크립트 출력을 위한 인덱스를 불러오는 함수
    /// </summary>
    /// <param name="part">인덱스 포지션(POINT)</param>
    /// <returns></returns>
    public int GetIndex(string part)
    {
        int index = 0;

        for (int i = 0; i < startIdxArr.Length; i++)
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
    /// 인덱스(파트)의 시작 인덱스(전체)를 가져오는 함수
    /// </summary>
    /// <param name="index">인덱스(파트)</param>
    /// <returns></returns>
    public int GetStartIndex(int index)
    {
        return startIdxArr[index];
    }

    /// <summary>
    /// 인덱스(파트)의 끝 인덱스(전체)를 가져오는 함수
    /// 끝 인덱스는 다음 인덱스의 시작 인덱스와 같다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 조건에 따라 움직임 제어하는 함수
    /// 스크립트 패널 ON, 움직임 X, 스크립트 텍스트 불러오기
    /// </summary>
    public void ConditionMove()
    {
        scriptPanel.SetActive(true);
        PauseGameForAct();

        //인덱스로 스크립트를 불러온다
        LoadScript(curIndex, langOffset);
        curIndex++;
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
    /// 스크립트 출력 타입 함수
    /// </summary>
    /// <param name="index">스크립트 인덱스</param>
    /// <returns></returns>
    public string GetScriptType(int index)
    {
        string type = script[index]["SYSTEM/SCRIPT"].ToString();
        return type;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //현재 Collider의 이름으로 인덱스를 받아온다
            index = GetIndex(colliderName);

            curIndex = GetStartIndex(index); //Start인덱스 구해오기
            nextIndex = GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기

            ConditionMove();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStay = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 스크립트 출력을 위한 움직임 정지 함수
    /// </summary>
    public void PauseGameForAct()
    {
        plInfo.plMoveSpd = 0;
    }

    /// <summary>
    /// 게임 재개 함수
    /// </summary>
    public void RestartGame()
    {
        plInfo.plMoveSpd = moveSpd;
    }
}
