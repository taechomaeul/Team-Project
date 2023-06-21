using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
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

            if (Input.GetMouseButtonDown(0))
            {
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
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isShowed)
        {
            Destroy(gameObject);
            //Destroy(gameObject.GetComponent<SphereCollider>());
            //Destroy(gameObject.GetComponent<BoxCollider>());
            showScript.isClick = false;
        }

    }

}
