using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyShowScript : MonoBehaviour
{
    public string pointName;
    public GameObject scriptPanel;
    public GameObject endingPanel;

    private ShowScript showScript;
    private int index;
    private int curIndex;
    private int nextIndex;

    void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();

        index = showScript.GetIndex(pointName);
        curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
        nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기

        ConditionMove();
    }

    private void Update()
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
                endingPanel.SetActive(true);
            }
        }
    }

    public void ConditionMove()
    {
        scriptPanel.SetActive(true);

        //인덱스로 스크립트를 불러온다
        showScript.LoadScript(curIndex);
        curIndex++;
    }
}
