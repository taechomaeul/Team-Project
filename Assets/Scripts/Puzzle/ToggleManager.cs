using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{

    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;

    public ToggleGame toggleGame;

    public bool isClear = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (img1.color == Color.red && img2.color == Color.red 
            && img3.color == Color.red && img4.color == Color.red && img5.color == Color.red)
        {
            isClear = true;
            Debug.Log("CLEAR!!!!!!!!!!!!!!!!");
            return;
        }
    }

    public void ResetPuzzle()
    {
        img1.color = Color.white;
        toggleGame = img1.transform.GetComponentInParent<ToggleGame>();
        toggleGame.flag1 = false;

        img2.color = Color.white;
        toggleGame = img2.transform.GetComponentInParent<ToggleGame>();
        toggleGame.flag1 = false;

        img3.color = Color.white;
        toggleGame = img3.transform.GetComponentInParent<ToggleGame>();
        toggleGame.flag1 = false;

        img4.color = Color.white;
        toggleGame = img4.transform.GetComponentInParent<ToggleGame>();
        toggleGame.flag1 = false;

        img5.color = Color.white;
        toggleGame = img5.transform.GetComponentInParent<ToggleGame>();
        toggleGame.flag1 = false;


    }
}
