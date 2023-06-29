using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public GameObject p;
    Vector3 t2;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t2.x = transform.position.x;
        t2.y = 0;
        t2.z = transform.position.z;

        Vector3 vv =(p.transform.position -t2).normalized;

        Debug.Log(Vector3.Lerp(transform.forward, vv, Time.deltaTime));

        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, vv, Time.deltaTime));

    }
}
