using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PushGameScript : MonoBehaviour
{

    [Header("연결 필수")]
    public string colliderName;
    public GameObject scriptPanel;

    [Header("스크립트용 연결")]
    public Text scriptText;

    [Header("데이터 연결")]
    public string scriptPath;
    public List<Dictionary<string, object>> script;

    [Header("---연결 X---")]
    public int index;
    public int curIndex;
    public int nextIndex;
    private readonly float moveSpd = 10f;
    public bool isShowed = false;
    private bool isStay = false;
    private PlayerInfo plInfo;

    private int[] startIdxArr;
    private string[] pointArr;



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

        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (isStay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (curIndex < nextIndex)
                {
                    LoadScript(curIndex);
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

    public int GetIndex(string type)
    {
        int index = 0;

        for (int i = 0; i < startIdxArr.Length; i++)
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

    public void ConditionMove()
    {
        scriptPanel.SetActive(true);
        PauseGameForAct();

        //인덱스로 스크립트를 불러온다
        LoadScript(curIndex);
        curIndex++;
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
        else
        {
            //인덱스 초기화
            index = 0;
            curIndex = 0;
            nextIndex = 0;

            isShowed = false; //저장은 다시 할 수 있으므로 저장 완료 스크립트를 보여줘야 함
        }

    }


    public void PauseGameForAct()
    {
        plInfo.plMoveSpd = 0;
    }

    public void RestartGame()
    {
        plInfo.plMoveSpd = moveSpd;
    }
}
