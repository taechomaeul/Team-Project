using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGame : MonoBehaviour
{
    public bool thisFlag = false;

    //public Image fire;
    public GameObject fire;

    void Start()
    {
        //fire = transform.GetChild(0).gameObject.GetComponent<Image>();
        fire = transform.GetChild(0).gameObject;
    }

    
    void Update()
    {
        
    }

    public void ChangeFlag()
    {
        bool flag = thisFlag;
        //Color color;
        if (flag == true)
        {
            //ON
            //color = Color.white;
            //fire.color = color;
            fire.SetActive(true);
        }
        else
        {
            //color = Color.red;
            //fire.color = color;
            fire.SetActive(false);
        }
        thisFlag = !flag;
    }


}
