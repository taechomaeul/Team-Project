using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptColliderInfo : MonoBehaviour
{
    public string colliderName;
    public int curIndex;
    public int nextIndex;

    public GameObject toolTipPanel;
    public GameObject scriptPanel;
    public GameObject recordPanel; 
    
    public ShowScript showScript;
    public ActionFuntion actionFunction;

    public enum S_ENTRY_CONDITION
    {
        MOVE,
        MOVE_AND_F,
        BATTLE_END
    }
    public S_ENTRY_CONDITION condition;

    private void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (condition == S_ENTRY_CONDITION.MOVE) //진입조건이 Collider로 이동일 때
            {
                scriptPanel.SetActive(true);

                //현재 Collider의 이름으로 인덱스를 받아온다
                int index = showScript.GetIndex(colliderName);
                curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
                nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기 

                actionFunction.PauseGameForAct();

                //인덱스로 스크립트를 불러온다
                showScript.LoadScript(curIndex);
                curIndex++;

            }
            else if (condition == S_ENTRY_CONDITION.MOVE_AND_F) //진입조건이 이동후 F버튼일 때
            {
                //MOVE와 같음
                //이후 F버튼을 누르면 다시한번 인덱스의 스크립트를 받아온다
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //F버튼 종류에 따라 나뉜다 (일지일수도, 빙의 등의 기능일수도)
                }
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject.GetComponent<SphereCollider>());
        }
    }
}
