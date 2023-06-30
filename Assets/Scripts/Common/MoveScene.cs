using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScene : MonoBehaviour
{
    [Header("연결 필수")]
    [Tooltip("이동하고자 하는 씬 이름")]
    public string colliderName;

    private GameManager gameManager;
    private SaveManager saveManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (colliderName) //충돌한 Collider 이름이 특정 씬 이름과 같다면, 씬 이동.
            {
                case "AnimMiddleBossCutScenes":
                    gameManager.MoveToMidBossScene();
                    break;

                case "PushGameScene":
                    gameManager.MoveToPush();
                    break;

                case "TestScene_2F":
                    saveManager.SetSaveClass(5);
                    gameManager.MoveTo2FScene();
                    break;

                case "AnimMainBossCutScenes":
                    gameManager.MoveToFinalBossScene();
                    break;

                case "EndingScene":
                    gameManager.MoveToEnding();
                    break;
            }
        }
    }
}
