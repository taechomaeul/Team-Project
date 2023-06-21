using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void MoveToToggle()
    {
        SceneManager.LoadScene("ToggleGameScene");
    }

    public void MoveTo1FScene()
    {
        SceneManager.LoadScene("Map1FScene");
    }

    public void MoveTo2FScene()
    {
        SceneManager.LoadScene("Map2FScene");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
