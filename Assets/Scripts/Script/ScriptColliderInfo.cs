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
    [SerializeField]
    private ActionFuntion actionFunction;
    //private ToggleManager toggleManager;

    private void Start()
    {
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        //toggleManager = GameObject.Find("ToggleManager").GetComponent<ToggleManager>();
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
    IEnumerator IsBtnClick()
    {
        yield return new WaitUntil(() => showScript.isClick == true);
        showScript.isClick = false;
    }

    public void ConditionMove()
    {
        scriptPanel.SetActive(true);
        //Debug.Log("PauseGameForAct");
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
            curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
            nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기
            //Debug.Log($"Enter/ Index: {index} | curIndex: {curIndex} | nextIndex: {nextIndex}");
            showScript.isClick = false;

            if (colliderName != "SAVE_A")
            {
                ConditionMove();
            }

            //ConditionMove();
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (colliderName.Equals("SAVE_A"))
            {
                SaveController saveController = GetComponent<SaveController>();
                if (saveController.curAreaIndex != 0 && !isShowed)
                {
                    ConditionMove();
                    isShowed = true;
                }
            }


            if (showScript.isClick)
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

            /*
            else if (Input.GetMouseButtonDown(0))
            {
                //if (!EventSystem.current.IsPointerOverGameObject())
                //{
                //클릭 처리
                if (curIndex < nextIndex)
                {
                    showScript.LoadScript(curIndex);
                    curIndex++;
                }
                else
                {
                    scriptPanel.SetActive(false);

                    actionFunction.RestartGame();
                    isShowed = true;
                }
                //}
                
            }*/
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isShowed && !gameObject.name.Contains("SavePoint"))
        {
            Destroy(gameObject);
            //Destroy(gameObject.GetComponent<SphereCollider>());
            //Destroy(gameObject.GetComponent<BoxCollider>());
            showScript.isClick = false;
            isShowed = false;

            if (colliderName.Equals("T_PUZZLE"))
            {
                ToggleManager toggleManager = GameObject.Find("ToggleManager").GetComponent<ToggleManager>();
                if (!toggleManager.isClear)
                {
                    //Debug.Log("T_PUZZLE");
                    actionFunction.PauseGameForAct();
                    toggleManager.PlayToggleGame();
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
