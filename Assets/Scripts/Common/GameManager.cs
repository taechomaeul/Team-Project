using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        if (Time.timeScale == 0)
        {
            //ResumeTheGame(); //일시정지 후 메인으로 온 뒤 게임 로드하면 시간 정지되는 문제
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 타이틀(로비)으로 이동하는 함수
    /// </summary>
    public void MoveToTitleScene()
    {
        Cursor.visible = true;
        SoundManager.Instance.BGMChangeWithFade(0);
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// PushGame으로 이동하는 함수
    /// </summary>
    public void MoveToPush()
    {
        SoundManager.Instance.BGMChangeWithFade(1);
        SceneManager.LoadScene("GeneratePush");
    }

    /// <summary>
    /// 1층 씬으로 이동하는 함수
    /// </summary>
    public void MoveTo1FScene()
    {

        int temp = SaveManager.Instance.saveClass.GetLastSavePosition();
            if(temp==8)
        {
            SoundManager.Instance.BGMChangeWithFade(2);
        }else if(temp>=0 && temp <= 4)
        {
        SoundManager.Instance.BGMChangeWithFade(1);
        }
        Cursor.visible = false;
        SceneManager.LoadScene("TestScene_1F");
    }

    /// <summary>
    /// 2층 씬으로 이동하는 함수
    /// </summary>
    public void MoveTo2FScene()
    {
        int temp = SaveManager.Instance.saveClass.GetLastSavePosition();
        if(temp==9)
        {
            SoundManager.Instance.BGMChangeWithFade(3);
        }else if(temp>=5 && temp <= 7)
        {
        SoundManager.Instance.BGMChangeWithFade(1);
        }
        Cursor.visible = false;
        SceneManager.LoadScene("TestScene_2F");
    }

    /// <summary>
    /// 엔딩씬으로 이동하는 함수
    /// </summary>
    public void MoveToEnding()
    {
        SoundManager.Instance.BGMChangeWithFade(4);
        SceneManager.LoadScene("EndingScene");
    }

    /// <summary>
    /// 중간 보스 연출을 위한 씬으로 이동하는 함수
    /// </summary>
    public void MoveToMidBossScene()
    {
        SoundManager.Instance.BGMStop();
        SceneManager.LoadScene("AnimMiddleBossCutScenes");
    }

    /// <summary>
    /// 최종 보스 연출을 위한 씬으로 이동하는 함수
    /// </summary>
    public void MoveToFinalBossScene()
    {
        SoundManager.Instance.BGMStop();
        SceneManager.LoadScene("AnimMainBossCutScenes");
    }

    /// <summary>
    /// 게임종료 함수
    /// </summary>
    public void QuitTheGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 일지 켰을 때 게임진행을 멈추기 위한 함수
    /// </summary>
    public void PauseTheGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    /// <summary>
    /// 일지를 종료했을 때 게임을 재개하기 위한 함수
    /// </summary>
    public void ResumeTheGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
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
            GameObject goToNewGamePanel = GameObject.Find("Canvas").transform.GetChild(0).GetChild(2).gameObject;
            goToNewGamePanel.SetActive(true); //새게임으로 가도록 유도하는 패널 ON
        }

        else
        {
            //인덱스에 따른 1층 위치
            int[] floorArr = { 1, 1, 1, 1, 1, 2, 2, 2, 1, 2 };

            if (floorArr[savePoint] == 1)
            {
                Debug.Log($"MOVE {floorArr[savePoint]}F, savePoint : {savePoint}");
                MoveTo1FScene();
            }
            else if (floorArr[savePoint] == 2)
            {
                Debug.Log($"MOVE {floorArr[savePoint]}F, savePoint : {savePoint}");
                MoveTo2FScene();
            }
        }

        
    }
}
