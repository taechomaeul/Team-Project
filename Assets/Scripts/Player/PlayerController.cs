using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("시점 및 이동 변수")]
    public float rotSpd = 10f;
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0;
    public readonly float moveSpd = 10f; //(고정) 움직임 속도, 현재 움직이는 속도는 plInfo에서 확인

    [Header("플래그")]
    public bool isActivated = false;
    public bool isAttack = false;
    public bool isNextAtk = false; // (M1, M2, M3)attack 상태에서 true가 되면 다음 모션으로 이동가능
    public bool isAvoid = false;
    public bool isNoDamage = false; //무적 상태
    public bool isDead = false;
    public bool isSkillCool = false; //스킬 쿨타임이 돌고 있는지 여부

    [Header("회피 애니메이션 변수")]
    public float avoidTime = 0; //deltaTime 더할 변수
    public float avoidJAnimTime = 1f; //실제 회피-점프 애니메이션 시간 변수
    public float avoidRAnimTime = 2f; //실제 회피-구르기 애니메이션 시간 변수

    [Header("공격 애니메이션 변수")]
    public float attackTime = 0; //deltaTime 더할 변수
    public float atkResetTime; //공격 초기화 시간 변수

    [Header("피격 애니메이션 변수")]
    public float damagedTime = 0; //deltaTime 더할 변수
    public float dmgResetTime; //피격 초기화 시간 변수

    [Header("공격 변수")]
    public int originAtk;
    public readonly float damageRange = 0.3f; //0~1 사이의 값

    [Header("회복 변수")]
    public readonly int healHp = 3; //초당 회복하는 영혼의 무게 수

    public Timer timer;
    public SkillInfo skillData;
    public PlayerInfo plInfo;
    public ActionFuntion actionFuntion;
    public Transform cameraTransform;
    public CharacterController characterController;

    private PlayerAnimatorControll pac;
    private SaveManager saveManager;

    private bool coroutineCheck;
    private bool waitTimeCheck;
    IEnumerator gcadt;

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

    public GameObject attackRange;

    void Start()
    {
        plInfo = GetComponent<PlayerInfo>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        actionFuntion = GameObject.Find("ActionFunction").GetComponent<ActionFuntion>();
        skillData = GameObject.Find("ActionFunction").GetComponent<SkillInfo>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        characterController = GetComponentInChildren<CharacterController>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        //anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        pac = GetComponent<PlayerAnimatorControll>();

        
        plInfo.curPositionIndex = saveManager.saveClass.GetLastSavePosition();

        transform.GetChild(0).GetComponent<CharacterController>().enabled = false;
        transform.rotation = GameObject.Find("Indexes").transform.GetChild(plInfo.curPositionIndex).rotation;
        transform.GetChild(0).localPosition = GameObject.Find("Indexes").transform.GetChild(plInfo.curPositionIndex).position;
        transform.GetChild(0).GetComponent<CharacterController>().enabled = true;

        Debug.Log($"Index{plInfo.curPositionIndex} : " + GameObject.Find("Indexes").transform.GetChild(plInfo.curPositionIndex).position);
        Debug.Log($"transform.GetChild(0).position : {transform.GetChild(0).position}");

        plInfo.plMoveSpd = moveSpd;
        originAtk = plInfo.plAtk;

        coroutineCheck = false;
        waitTimeCheck = false;
    }

    //void OnDrawGizmosSelected()
    //{
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(GameObject.Find("Sword").transform.position, 1f);
    //}



    void FixedUpdate()
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

        if (!isActivated) //스크립트 활성화 중에는 힐, 스킬, 회피, 점프 등 상태 변경 불가능
        {

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
            IsSkill();
            IsAvoiding();

            switch (plState)
            {
                case PL_STATE.IDLE:
                    //애니메이션 연결
                    //anim.SetInteger("State", 0);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Idle);

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
                    //애니메이션 연결
                    //anim.SetInteger("State", 1);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Move);

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
                    //애니메이션 연결
                    //anim.SetInteger("State", 2);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Move);

                    plInfo.plMoveSpd = WalkMoveSpd();
                    //이동속도를 반으로 줄인다.

                    if (IsAttacking())
                    {
                        originAtk = plInfo.plAtk; //원래 공격력 임시저장
                        plInfo.plAtk = (int)(plInfo.plAtk * 1.5f); //공격력 1.5배 증가 (공격력 설정)
                        plState = PL_STATE.ATTACKM1;
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
                    isActivated = true;
                    break;

                case PL_STATE.JUMP:
                    //애니메이션 연결
                    //anim.SetInteger("State", 4);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Jump);


                    if (characterController.isGrounded)
                    {
                        plState = PL_STATE.IDLE;
                    }
                    break;


                case PL_STATE.ATTACKM1:
                    //애니메이션 연결
                    //anim.SetInteger("State", 5);
                    plInfo.plMoveSpd = 0; //공격할 때에는 움직이지 못하게 한다.

                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Attack1);
                    //atkResetTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack1);

                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack1);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        atkResetTime = (float)temp;


                        attackRange.SetActive(true);

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
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plState = PL_STATE.ATTACKM2;
                                isNextAtk = false;
                            }
                            else
                            {
                                attackTime = 0;
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plInfo.plAtk = originAtk; //원래의 대미지로 변경
                                plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                                plState = PL_STATE.IDLE;
                                isNextAtk = false;
                                isAttack = false;
                            }
                            attackRange.SetActive(false);
                        }
                    }


                    break;

                case PL_STATE.ATTACKM2:
                    //애니메이션 연결
                    //anim.SetInteger("State", 6);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Attack2);
                    //atkResetTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack2);

                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack2);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        atkResetTime = (float)temp;


                        plInfo.plAtk = originAtk; //공격력 설정
                        attackRange.SetActive(true);

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
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plState = PL_STATE.ATTACKM3;
                                isNextAtk = false;
                            }
                            else
                            {
                                attackTime = 0;
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                                plState = PL_STATE.IDLE;
                                isNextAtk = false;
                                isAttack = false;
                            }
                            attackRange.SetActive(false);
                        }
                    }



                    break;

                case PL_STATE.ATTACKM3:
                    //애니메이션 연결
                    //anim.SetInteger("State", 7);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Attack3);
                    //atkResetTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack3);
                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Attack3);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        atkResetTime = (float)temp;


                        plInfo.plAtk = originAtk;
                        //공격력 설정

                        attackRange.SetActive(true);

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
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plState = PL_STATE.ATTACKM1;
                                isNextAtk = false;
                            }
                            else
                            {
                                attackTime = 0;
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plInfo.plMoveSpd = moveSpd; //공격 종료, 원래 속도로 변경
                                plState = PL_STATE.IDLE;
                                isNextAtk = false;
                                isAttack = false;
                            }
                            attackRange.SetActive(false);
                        }
                    }


                    break;

                case PL_STATE.DAMAGED:
                    //애니메이션 연결
                    //anim.SetInteger("State", 8);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Hit);
                    //dmgResetTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Hit); //피격 애니메이션 길이
                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Hit);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        dmgResetTime = (float)temp;


                        //애니메이션 시간 대기
                        damagedTime += Time.deltaTime;
                        if (damagedTime > dmgResetTime)
                        {
                            damagedTime = 0;
                            if (plInfo.curHp <= 0) //현재 PL의 HP(혼력) 0이하면 DIE
                            {
                                plInfo.curHp = 0;
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plState = PL_STATE.DIE;
                            }
                            else
                            {
                                coroutineCheck = false;
                                waitTimeCheck = false;
                                plState = PL_STATE.IDLE;
                            }
                        }

                    }


                    break;

                case PL_STATE.AVOIDJUMP:
                    //애니메이션 연결
                    //anim.SetInteger("State", 9);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Avoid1);
                    //avoidJAnimTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Avoid1);
                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Avoid1);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        avoidJAnimTime = (float)temp;


                        isNoDamage = true; //무적 ON

                    //애니메이션 시간 대기
                    avoidTime += Time.deltaTime;
                    if (avoidTime > avoidJAnimTime)
                    {
                        avoidTime = 0;
                            coroutineCheck = false;
                            waitTimeCheck = false;
                            plState = PL_STATE.AVOIDROLL;
                    }
                    }


                    break;

                case PL_STATE.AVOIDROLL:
                    isNoDamage = false; //무적 OFF

                    //애니메이션 연결
                    //anim.SetInteger("State", 10);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Avoid2);
                    //avoidRAnimTime = pac.GetAnimationDurationTime(PlayerAnimatorControll.Animation_State.Avoid2);
                    if (!coroutineCheck)
                    {
                        StopCoroutine(gcadt);
                        gcadt = pac.GetCurrentAnimationDurationTime(PlayerAnimatorControll.Animation_State.Avoid2);
                        StartCoroutine(gcadt);
                        coroutineCheck = true;
                    }

                    if (waitTimeCheck)
                    {
                        float? temp = gcadt.Current as float?;
                        avoidRAnimTime = (float)temp;


                        //애니메이션 시간 대기
                        avoidTime += Time.deltaTime;
                    if (avoidTime > avoidRAnimTime)
                    {
                        avoidTime = 0;
                            coroutineCheck = false;
                            waitTimeCheck = false;
                            plState = PL_STATE.IDLE;
                    }
                    }

                    isAvoid = false;
                    break;

                case PL_STATE.DIE:
                    //애니메이션 연결
                    //anim.SetInteger("State", 11);
                    pac.SetAnimationState(PlayerAnimatorControll.Animation_State.Dead);

                    //플래그 ON, 속도 X
                    isDead = true;
                    plInfo.plMoveSpd = 0;
                    Debug.Log("PLAYER DIE");
                    break;

                default:
                    break;
            }
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

    /// <summary>
    /// 피격 당했을 경우, 플레이어의 상태를 바꾸고 대미지를 차감하는 함수
    /// </summary>
    /// <param name="damage">적이 가한 대미지 양</param>
    public void BeAttacked(int damage)
    {
        plInfo.curHp -= damage;
        //적이 가한 대미지의 양을 플레이어 현재 혼력(HP)에서 차감
        plState = PL_STATE.DAMAGED;
        //플레이어 상태 변경
    }

    /// <summary>
    /// 스킬 쿨타임에 따른 스킬 사용 가능 여부 확인 후, 스킬을 실행하는 함수
    /// E키를 눌렀을 때 SkillCool이 초기화(false)가 되었다면 스킬 사용 가능
    /// </summary>
    public void IsSkill()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSkillCool) //E키 입력이 들어왔는데 CoolTime이 없다면
        {
            isSkillCool = true;
            switch (plInfo.curSkill.skillName)
            {
                case "힘증가":
                    int originAtk = plInfo.plAtk;
                    actionFuntion.IncreasePower();
                    StartCoroutine(Reset(plInfo.curSkill.duringTime, plInfo.curSkill.coolTime, originAtk, 1));
                    break;

                case "민첩증가":
                    int originMoveSpd = plInfo.plAtk;
                    actionFuntion.IncreaseSpeed();
                    StartCoroutine(Reset(plInfo.curSkill.duringTime, plInfo.curSkill.coolTime, originMoveSpd, 2));
                    break;

                case "체력회복":
                    actionFuntion.IncreaseHp();
                    StartCoroutine(ResetCoolTime(plInfo.curSkill.coolTime));
                    break;
            }
        }
    }

    /// <summary>
    /// 지속시간 + 쿨타임 계산용 함수.
    /// </summary>
    /// <param name="duringTime">스킬 지속시간</param>
    /// <param name="coolTime">스킬 쿨타임</param>
    /// <param name="originAmount">변수 변경되기 전의 값</param>
    /// <param name="select">공격력, 이동속도 선택용 변수</param>
    /// <returns></returns>
    public IEnumerator Reset(float duringTime, float coolTime, int originAmount, int select)
    {
        StartCoroutine(ResetCoolTime(coolTime)); //쿨타임 계산 함수
        yield return new WaitForSeconds(duringTime);

        if (select == 1) { plInfo.plAtk = originAmount; }
        else if (select == 2) { plInfo.plMoveSpd = originAmount; }
        //원래 값으로 변경
    }

    /// <summary>
    /// 쿨타임 계산용 함수.
    /// </summary>
    /// <param name="coolTime">스킬 쿨타임</param>
    /// <returns></returns>
    public IEnumerator ResetCoolTime(float coolTime)
    {
        //Debug.Log("CoolTIme Reset-ing");
        yield return new WaitForSeconds(coolTime);
        isSkillCool = false; //스킬쿨 해제
        //Debug.Log("CoolTIme Reset Complete!");
    }

    public void InitAnimator()
    {
        pac.InitAnimator();
    }


    public void WaitTimeCheckChange(bool tf)
    {
        waitTimeCheck = tf;
    }
}
