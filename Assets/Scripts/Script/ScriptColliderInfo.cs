using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ScriptColliderInfo : MonoBehaviour
{
    [Header("연결 필수")]
    public string colliderName;
    public GameObject scriptPanel;

    [Header("연결 X")]
    public int index;
    public int curIndex;
    public int nextIndex;
    public bool isShowed = false;
    private bool isStay = false;

    private ShowScript showScript;
    private ActionFuntion actionFunction;

    private void Start()
    {
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
    }

    private void Update()
    {
        if (isStay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (curIndex < nextIndex)
                {
                    if (colliderName != "SAVE_A")
                    {
                        showScript.LoadScript(curIndex);
                        curIndex++;
                    }
                }
                else
                {
                    scriptPanel.SetActive(false);

                    actionFunction.RestartGame();
                    isShowed = true;
                }
            }
        }
    }

    /// <summary>
    /// 버튼 클릭 시까지 대기했다가 (누군가의) 일지가 OFF 되면 스크립트를 출력하게 도와주는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator IsBtnClick()
    {
        yield return new WaitUntil(() => showScript.isClick == true);
        showScript.isClick = false;
    }

    public void ConditionMove()
    {
        scriptPanel.SetActive(true);
        actionFunction.PauseGameForAct();

        //인덱스로 스크립트를 불러온다
        showScript.LoadScript(curIndex);
        curIndex++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //현재 Collider의 이름으로 인덱스를 받아온다
            index = showScript.GetIndex(colliderName);

            Debug.Log($"GetIndex : {index}");
            if (showScript.checkScriptComplete[index]) //true가 아닐 때만 스크립트 읽기
            {
                gameObject.SetActive(false);
            }

            else
            {
                curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
                nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기
                showScript.isClick = false;

                if (colliderName != "SAVE_A")
                {
                    ConditionMove();
                }
            }

            
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (colliderName.Equals("SAVE_A")) //세이브 포인트에서 스크립트를 출력할 때
            {
                SaveController saveController = GetComponent<SaveController>();
                if (saveController.curAreaIndex != 0 && !isShowed) //세이브 포인트의 인덱스가 0이라면 출력하지 않음
                {
                    ConditionMove();
                    isShowed = true;
                }
            }


            if (showScript.isClick) //일지 닫기 버튼 클릭
            {
                curIndex = nextIndex;
                nextIndex = showScript.GetEndIndex(index + 1);
                StartCoroutine(IsBtnClick());

                ConditionMove();
                showScript.isClick = false;
            }
            else
            {
                isStay = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isShowed && !gameObject.name.Contains("SavePoint"))
        {
            Destroy(gameObject);
            showScript.isClick = false;
            isShowed = false;

            //스크립트 체크 완료
            showScript.checkScriptComplete[index] = true;
            Debug.Log($"CheckScriptComplete[{index}] : {showScript.checkScriptComplete[index]}");

            if (colliderName.Equals("T_PUZZLE"))
            {
                ToggleManager toggleManager = GameObject.Find("ToggleManager").GetComponent<ToggleManager>();
                if (!toggleManager.isClear) //토글 게임이 클리어 상태가 아닐 경우에만 토글 게임을 진행할 수 있어야 한다
                {
                    actionFunction.PauseGameForAct(); //토글 게임 진행을 위해서 움직임(카메라, 플레이어) 이동을 멈춘다.
                    toggleManager.PlayToggleGame(); //토글 게임 시작
                }
            }
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

}
