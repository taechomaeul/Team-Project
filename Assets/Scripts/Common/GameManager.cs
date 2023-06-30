using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    /// <summary>
    /// PushGame으로 이동하는 함수
    /// </summary>
    public void MoveToPush()
    {
        SceneManager.LoadScene("PushGameScene");
    }

    /// <summary>
    /// 1층 씬으로 이동하는 함수
    /// </summary>
    public void MoveTo1FScene()
    {
        SceneManager.LoadScene("TestScene_1F");
    }

    /// <summary>
    /// 2층 씬으로 이동하는 함수
    /// </summary>
    public void MoveTo2FScene()
    {
        SceneManager.LoadScene("TestScene_2F");
    }

    /// <summary>
    /// 엔딩씬으로 이동하는 함수
    /// </summary>
    public void MoveToEnding()
    {
        SceneManager.LoadScene("EndingScene");
    }

    /// <summary>
    /// 중간 보스 연출을 위한 씬으로 이동하는 함수
    /// </summary>
    public void MoveToMidBossScene()
    {
        SceneManager.LoadScene("AnimMiddleBossCutScenes");
    }

    /// <summary>
    /// 최종 보스 연출을 위한 씬으로 이동하는 함수
    /// </summary>
    public void MoveToFinalBossScene()
    {
        SceneManager.LoadScene("AnimMainBossCutScenes");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 일지 켰을 때 게임진행을 멈추기 위한 함수
    /// </summary>
    public void PauseTheGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// 일지를 종료했을 때 게임을 재개하기 위한 함수
    /// </summary>
    public void ResumeTheGame()
    {
        Time.timeScale = 1f;
    }
}
