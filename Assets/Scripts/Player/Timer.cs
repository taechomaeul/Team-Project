using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float curTime = 0;
    public bool CountSeconds(float coolTime)
    {
        curTime += Time.deltaTime;
        if (curTime > coolTime)
        {
            curTime = 0;
            return true;
        }
        else { return false; }
    }
}
