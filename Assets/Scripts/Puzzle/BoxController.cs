using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [Tooltip("제일 처음 배치되는 위치")]
    public Vector3 originPos;
    public PushPlayerController pPlayerController;

    private void Start()
    {
        pPlayerController = GameObject.Find("Player").GetComponent<PushPlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            pPlayerController.plState = PushPlayerController.PL_STATE.PUSH; //미는 상태로 변경
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | 
                    ~RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }


    public void ResetBox()
    {
        transform.position = originPos;
    }
}
