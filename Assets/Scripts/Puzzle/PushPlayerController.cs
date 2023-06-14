using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayerController : MonoBehaviour
{
    [Header("시점 및 이동 변수")]
    public float rotSpd = 5f;
    public const float moveSpd = 10f; //(고정) 움직임 속도, 현재 움직이는 속도는 plInfo에서 확인

    [Header("기타 변수")]
    public Transform childTransform;
    public PlayerInfo plInfo;
    public CharacterController characterController;

    public enum PL_STATE
    {
        IDLE,
        MOVE, //달리기
        PUSH //밀기
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
        //이동 및 캐릭터 회전

        switch (plState)
        {
            case PL_STATE.IDLE:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) //WASD 입력이 들어오면
                {
                    plState = PL_STATE.MOVE; //움직이는 상태로 변경
                }
                break;

            case PL_STATE.MOVE:
                if (!Input.anyKey) //만약 아무 키도 입력이 들어오지 않는다면
                {
                    plState = PL_STATE.IDLE; //IDLE 상태로 변경한다.
                }
                break;

            case PL_STATE.PUSH:
                plInfo.plMoveSpd = WalkMoveSpd(); //이동속도를 반으로 변경
                if (!Input.anyKey)
                {
                    plInfo.plMoveSpd = moveSpd; //원래 속도로 변경
                    plState = PL_STATE.IDLE;
                }
                break;

            default:
                break;
        }
    }

    public float WalkMoveSpd()
    {
        return moveSpd / 2;
    }
}
