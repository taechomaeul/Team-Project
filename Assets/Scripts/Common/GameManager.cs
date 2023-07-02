using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 타이틀(로비)으로 이동하는 함수
    /// </summary>
    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
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

    /// <summary>
    /// 게임종료 함수
    /// </summary>
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

    /// <summary>
    /// 기존 게임을 불러오기 위한 함수
    /// </summary>
    public void LoadGame()
    {
        SaveManager.Instance.LoadSaveData();
        int savePoint = SaveManager.Instance.saveClass.GetLastSavePosition();
        Debug.Log($"SavePoint : {savePoint}");

        if (!SaveManager.Instance.SaveFileExistCheck()) //저장된 세이브 파일이 없다면
        {
            GameObject goToNewGamePanel = GameObject.Find("Canvas").transform.GetChild(0).GetChild(3).gameObject;
            goToNewGamePanel.SetActive(true); //새게임으로 가도록 유도하는 패널 ON
        }

        else
        {
            //인덱스에 따른 1층 위치
            int[] floorArr = { 0, 1, 1, 1, 1, 1, 2, 2, 2, 1, 2 };

            if (floorArr[savePoint] == 1)
            {
                MoveTo1FScene();
            }
            else if (floorArr[savePoint] == 2)
            {
                MoveTo2FScene();
            }
        }

        
    }
}
