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
        if (collision.transform.CompareTag("Player")) //박스와 부딪힌 오브젝트가 플레이어라면
        {
            pPlayerController.plState = PushPlayerController.PL_STATE.PUSH; //미는 상태로 변경
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) //앞뒤로 밀기의 경우 Z축제외 프리즈
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | 
                    ~RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //옆으로 밀기의 경우 X축 제외 프리즈
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    /// <summary>
    /// Box의 위치를 처음 설정한 자리 이동시킨다.
    /// </summary>
    public void ResetBox()
    {
        transform.position = originPos;
    }
}
