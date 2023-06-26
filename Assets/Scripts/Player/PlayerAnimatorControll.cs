using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimatorControll : MonoBehaviour
{
    // 모델에 달려있는 애니메이터
    private Animator animator;

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
    [SerializeField] private Animation_State animationState;



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

    /// <summary>
    /// 현재 상태의 애니메이션 재생시간 반환
    /// </summary>
    /// <param name="state">재생시간을 반환받을 애니메이션 상태</param>
    /// <returns>애니메이션 재생 시간</returns>
    internal float GetAnimationDurationTime(Animation_State state)
    {
        // 재생시간
        float time = 0;
        // 상태머신 state를 비교하기 위한 변수
        string smState = GetStringFromAnimationStateMachine(state);
        // 상태머신 state의 애니메이션 clip
        string smClip = "";
        // 상태머신 state의 배속
        float smSpeed = 1;

        // 상태머신 관련 초기화
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
        AnimatorStateMachine sm = ac.layers[0].stateMachine;

        float skillLoopTime = 1;

        // 현재 상태의 애니메이션 찾고 재생시간 반환
        // 상태와 연결된 애니메이션 이름과 상태 재생속도, 상태 반복 횟수 확인
        for (int i = 0; i < sm.states.Length; i++)
        {
            if (sm.states[i].state.name.Equals(smState))
            {
                if (smState.Equals("Skill"))
                {
                    skillLoopTime = sm.states[i].state.transitions[0].exitTime;
                }
                smClip = sm.states[i].state.motion.name;
                smSpeed = sm.states[i].state.speed;
            }
        }

        // 찾은 애니메이션 이름으로 애니메이션 재생 길이 확인
        for (int i = 0; i < rac.animationClips.Length; i++)
        {
            if (rac.animationClips[i].name.Equals(smClip))
            {
                time = rac.animationClips[i].length;
            }
        }

        // 애니메이션 재생 길이, 상태 재생 속도, 반복 횟수를 계산한 최종 상태 재생 길이 반환
        if (smState.Equals("Skill"))
        {
            return (time / smSpeed) * skillLoopTime;
        }
        else
        {
            return time / smSpeed;
        }
    }

    /// <summary>
    /// 애니메이션 상태에 해당하는 상태 머신의 이름을 string으로 반환
    /// </summary>
    /// <param name="state">string을 반환할 애니메이션 상태</param>
    /// <returns></returns>
    string GetStringFromAnimationStateMachine(Animation_State state)
    {
        string smStateString = "";

        // 성능을 위해 하드코딩
        switch (state)
        {
            case Animation_State.Move:
                smStateString = "Move";
                break;
            case Animation_State.Walk:
                
                break;
            case Animation_State.Jump:
                smStateString = "Jump";
                break;
            case Animation_State.Attack1:
                smStateString = "Attack1";
                break;
            case Animation_State.Attack2:
                smStateString = "Attack2";
                break;
            case Animation_State.Attack3:
                smStateString = "Attack3";
                break;
            case Animation_State.Hit:
                smStateString = "Hit";
                break;
            case Animation_State.Avoid1:
                smStateString = "Roll";
                break;
            case Animation_State.Avoid2:
                break;
            case Animation_State.Dead:
                smStateString = "Dead";
                break;
        }
        return smStateString;
    }
}