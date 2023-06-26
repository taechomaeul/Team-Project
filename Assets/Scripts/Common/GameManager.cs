using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void MoveToPush()
    {
        SceneManager.LoadScene("PushGameScene");
    }

    public void MoveTo1FScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void MoveTo2FScene()
    {
        SceneManager.LoadScene("TestScene_2F");
    }

    public void MoveToEnding()
    {
        SceneManager.LoadScene("EndingScene");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
