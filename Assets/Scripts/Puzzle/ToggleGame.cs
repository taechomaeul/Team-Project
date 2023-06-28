using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGame : MonoBehaviour
{
    public bool thisFlag = false;
    public GameObject fire;

    void Start()
    {
        fire = gameObject.transform.GetChild(0).gameObject;
    }

    public void ChangeFlag()
    {
        bool flag = thisFlag;
        if (flag == true)
        {
            fire.SetActive(false);
        }
        else
        {
            fire.SetActive(true);
            fire.GetComponent<ParticleSystem>().Play();
        }
        thisFlag = !flag;
    }


}
