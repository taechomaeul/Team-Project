using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyShowScript : MonoBehaviour
{
    [Tooltip("스크립트 포인트(POINT) 이름")]
    public string pointName;
    [Tooltip("스크립트 출력 패널")]
    public GameObject scriptPanel;
    [Tooltip("튜토리얼 엔딩 출력 패널")]
    public GameObject endingPanel;

    private ShowScript showScript;
    private int index;
    private int curIndex;
    private int nextIndex;
    [SerializeField]
    private int langOffset;

    void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();

        index = showScript.GetIndex(pointName);
        curIndex = showScript.GetStartIndex(index); //Start인덱스 구해오기
        nextIndex = showScript.GetEndIndex(index); //다음 인덱스의 Start인덱스가져오기

        string lang = "EN"; //settingManager에서 끌어올 수 있게 만들어줌
        if (lang.Equals("KR"))
        {
            langOffset = 0;
        }
        else if (lang.Equals("EN"))
        {
            langOffset = 63;
        }
        
        ConditionMove();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (curIndex < nextIndex)
            {
                showScript.LoadScript(curIndex, langOffset);
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
        showScript.LoadScript(curIndex, langOffset);
        curIndex++;
    }
}
