using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    [Header("연결 필수")]
    public GameObject mainCanvas;
    public GameObject togglePanel;
    public GameObject clearPanel;
    public GameObject toggleCamera;

    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject fire4;
    public GameObject fire5;

    [Header("플래그 변수")]
    public bool isClear = false;

    private ToggleGame toggleGame;
    private ActionFuntion actionFuntion;

    void Start()
    {
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    
    void Update()
    {
        if (fire1.activeSelf && fire2.activeSelf && fire3.activeSelf && fire4.activeSelf && fire5.activeSelf)
        {
            isClear = true;
            Debug.Log("CLEAR!!!!!!!!!!!!!!!!");
            StartCoroutine(WaitClearFunction());
            enabled = false;
        }
    }

    public void ResetPuzzle()
    {
        //toggleGame = togglePanel.transform.GetChild(0).GetComponentInParent<ToggleGame>();
        toggleGame = fire1.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        //toggleGame = togglePanel.transform.GetChild(1).GetComponentInParent<ToggleGame>();
        toggleGame = fire2.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        //toggleGame = togglePanel.transform.GetChild(2).GetComponentInParent<ToggleGame>();
        toggleGame = fire3.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        //toggleGame = togglePanel.transform.GetChild(3).GetComponentInParent<ToggleGame>();
        toggleGame = fire4.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        //toggleGame = togglePanel.transform.GetChild(4).GetComponentInParent<ToggleGame>();
        toggleGame = fire5.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        fire1.SetActive(false);
        fire2.SetActive(false);
        fire3.SetActive(false);
        fire4.SetActive(false);
        fire5.SetActive(false);

    }

    public IEnumerator WaitClearFunction()
    {
        yield return StartCoroutine(ClearToggleGame());
    }

    public IEnumerator ClearToggleGame()
    {
        yield return new WaitForSeconds(1f);
        clearPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        togglePanel.SetActive(false);
        clearPanel.SetActive(false);
        actionFuntion.RestartGame();
        toggleCamera.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void PlayToggleGame()
    {
        mainCanvas.SetActive(false);
        toggleCamera.SetActive(true);
        togglePanel.SetActive(true);
    }
}
