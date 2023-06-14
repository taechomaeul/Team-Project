using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGame : MonoBehaviour
{
    public bool flag1 = false;

    public Image fire;

    void Start()
    {
        fire = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    
    void Update()
    {
        
    }

    public void ChangeFlag1()
    {
        bool flag = flag1;
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
        flag1 = !flag;
    }


}
