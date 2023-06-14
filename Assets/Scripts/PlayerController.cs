using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("시점 및 이동 변수")]
    public float rotSpd = 10f;
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0;
    public const float moveSpd = 10f; //(고정) 움직임 속도, 현재 움직이는 속도는 plInfo에서 확인

    [Header("플래그")]
    public bool isActivated = false;
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

    [Header("공격 변수")]
    public float originAtk;
    public const float damageRange = 0.3f; //0~1 사이의 값

    [Header("회복 변수")]
    public const float healHp = 3; //초당 회복하는 영혼의 무게 수

    public Timer timer;
    public PlayerInfo plInfo;
    public DamageCalc damangeCalc;
    public ActionFuntion actionFuntion;
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
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        damangeCalc = GetComponent<DamageCalc>();
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        characterController = GetComponentInChildren<CharacterController>();

        plInfo.plMoveSpd = moveSpd;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(GameObject.Find("Sword").transform.position, 1f);
    }


    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection *= plInfo.plMoveSpd;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotSpd);
        }


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

        if (Input.GetKey(KeyCode.Alpha1)) //1번키가 들어오면
        {
            Heal(); //회복한다.
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

                if (IsAttacking())
                {
                    plState = PL_STATE.ATTACKM1;
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
                else if (IsAttacking())
                {
                    plState = PL_STATE.ATTACKM1;
                }
                else if (!Input.anyKey)
                {
                    plState = PL_STATE.IDLE;
                }
                break;

            case PL_STATE.WALK:
                plInfo.plMoveSpd = WalkMoveSpd();
                //이동속도를 반으로 줄인다.

                if (IsAttacking())
                {
                    plState = PL_STATE.ATTACKM1;

                    originAtk = plInfo.plAtk; //원래 공격력 임시저장
                    plInfo.plAtk *= 1.5f; //공격력 1.5배 증가 (공격력 설정)
                }

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
                plInfo.plMoveSpd = 0; //공격할 때에는 움직이지 못하게 한다.

                //실제 들어갈 대미지 계산

                //애니메이션 실행 코드

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
                        plInfo.plAtk = originAtk; //원래의 대미지로 변경
                        plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }
                    
                }

                break;

            case PL_STATE.ATTACKM2:
                plInfo.plAtk = originAtk;
                //공격력 설정

                //실제 들어갈 대미지 계산

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
                        plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }


                break;

            case PL_STATE.ATTACKM3:
                plInfo.plAtk = originAtk;
                //공격력 설정

                //enemy.currentHp -= damangeCalc.DamageRandomCalc(plInfo.plAtk, damageRange);
                //실제 들어갈 대미지 계산

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
                        plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }

                break;

            case PL_STATE.DAMAGED:
                if (plInfo.curHp <= 0) //현재 PL의 HP(혼력) 0이하면 DIE
                {
                    plInfo.curHp = 0;
                    plState = PL_STATE.DIE;
                }
                else
                {
                    plState = PL_STATE.IDLE;
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

    public void Heal()
    {
        if (timer.CountSeconds(1f))
        {
            if (plInfo.soulHp > 0) //영혼석에 담긴 영혼의 무게가 0보다 크고
            {
                if (plInfo.curHp < plInfo.maxHp) //현재 플레이어의 혼력(HP)가 최대치보다 작으면
                {
                    actionFuntion.FillHpUsingStone(healHp);//회복
                }
                else
                {
                    Debug.Log("최대 체력 이상으로 회복할 수 없습니다!");
                }
            }
            else
            {
                Debug.Log("영혼석이 비어있습니다!");
            }
        }
        
    }

    public void BeAttacked(int damage)
    {
        plInfo.curHp -= damage;
        //적이 가한 대미지의 양을 플레이어 현재 혼력(HP)에서 차감
        plState = PL_STATE.DAMAGED;
        //플레이어 상태 변경
    }
}
