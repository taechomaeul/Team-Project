using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScene : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("이동하고자 하는 씬 이름")]
    public string colliderName;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (colliderName) //충돌한 Collider 이름이 특정 씬 이름과 같다면, 씬 이동.
            {
                case "AnimMiddleBossCutScenes":
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowScript>().checkScriptComplete);
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowRecord>().checkRecordComplete);
                    gameManager.MoveToMidBossScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "PushGameScene":
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowScript>().checkScriptComplete);
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowRecord>().checkRecordComplete);
                    gameManager.MoveToPush();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "TestScene_2F":
                    SaveManager.Instance.saveClass.SetLastSavePosition(5);
                    //SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowScript>().checkScriptComplete);
                    //SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowRecord>().checkRecordComplete);
                    gameManager.MoveTo2FScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "AnimMainBossCutScenes":
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowScript>().checkScriptComplete);
                    SaveManager.Instance.saveClass.SetScriptData(GameObject.Find("ActionFunction").GetComponent<ShowRecord>().checkRecordComplete);
                    gameManager.MoveToFinalBossScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "EndingScene":
                    gameManager.MoveToEnding();
                    //SaveManager.Instance.LoadSaveData();
                    break;
            }
        }
    }
}
