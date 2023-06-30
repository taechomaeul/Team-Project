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

    private SaveManager saveManager;


    void Start()
    {
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
                saveManager.SaveCurrentDataToClass(curAreaIndex);

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
