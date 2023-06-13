using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float curTime = 0;
    public void CountSeconds(float coolTime)
    {
        StartCoroutine(Counting(coolTime));
    }

    IEnumerator Counting(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
    }
}
