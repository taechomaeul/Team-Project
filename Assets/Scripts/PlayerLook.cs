using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float sensitivity = 500f;
    public float rotationX;
    public float rotationY;

    public PlayerInfo plInfo;


    void Start()
    {
        plInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    void Update()
    {
        float mouseMoveValueX = Input.GetAxis("Mouse X");
        //float mouseMoveValueY = Input.GetAxis("Mouse Y");

        rotationY += mouseMoveValueX * sensitivity * Time.deltaTime;
        //rotationX += mouseMoveValueY * sensitivity * Time.deltaTime;

        //if (rotationX > 5f) { rotationX = 5f; }
        //if (rotationX < -5f) { rotationX = -5f; }
        //상하 위치 제한

        transform.eulerAngles = new Vector3(-rotationX, rotationY, 0);
    }
}
