using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public ToggleGame toggleGame;
    public ActionFuntion actionFuntion;

    [Header("연결 필수")]
    public GameObject togglePanel;
    public GameObject clearPanel;

    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;

    public bool isClear = false;

    void Start()
    {
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
    }

    
    void Update()
    {
        if (img1.color == Color.red && img2.color == Color.red 
            && img3.color == Color.red && img4.color == Color.red && img5.color == Color.red)
        {
            isClear = true;
            Debug.Log("CLEAR!!!!!!!!!!!!!!!!");
            StartCoroutine(WaitClearFunction());
            actionFuntion.RestartGame();
            enabled = false;
        }
    }

    public void ResetPuzzle()
    {
        img1.color = Color.white;
        toggleGame = img1.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        img2.color = Color.white;
        toggleGame = img2.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        img3.color = Color.white;
        toggleGame = img3.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        img4.color = Color.white;
        toggleGame = img4.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;

        img5.color = Color.white;
        toggleGame = img5.transform.GetComponentInParent<ToggleGame>();
        toggleGame.thisFlag = false;


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
    }

    public void PlayToggleGame()
    {
        togglePanel.SetActive(true);
    }
}
