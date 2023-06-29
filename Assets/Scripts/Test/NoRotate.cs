using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour
{

    private Transform cameraTransform;
    private Quaternion playerRot;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        playerRot = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.rotation = Quaternion.Euler(90f, playerRot.y, 0f);
    }
}
