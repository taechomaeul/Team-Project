using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    // 적 정보
    Enemy enemyInfo;
    Boss bossInfo;

    // 현재 공격 진행도
    float attackProgress;

    // 스킬 남은 재사용 대기시간
    float currentSkillCoolDown;

    // 보스인지 확인
    bool isBoss;

    // 애니메이터 컨트롤
    EnemyAnimationControll eac;
    // 이펙트 컨트롤
    EnemyEffectAndSoundControll eec;

    // 인스펙터
    [Header("공격 종류 개수")]
    [Tooltip("공격 종류 개수")]
    [Range(1, 3)][SerializeField] int numberOfAttackType;

    [Header("공격 판정 범위")]
    [Tooltip("평타 판정 범위")]
    [SerializeField] GameObject attackRange;

    [Tooltip("스킬 판정 범위")]
    [SerializeField] GameObject skillRange;



    void Start()
    {
        // 적 정보 초기화
        if (enemyInfo == null)
        {
            // 일반 몬스터라면
            if (GetComponent<BossInfo>() == null)
            {
                enemyInfo = GetComponent<EnemyInfo>().stat;
                isBoss = false;
            }
            // 보스라면
            else
            {
                enemyInfo = GetComponent<BossInfo>().stat;
                bossInfo = enemyInfo as Boss;
                isBoss = true;
                // 스킬 사용 가능 초기화
                bossInfo.SetCanSkill(true);
                // 스킬 판정 off
                skillRange.SetActive(false);
            }
        }

        // 공격 판정 off
        attackRange.SetActive(false);
        // 공격 진행도 초기화
        attackProgress = 0f;
        // 스킬 재사용 대기시간 초기화
        currentSkillCoolDown = 0f;
        // 공격 가능 초기화
        enemyInfo.SetCanAttack(true);

        // 애니메이터 컨트롤 세팅
        eac = GetComponent<EnemyAnimationControll>();
        // 이펙트 컨트롤 세팅
        eec = GetComponent<EnemyEffectAndSoundControll>();
    }

    void FixedUpdate()
    {
        // 살아있는 상태라면
        if (!enemyInfo.GetIsDead())
        {
            // 탐지 대상을 인식하고 있는 중이라면
            if (enemyInfo.GetIsTracking())
            {
                // 탐지 대상이 공격 사거리 안에 존재한다면
                if (enemyInfo.GetDistanceFromTarget() <= enemyInfo.GetAttackRange())
                {
                    // 공격 사거리 진입 -> true
                    enemyInfo.SetIsInAttackRange(true);
                }
                // 탐지 대상이 공격 사거리 밖이라면
                else
                {
                    // 공격 사거리 진입 -> false
                    enemyInfo.SetIsInAttackRange(false);
                }

                // 보스라면
                if (isBoss)
                {
                    // 탐지대상이 스킬 사거리 내에 있다면 
                    if (enemyInfo.GetDistanceFromTarget() <= bossInfo.GetSkillCastRange())
                    {
                        // 스킬 사거리 진입 -> true
                        bossInfo.SetIsInSkillRange(true);
                    }
                    // 탐지대상이 스킬 사거리 내에 없다면
                    else
                    {
                        // 스킬 사거리 진입 -> false
                        bossInfo.SetIsInSkillRange(false);
                    }
                }

                // 공격 가능이 true라면
                if (enemyInfo.GetCanAttack())
                {
                    // 공격, 피격 중이 아니라면
                    if (!enemyInfo.GetIsAttacking() && !enemyInfo.GetIsAttacked())
                    {
                        // 보스라면
                        if (isBoss)
                        {
                            // 스킬 사용 페이즈이고 스킬 사용이 가능하다면
                            if (bossInfo.GetCurrentHp() <= bossInfo.GetMaxHp() * bossInfo.GetSkillPhaseHpRatio() && bossInfo.GetCanSkill())
                            {
                                // 탐지대상이 스킬 사거리 내에 있다면 
                                if (bossInfo.GetIsInSkillRange())
                                {
                                    // 공격 중 -> true
                                    enemyInfo.SetIsAttacking(true);
                                    // 공격 가능 -> false
                                    enemyInfo.SetCanAttack(false);

                                    // 스킬 발동
                                    // 스킬 발동 가능 -> false
                                    bossInfo.SetCanSkill(false);
                                    // 스킬 시전 중 -> true
                                    bossInfo.SetIsSkillCasting(true);

                                    // 스킬 시작
                                    StartCoroutine(CastSkill());
                                    // 스킬 재사용 대기시간 타이머 시작
                                    StartCoroutine(SkillTimer());
                                    // 공격 가능 계산 타이머 시작
                                    StartCoroutine(AttackTimer());
                                }
                            }
                            // 스킬 사용 불가능이라면
                            else
                            {
                                // 탐지 대상이 공격 사거리 안에 존재한다면
                                if (enemyInfo.GetIsInAttackRange())
                                {
                                    // 탐지 대상이 시야각 안에 존재한다면
                                    if ((Mathf.Acos(Vector3.Dot(transform.forward, (enemyInfo.GetCurrentTarget().transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= enemyInfo.GetDetectAngle() * 0.5f)
                                    {
                                        // 공격 중 -> true
                                        enemyInfo.SetIsAttacking(true);
                                        // 공격 가능 -> false
                                        enemyInfo.SetCanAttack(false);

                                        // 공격 시작
                                        StartCoroutine(Attack());
                                        // 공격 가능 계산 타이머 시작
                                        StartCoroutine(AttackTimer());
                                    }
                                }
                            }
                        }
                        // 일반 몬스터라면
                        else
                        {
                            // 탐지 대상이 공격 사거리 안에 존재한다면
                            if (enemyInfo.GetIsInAttackRange())
                            {
                                // 탐지 대상이 시야각 안에 존재한다면
                                if ((Mathf.Acos(Vector3.Dot(transform.forward, (enemyInfo.GetCurrentTarget().transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= enemyInfo.GetDetectAngle() * 0.5f)
                                {
                                    // 공격, 피격 중이 아니라면
                                    if (!enemyInfo.GetIsAttacking() || !enemyInfo.GetIsAttacked())
                                    {
                                        // 공격 중 -> true
                                        enemyInfo.SetIsAttacking(true);
                                        // 공격 가능 -> false
                                        enemyInfo.SetCanAttack(false);

                                        // 공격 시작
                                        StartCoroutine(Attack());
                                        // 공격 가능 계산 타이머 시작
                                        StartCoroutine(AttackTimer());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        // 죽었다면
        else
        {
            // 공격, 스킬, 타이머 멈춤
            StopAllCoroutines();

            // 공격 판정 off
            attackRange.SetActive(false);

            // 보스라면
            if (isBoss)
            {
                // 스킬 판정 off
                skillRange.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 공격
    /// </summary>
    IEnumerator Attack()
    {
        // 공격 중이 true라면
        if (enemyInfo.GetIsAttacking())
        {
            // 공격 타입 랜덤 설정
            int attackType = Random.Range(0, numberOfAttackType)+2;

            // 공격 판정 on
            attackRange.SetActive(true);

            // 공격 모션 시작
            eac.SetAnimationState((EnemyAnimationControll.Animation_State)attackType);

            // 공격 이펙트 시작
            eec.TrunOnEffectAttack(attackType);

            //StartCoroutine(eac.GetCurrentAnimationDurationTime((EnemyAnimationControll.Animation_State)attackType));


            // attackTime 만큼 기다림(공격 모션 시작부터 끝까지의 시간)
            yield return new WaitForSeconds(eac.GetAnimationDurationTime((EnemyAnimationControll.Animation_State)attackType));

            // 공격 모션 끝
            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Idle);

            // 공격 판정 off
            attackRange.SetActive(false);
            // 공격 중 -> false
            enemyInfo.SetIsAttacking(false);
        }
        // 공격 중이 false라면
        else
        {
            // 공격 코루틴 종료
            yield return null;
        }
    }

    /// <summary>
    /// 스킬 시전
    /// </summary>
    IEnumerator CastSkill()
    {
        // 스킬 시전 중이 true라면
        if (bossInfo.GetIsSkillCasting())
        {
            // 스킬 모션 시작
            eac.SetAnimationState(EnemyAnimationControll.Animation_State.CastSkill);

            // 스킬 준비 시간 기다림
            yield return new WaitForSeconds(eac.GetAnimationDurationTime(EnemyAnimationControll.Animation_State.CastSkill));

            // 스킬 판정 on
            skillRange.SetActive(true);

            // 스킬 이펙트 시작
            eec.TrunOnEffectSkill();

            // skillTime 만큼 기다림(스킬 모션 시작부터 끝까지의 시간)
            yield return new WaitForSeconds(eac.GetAnimationDurationTime(EnemyAnimationControll.Animation_State.Skill));

            // 스킬 모션 끝
            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Idle);
            // 스킬 판정 off
            skillRange.SetActive(false);
            // 공격 중 -> false
            enemyInfo.SetIsAttacking(false);
            // 스킬 시전 중 -> false
            bossInfo.SetIsSkillCasting(false);
        }
        // 공격 중이 false라면
        else
        {
            // 공격 코루틴 종료
            yield return null;
        }
    }

    /// <summary>
    /// 공격 가능 계산 타이머
    /// </summary>
    IEnumerator AttackTimer()
    {
        // 공격 가능한 상태가 아니라면
        if (!enemyInfo.GetCanAttack())
        {
            while (true)
            {
                // 공격 진행도가 공격 주기를 넘으면
                if (attackProgress >= enemyInfo.GetAttackCycle())
                {
                    // 공격진행도 초기화
                    attackProgress = 0f;
                    // 공격 가능 -> true
                    enemyInfo.SetCanAttack(true);
                    // 타이머 코루틴 종료
                    break;
                }
                // 매 프레임 공격 진행도에 경과 시간 추가
                attackProgress += Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// 스킬 재사용 대기시간 계산 타이머
    /// </summary>
    IEnumerator SkillTimer()
    {
        while (true)
        {
            // 재사용 대기시간이 지나면
            if (currentSkillCoolDown >= bossInfo.GetSkillCoolDown())
            {
                // 재사용 대기시간 초기화
                currentSkillCoolDown = 0f;
                // 스킬 사용 가능 -> true
                bossInfo.SetCanSkill(true);
                //타이머 코루틴 종료
                break;
            }
            // 매 프레임 경과 시간 추가
            currentSkillCoolDown += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        // 디버그용 적 정보 초기화
        if (enemyInfo == null)
        {
            // 일반 몬스터라면
            if (GetComponent<BossInfo>() == null)
            {
                enemyInfo = GetComponent<EnemyInfo>().stat;
            }
            // 보스라면
            else
            {
                enemyInfo = GetComponent<BossInfo>().stat;
            }
        }

        // 디버그 스위치가 켜져있다면
        if (enemyInfo.GetIsDebug())
        {
            //유니티 에디터에서만
#if UNITY_EDITOR
            // 공격 사거리 기즈모 표시(빨간색)
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, enemyInfo.GetAttackRange());

            // 보스라면
            if (isBoss)
            {
                // 스킬 시전 사거리 기즈모 표시(하늘색)
                Handles.color = Color.cyan;
                Handles.DrawWireDisc(transform.position, transform.up, bossInfo.GetSkillCastRange());
            }
#endif
        }
    }
}