using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("시점 및 이동 변수")]
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0;
    public const float moveSpd = 10f; //(고정) 움직임 속도, 현재 움직이는 속도는 plInfo에서 확인
    
    [Header("플래그")]
    public bool isAttack = false;
    public bool isNextAtk = false; // (M1, M2, M3)attack 상태에서 true가 되면 다음 모션으로 이동가능
    public bool isAvoid = false;
    public bool isNoDamage = false; //무적 상태
    public bool isDead = false;

    [Header("회피 애니메이션 변수")]
    public float avoidTime = 0; //deltaTime 더할 변수
    public float avoidJAnimTime = 1f; //실제 회피-점프 애니메이션 시간 변수
    public float avoidRAnimTime = 2f; //실제 회피-구르기 애니메이션 시간 변수

    [Header("공격 애니메이션 변수")]
    public float attackTime = 0; //deltaTime 더할 변수
    public float atkResetTime = 2f; //공격 초기화 시간 2초

    public PlayerInfo plInfo;
    public Transform cameraTransform;
    public CharacterController characterController;

    public enum PL_STATE
    {
        IDLE,
        MOVE, //달리기
        WALK, //기습 구보
        ACT, //상호작용
        JUMP, //점프
        ATTACKM1, //공격모션1
        ATTACKM2, //공격모션2
        ATTACKM3, //공격모션3
        DAMAGED, //피격
        AVOIDJUMP, //회피-점프
        AVOIDROLL, //회피-구르기
        DIE //사망
    }
    public PL_STATE plState;

    void Start()
    {
        plInfo = GetComponent<PlayerInfo>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        characterController = GetComponentInChildren<CharacterController>();

        plInfo.plMoveSpd = moveSpd;
    }


    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection *= plInfo.plMoveSpd;

        if (characterController.isGrounded)
        {
            yVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jumpSpeed;
                plState = PL_STATE.JUMP;
            }
        }

        yVelocity += (gravity * Time.deltaTime);
        moveDirection.y = yVelocity;
        characterController.Move(moveDirection * Time.deltaTime);

        if (IsAttacking())
        {
            if (plState == PL_STATE.IDLE)
            {
                plState = PL_STATE.ATTACKM1;
            }
            
        }

        IsAvoiding();

        switch (plState)
        {
            case PL_STATE.IDLE:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    plState = PL_STATE.MOVE;
                }

                break;

            case PL_STATE.MOVE:
                plInfo.plMoveSpd = moveSpd;

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    plState = PL_STATE.WALK;
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    plState = PL_STATE.WALK;
                }
                else if (!Input.anyKey)
                {
                    plState = PL_STATE.IDLE;
                }
                break;

            case PL_STATE.WALK:
                plInfo.plMoveSpd = WalkMoveSpd();

                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    plState = PL_STATE.MOVE;
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    plState = PL_STATE.IDLE;
                }
                break;

            case PL_STATE.ACT:
                break;

            case PL_STATE.JUMP:
                if (characterController.isGrounded)
                {
                    plState = PL_STATE.IDLE;
                }
                break;

            case PL_STATE.ATTACKM1:
                //애니메이션 실행

                //연타 초기화 시간
                attackTime += Time.deltaTime;
                if (IsAttacking())
                {
                    isNextAtk = true;
                }
                if (attackTime > atkResetTime)
                {
                    if (isNextAtk == true)
                    {
                        attackTime = 0;
                        plState = PL_STATE.ATTACKM2;
                        isNextAtk = false;
                    }
                    else
                    {
                        attackTime = 0;
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }
                    
                }

                break;

            case PL_STATE.ATTACKM2:
                //애니메이션 실행

                //연타 초기화 시간
                attackTime += Time.deltaTime;
                if (IsAttacking())
                {
                    isNextAtk = true;
                }
                if (attackTime > atkResetTime)
                {
                    if (isNextAtk == true)
                    {
                        attackTime = 0;
                        plState = PL_STATE.ATTACKM3;
                        isNextAtk = false;
                    }
                    else
                    {
                        attackTime = 0;
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }


                break;

            case PL_STATE.ATTACKM3:
                //애니메이션 실행

                //연타 초기화 시간
                attackTime += Time.deltaTime;
                if (IsAttacking())
                {
                    isNextAtk = true;
                }
                if (attackTime > atkResetTime)
                {
                    if (isNextAtk == true)
                    {
                        attackTime = 0;
                        plState = PL_STATE.ATTACKM1;
                        isNextAtk = false;
                    }
                    else
                    {
                        attackTime = 0;
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }

                break;

            case PL_STATE.DAMAGED:

                //현재 PL의 HP(혼력) 0이하면 DIE
                if (plInfo.curHp <= 0)
                {
                    plInfo.curHp = 0;
                    plState = PL_STATE.DIE;
                }

                break;

            case PL_STATE.AVOIDJUMP:
                isNoDamage = true; //무적 ON

                //애니메이션 실행

                //애니메이션 시간 대기
                avoidTime += Time.deltaTime;
                if (avoidTime > avoidJAnimTime)
                {
                    avoidTime = 0;
                    plState = PL_STATE.AVOIDROLL;
                }
                
                break;

            case PL_STATE.AVOIDROLL:
                isNoDamage = false; //무적 OFF

                //애니메이션 실행

                //애니메이션 시간 대기
                avoidTime += Time.deltaTime;
                if (avoidTime > avoidRAnimTime)
                {
                    avoidTime = 0;
                    plState = PL_STATE.IDLE;
                }
                isAvoid = false;
                break;

            case PL_STATE.DIE:
                Debug.Log("PLAYER DIE");
                break;

            default:
                break;
        }
    }

    public float WalkMoveSpd()
    {
        return moveSpd / 2;
    }

    public void IsAvoiding()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isAvoid = true;
            plState = PL_STATE.AVOIDJUMP;
        }
    }

    public bool IsAttacking()
    {
        if (Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1))
        {
            isAttack = true;
            return true;
        }
        else return false;
        
    }
}
