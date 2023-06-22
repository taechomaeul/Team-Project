using UnityEditor.Animations;
using UnityEngine;

public class EnemyAnimationControll : MonoBehaviour
{
    // 애니메이션 목록
    [SerializeField]
    internal enum Animation_State
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Attack3,
        Skill,
        Hit,
        Dead
    }
    [Header("애니메이션")]
    [Tooltip("애니메이션 상태")]
    [SerializeField, ReadOnly] Animation_State animationState;

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
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Move:
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Attack1:
                animator.SetBool("isAttacking1", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Attack2:
                animator.SetBool("isAttacking2", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Attack3:
                animator.SetBool("isAttacking3", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Skill:
                animator.SetBool("isSkillCasting", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Hit:
                animator.SetBool("isHit", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isDead", false);
                break;
            case Animation_State.Dead:
                animator.SetBool("isDead", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking3", false);
                animator.SetBool("isSkillCasting", false);
                animator.SetBool("isHit", false);
                break;
        }
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
    /// <returns></returns>
    internal float GetAnimationDurationTime(Animation_State state)
    {
        // 재생시간
        float time = 0;
        // 상태머신 state를 비교하기 위한 변수
        string smState = "";
        // 상태머신 state의 애니메이션 clip
        string smClip = "";
        // 상태머신 state의 배속
        float smSpeed = 1;

        // 현재 상태
        switch (state)
        {
            case Animation_State.Idle:
                smState = "Idle";
                break;
            case Animation_State.Move:
                smState = "Move";
                break;
            case Animation_State.Attack1:
                smState = "Attack1";
                break;
            case Animation_State.Attack2:
                smState = "Attack2";
                break;
            case Animation_State.Attack3:
                smState = "Attack3";
                break;
            case Animation_State.Skill:
                smState = "CastSkill";
                break;
            case Animation_State.Hit:
                smState = "Hit";
                break;
            case Animation_State.Dead:
                smState = "Dead";
                break;
        }
        // 상태머신 관련 초기화
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
        AnimatorStateMachine sm = ac.layers[0].stateMachine;

        string skillClip = "";
        float skillSpeed = 1;
        float skillTime = 0;

        //현재 상태의 애니메이션 찾고 재생시간 반환
        for (int i = 0; i < sm.states.Length; i++)
        {
            if (smState.Equals("CastSkill") && sm.states[i].state.name.Equals("Skill"))
            {
                skillClip = sm.states[i].state.motion.name;
                skillSpeed = sm.states[i].state.speed;
            }
            if (sm.states[i].state.name.Equals(smState))
            {
                smClip = sm.states[i].state.motion.name;
                smSpeed = sm.states[i].state.speed;

            }
        }

        for (int i = 0; i < rac.animationClips.Length; i++)
        {
            if (rac.animationClips[i].name.Equals(smClip))
            {
                time = rac.animationClips[i].length;
            }
            else if (rac.animationClips[i].name.Equals(skillClip))
            {
                skillTime = rac.animationClips[i].length;
            }
        }

        if (smState.Equals("CastSkill"))
        {
            return (time / smSpeed) + (skillTime / skillSpeed);
        }
        else
        {
            return time / smSpeed;
        }
    }
}
