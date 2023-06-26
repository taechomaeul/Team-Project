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
                case "TestScene_2F":
                    gameManager.MoveTo2FScene();
                    break;

                case "PushGameScene":
                    gameManager.MoveToPush();
                    break;

                case "EndingScene":
                    gameManager.MoveToEnding();
                    break;
            }
        }
    }
}
