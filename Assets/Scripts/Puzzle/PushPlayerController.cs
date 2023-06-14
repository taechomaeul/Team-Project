using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerController : MonoBehaviour
{
    [Header("시점 및 이동 변수")]
    public float rotSpd = 2f;
    public const float moveSpd = 10f; //(고정) 움직임 속도, 현재 움직이는 속도는 plInfo에서 확인

    [Header("기타 변수")]
    public Transform childTransform;
    public PlayerInfo plInfo;
    public CharacterController characterController;

    public enum PL_STATE
    {
        IDLE,
        MOVE, //달리기
        WALK, //(밀 때?)
        ACT, //상호작용
    }
    public PL_STATE plState;

    void Start()
    {
        plInfo = GetComponent<PlayerInfo>();
        childTransform = transform.GetChild(0).transform;
        characterController = GetComponentInChildren<CharacterController>();

        plInfo.plMoveSpd = moveSpd;
    }

    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= plInfo.plMoveSpd;

        if (moveDirection != Vector3.zero)
        {
            childTransform.rotation = Quaternion.Lerp(childTransform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotSpd);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        

    }
}
