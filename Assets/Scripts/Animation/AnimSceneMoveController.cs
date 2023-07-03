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
    }

    void Update()
    {
        if (playableDirector.time >= playableDirector.duration)
        {
            Scene scene = SceneManager.GetActiveScene(); //현재 씬이름을 불러오기 위한 변수 선언
            if (scene.name.Equals("AnimMainBossCutScenes"))
            {
                //최종보스 연출 후 시작할 위치로 index 변경
                SaveManager.Instance.saveClass.SetLastSavePosition(9);
                gameManager.MoveTo2FScene();
            }
            else if (scene.name.Equals("AnimMiddleBossCutScenes"))
            {
                //중간보스 연출 후 시작할 위치로 index 변경
                SaveManager.Instance.saveClass.SetLastSavePosition(8);
                gameManager.MoveTo1FScene();
            }

            enabled = false;
        }
    }


}
