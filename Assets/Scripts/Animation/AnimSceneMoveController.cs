using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AnimSceneMoveController : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private GameManager gameManager;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Debug.Log($"PlayableDirector.Duration : {playableDirector.duration}");
    }

    void Update()
    {
        //Debug.Log($"CurTIme : {playableDirector.time}");
        if (playableDirector.time >= playableDirector.duration)
        {
            Scene scene = SceneManager.GetActiveScene(); //현재 씬이름을 불러오기 위한 변수 선언
            //Debug.Log($"Scene Name : {scene.name}");
            if (scene.name.Equals("AnimMainBossCutScenes"))
            {
                gameManager.MoveTo2FScene();
            }
            else if (scene.name.Equals("AnimMiddleBossCutScenes"))
            {
                gameManager.MoveTo1FScene();
            }

            enabled = false;
        }
    }


}
