using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("���� �� �̵� ����")]
    public float rotSpd = 10f;
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0;
    public const float moveSpd = 10f; //(����) ������ �ӵ�, ���� �����̴� �ӵ��� plInfo���� Ȯ��

    [Header("�÷���")]
    public bool isActivated = false;
    public bool isAttack = false;
    public bool isNextAtk = false; // (M1, M2, M3)attack ���¿��� true�� �Ǹ� ���� ������� �̵�����
    public bool isAvoid = false;
    public bool isNoDamage = false; //���� ����
    public bool isDead = false;

    [Header("ȸ�� �ִϸ��̼� ����")]
    public float avoidTime = 0; //deltaTime ���� ����
    public float avoidJAnimTime = 1f; //���� ȸ��-���� �ִϸ��̼� �ð� ����
    public float avoidRAnimTime = 2f; //���� ȸ��-������ �ִϸ��̼� �ð� ����

    [Header("���� �ִϸ��̼� ����")]
    public float attackTime = 0; //deltaTime ���� ����
    public float atkResetTime = 2f; //���� �ʱ�ȭ �ð� 2��

    [Header("���� ����")]
    public float originAtk;
    public const float damageRange = 0.3f; //0~1 ������ ��

    [Header("ȸ�� ����")]
    public const float healHp = 3; //�ʴ� ȸ���ϴ� ��ȥ�� ���� ��

    public Timer timer;
    public PlayerInfo plInfo;
    public DamageCalc damangeCalc;
    public ActionFuntion actionFuntion;
    public Transform cameraTransform;
    public CharacterController characterController;

    public enum PL_STATE
    {
        IDLE,
        MOVE, //�޸���
        WALK, //��� ����
        ACT, //��ȣ�ۿ�
        JUMP, //����
        ATTACKM1, //���ݸ��1
        ATTACKM2, //���ݸ��2
        ATTACKM3, //���ݸ��3
        DAMAGED, //�ǰ�
        AVOIDJUMP, //ȸ��-����
        AVOIDROLL, //ȸ��-������
        DIE //���
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

        if (Input.GetKey(KeyCode.Alpha1)) //1��Ű�� ������
        {
            Heal(); //ȸ���Ѵ�.
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
                //�̵��ӵ��� ������ ���δ�.

                if (IsAttacking())
                {
                    plState = PL_STATE.ATTACKM1;

                    originAtk = plInfo.plAtk; //���� ���ݷ� �ӽ�����
                    plInfo.plAtk *= 1.5f; //���ݷ� 1.5�� ���� (���ݷ� ����)
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
                plInfo.plMoveSpd = 0; //������ ������ �������� ���ϰ� �Ѵ�.

                //���� �� ����� ���

                //�ִϸ��̼� ���� �ڵ�

                //��Ÿ �ʱ�ȭ �ð�

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
                        plInfo.plAtk = originAtk; //������ ������� ����
                        plInfo.plMoveSpd = moveSpd; //���� ����, ���� �ӵ��� ����
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }
                    
                }

                break;

            case PL_STATE.ATTACKM2:
                plInfo.plAtk = originAtk;
                //���ݷ� ����

                //���� �� ����� ���

                //�ִϸ��̼� ����

                //��Ÿ �ʱ�ȭ �ð�
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
                        plInfo.plMoveSpd = moveSpd; //���� ����, ���� �ӵ��� ����
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }


                break;

            case PL_STATE.ATTACKM3:
                plInfo.plAtk = originAtk;
                //���ݷ� ����

                //enemy.currentHp -= damangeCalc.DamageRandomCalc(plInfo.plAtk, damageRange);
                //���� �� ����� ���

                //�ִϸ��̼� ����

                //��Ÿ �ʱ�ȭ �ð�
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
                        plInfo.plMoveSpd = moveSpd; //���� ����, ���� �ӵ��� ����
                        plState = PL_STATE.IDLE;
                        isNextAtk = false;
                        isAttack = false;
                    }

                }

                break;

            case PL_STATE.DAMAGED:
                if (plInfo.curHp <= 0) //���� PL�� HP(ȥ��) 0���ϸ� DIE
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
                isNoDamage = true; //���� ON

                //�ִϸ��̼� ����

                //�ִϸ��̼� �ð� ���
                avoidTime += Time.deltaTime;
                if (avoidTime > avoidJAnimTime)
                {
                    avoidTime = 0;
                    plState = PL_STATE.AVOIDROLL;
                }
                
                break;

            case PL_STATE.AVOIDROLL:
                isNoDamage = false; //���� OFF

                //�ִϸ��̼� ����

                //�ִϸ��̼� �ð� ���
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
            if (plInfo.soulHp > 0) //��ȥ���� ��� ��ȥ�� ���԰� 0���� ũ��
            {
                if (plInfo.curHp < plInfo.maxHp) //���� �÷��̾��� ȥ��(HP)�� �ִ�ġ���� ������
                {
                    actionFuntion.FillHpUsingStone(healHp);//ȸ��
                }
                else
                {
                    Debug.Log("�ִ� ü�� �̻����� ȸ���� �� �����ϴ�!");
                }
            }
            else
            {
                Debug.Log("��ȥ���� ����ֽ��ϴ�!");
            }
        }
        
    }

    public void BeAttacked(int damage)
    {
        plInfo.curHp -= damage;
        //���� ���� ������� ���� �÷��̾� ���� ȥ��(HP)���� ����
        plState = PL_STATE.DAMAGED;
        //�÷��̾� ���� ����
    }
}
