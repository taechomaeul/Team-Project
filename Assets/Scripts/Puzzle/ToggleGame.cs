using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGame : MonoBehaviour
{
    public bool thisFlag = false;

    //public Image fire;
    public GameObject fire;

    private ToggleManager toggleManager;
    void Start()
    {
        //fire = transform.GetChild(0).gameObject.GetComponent<Image>();

        toggleManager = GameObject.Find("ToggleManager").GetComponent<ToggleManager>();
        
        switch (gameObject.name)
        {
            case "Button_1":
                fire = toggleManager.fire1;
                break;
            case "Button_2":
                fire = toggleManager.fire2;
                break;
            case "Button_3":
                fire = toggleManager.fire3;
                break;
            case "Button_4":
                fire = toggleManager.fire4;
                break;
            case "Button_5":
                fire = toggleManager.fire5;
                break;
        }
    }

    public void ChangeFlag()
    {
        bool flag = thisFlag;
        //Color color;
        Debug.Log(gameObject);
        Debug.Log($"This Flag : {flag}");
        if (flag == true)
        {
            //ON
            //color = Color.white;
            //fire.color = color;
            fire.SetActive(false);
        }
        else
        {
            //color = Color.red;
            //fire.color = color;

            fire.SetActive(true);
            fire.GetComponent<ParticleSystem>().Play();
        }
        thisFlag = !flag;
        Debug.Log($"Changed Flag : {thisFlag}");
    }


}
