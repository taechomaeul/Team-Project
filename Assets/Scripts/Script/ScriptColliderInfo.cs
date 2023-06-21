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

    [SerializeField]
    private ShowScript showScript;
    [SerializeField]
    private ActionFuntion actionFunction;
    private ToggleManager toggleManager;

    private void Start()
    {
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        toggleManager = GameObject.Find("ToggleManager").GetComponent<ToggleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //현재 Collider의 이름으로 인덱스를 받아온다
            index = showScript.GetIndex(colliderName);
            curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
            nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기 

            ConditionMove();
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
        actionFunction.PauseGameForAct();

        //인덱스로 스크립트를 불러온다
        showScript.LoadScript(curIndex);
        curIndex++;
    }

    bool isStay = false;

    private void Update()
    {
        if (isStay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (curIndex < nextIndex)
                {
                    Debug.Log(curIndex);
                    //StartCoroutine(WaitSecondsFunction(1f));
                    showScript.LoadScript(curIndex);
                    curIndex++;
                }
                else
                {
                    //StartCoroutine(WaitSecondsFunction(1f));
                    scriptPanel.SetActive(false);

                    actionFunction.RestartGame();
                    isShowed = true;
                }
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
                Debug.Log(curIndex);
                    //StartCoroutine(WaitSecondsFunction(1f));
                    showScript.LoadScript(curIndex);
                    curIndex++;
                    Debug.Log("aaaaaaaaaaa");
                }
                else
                {
                    Debug.Log("dsfdsaf");
                    //StartCoroutine(WaitSecondsFunction(1f));
                    scriptPanel.SetActive(false);

                    actionFunction.RestartGame();
                    isShowed = true;
                }
                //}
                
            }*/
        }
    }

/*    IEnumerator WaitNSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator WaitSecondsFunction(float time)
    {
        yield return StartCoroutine(WaitNSeconds(time));
    }*/


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isShowed)
        {
            Destroy(gameObject);
            //Destroy(gameObject.GetComponent<SphereCollider>());
            //Destroy(gameObject.GetComponent<BoxCollider>());
            showScript.isClick = false;

            if (colliderName.Equals("T_PUZZLE") && !toggleManager.isClear)
            {
                Debug.Log("T_PUZZLE");
                actionFunction.PauseGameForAct();
                toggleManager.PlayToggleGame();
            }
        }

    }

}
