using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationControll : MonoBehaviour
{
    // 애니메이션 목록
    [SerializeField] internal enum Animation_State
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Hit,
        Dead
    }
    [Header("애니메이션")]
    [Tooltip("애니메이션 상태")]
    [SerializeField] Animation_State animationState;

    [Header("애니메이션 플래그")]
    [Tooltip("이동")]
    [SerializeField] bool isMoving;
    [Tooltip("공격1")]
    [SerializeField] bool isAttacking1;
    [Tooltip("공격2")]
    [SerializeField] bool isAttacking2;
    [Tooltip("피격")]
    [SerializeField] bool isHit;
    [Tooltip("사망")]
    [SerializeField] bool isDead;

    // 모델에 달려있는 애니메이터
    Animator animator;

    private void Awake()
    {
        // 첫번째 자식(모델)의 Animator 컴포넌트로 초기화
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        switch (animationState)
        {
            case Animation_State.Idle:
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Move:
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Attack1:
                animator.SetBool("isAttacking1", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Attack2:
                animator.SetBool("isAttacking2", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Hit:
                animator.SetBool("isHit", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Dead:
                animator.SetBool("isDead", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isHit", false);
                break;
        }
    }

    internal void SetAnimationState(Animation_State state)
    {
        animationState = state;
    }
}
