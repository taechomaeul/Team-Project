using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SaveController : MonoBehaviour
{
    [Header("연결 필수")]
    public GameObject saveFPanel;

    [Header("연결 X")]
    public int curAreaIndex;
    public bool isSaveCompleted = false;

    private PlayerInfo plInfo;
    private ActionFuntion actionFunction;
    private SaveManager saveManager;


    void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        actionFunction = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            saveFPanel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                saveFPanel.SetActive(false);

                //플래그로 인덱스 불러온 후 저장
                curAreaIndex = int.Parse(gameObject.transform.name.Split('_')[1]); //현재 저장하려는 위치의 이름에서 인덱스 가져오자 ex SavePoint_1 -> 1
                Debug.Log($"curAreaIndex : {curAreaIndex}");
                //saveManager.SaveCurrentData(GameObject.FindGameObjectWithTag("Player").transform, plInfo.curHp, plInfo.curPrefabIndex, plInfo.curSkill.skillName);
                //영혼석의 현재 무게도 저장해야 합니다..!!

                isSaveCompleted = true; //저장 완료 플래그
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isSaveCompleted)
        {
            saveFPanel.SetActive(false);
        }

        if (other.CompareTag("Player") && isSaveCompleted)
        {
            curAreaIndex = 0; //초기화
            saveFPanel.SetActive(false);
            isSaveCompleted = false;
        }
    }
}
