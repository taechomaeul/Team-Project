using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("이동하고자 하는 씬 이름")]
    public string colliderName;

    private GameManager gameManager;
    private GameObject actionFuntion;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        actionFuntion = GameObject.Find("ActionFunction");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (colliderName) //충돌한 Collider 이름이 특정 씬 이름과 같다면, 씬 이동.
            {
                case "AnimMiddleBossCutScenes":
                    actionFuntion.GetComponent<ColliderController>().AddMoveCollider(gameObject.name);
                    //SaveManager.Instance.saveClass.SetScriptData(actionFuntion.GetComponent<ShowScript>().checkScriptComplete);
                    //SaveManager.Instance.saveClass.SetRecordData(actionFuntion.GetComponent<ShowRecord>().checkRecordComplete);
                    SaveManager.Instance.SaveCurrentDataToClass(SaveManager.Instance.saveClass.GetLastSavePosition());
                    gameManager.MoveToMidBossScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "PushGameScene":
                    actionFuntion.GetComponent<ColliderController>().AddMoveCollider(gameObject.name);
                    //SaveManager.Instance.saveClass.SetScriptData(actionFuntion.GetComponent<ShowScript>().checkScriptComplete);
                    //SaveManager.Instance.saveClass.SetRecordData(actionFuntion.GetComponent<ShowRecord>().checkRecordComplete);
                    SaveManager.Instance.saveClass.SetCurrentBodyIndex(1); //푸시 게임 이후에는 일반 몬스터로 playerPrefab을 변경시켜야 한다
                    SaveManager.Instance.SaveCurrentDataToClass(SaveManager.Instance.saveClass.GetLastSavePosition());
                    gameManager.MoveToPush();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "TestScene_2F":
                    SaveManager.Instance.saveClass.SetLastSavePosition(5);
                    if (!SceneManager.GetActiveScene().name.Equals("PushGameScene"))
                    {
                        SaveManager.Instance.saveClass.SetScriptData(actionFuntion.GetComponent<ShowScript>().checkScriptComplete);
                        SaveManager.Instance.saveClass.SetRecordData(actionFuntion.GetComponent<ShowRecord>().checkRecordComplete);
                    }
                    gameManager.MoveTo2FScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "AnimMainBossCutScenes":
                    actionFuntion.GetComponent<ColliderController>().AddMoveCollider(gameObject.name);
                    //SaveManager.Instance.saveClass.SetScriptData(actionFuntion.GetComponent<ShowScript>().checkScriptComplete);
                    //SaveManager.Instance.saveClass.SetRecordData(actionFuntion.GetComponent<ShowRecord>().checkRecordComplete);
                    SaveManager.Instance.SaveCurrentDataToClass(SaveManager.Instance.saveClass.GetLastSavePosition());
                    gameManager.MoveToFinalBossScene();
                    //SaveManager.Instance.LoadSaveData();
                    break;

                case "EndingScene":
                    actionFuntion.GetComponent<ColliderController>().AddMoveCollider(gameObject.name);
                    SaveManager.Instance.SaveCurrentDataToClass(SaveManager.Instance.saveClass.GetLastSavePosition());
                    gameManager.MoveToEnding();
                    //SaveManager.Instance.LoadSaveData();
                    break;
            }
        }
    }
}
