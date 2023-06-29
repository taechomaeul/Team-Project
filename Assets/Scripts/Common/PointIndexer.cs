using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointIndexer : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<PointIndexer>();
        if (obj.Length == 1) //PointIndexer 중복 방지
        {
            //Debug.Log("DontDestroyOnLoad");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }
}
