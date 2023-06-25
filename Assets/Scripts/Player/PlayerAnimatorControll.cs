using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorControll : MonoBehaviour
{
    // 모델에 달려있는 애니메이터
    Animator animator;

    // 애니메이션 목록
    internal enum Animation_State
    {
        Idle,
        Move,
        Walk,
        Jump,
        Attack1,
        Attack2,
        Attack3,
        Hit,
        Avoid1,
        Avoid2,
        Dead
    }

    // 인스펙터
    [Header("애니메이션")]
    [Tooltip("애니메이션 상태")]
    [SerializeField,ReadOnly] Animation_State animationState;

    private void Awake()
    {
        // 플레이어 모델에 애니메이터가 존재한다면
        if (transform.GetChild(0).GetChild(0).GetComponent<Animator>()!=null)
        {
            // animator 초기화
            animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        // 상태별 애니메이터 컨트롤
        switch (animationState)
        {
            case Animation_State.Idle:
                SetAnimatorParam();
                break;
            case Animation_State.Move:
                SetAnimatorParam("isMoving");
                break;
            case Animation_State.Walk:
                // 걷기
                break;
            case Animation_State.Jump:
                SetAnimatorParam("isJumping");
                break;
            case Animation_State.Attack1:
                SetAnimatorParam("isAttacking1");
                break;
            case Animation_State.Attack2:
                SetAnimatorParam("isAttacking2");
                break;
            case Animation_State.Attack3:
                SetAnimatorParam("isAttacking3");
                break;
            case Animation_State.Hit:
                SetAnimatorParam("isHit");
                break;
            case Animation_State.Avoid1:
                SetAnimatorParam("isRolling");
                break;
            case Animation_State.Avoid2:
                // 회피2
                break;
            case Animation_State.Dead:
                SetAnimatorParam("isDead");
                break;
        }
    }

    /// <summary>
    /// 애니메이터의 모든 파라미터 false
    /// </summary>
    void SetAnimatorParam()
    {
                animator.SetBool("isMoving", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isRolling", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false );
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
    }

    /// <summary>
    /// 애니메이터에서 함수의 매개변수로 들어오는 파라미터만 true
    /// </summary>
    /// <param name="anim">true로 만들 애니메이터 파라미터</param>
    void SetAnimatorParam(string anim)
    {
        SetAnimatorParam();
        animator.SetBool(anim, true);
    }

    /// <summary>
    /// 애니메이션 재생
    /// </summary>
    /// <param name="state">재생될 애니메이션 상태</param>
    internal void SetAnimationState(Animation_State state)
    {
        animationState = state;
    }
}