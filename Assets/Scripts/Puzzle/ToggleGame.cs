using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGame : MonoBehaviour
{
    public bool thisFlag = false;

    public Image fire;

    void Start()
    {
        fire = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    
    void Update()
    {
        
    }

    public void ChangeFlag()
    {
        bool flag = thisFlag;
        Color color;
        if (flag == true)
        {
            //ON
            color = Color.white;
            fire.color = color;
        }
        else
        {
            color = Color.red;
            fire.color = color;
        }
        thisFlag = !flag;
    }


}
