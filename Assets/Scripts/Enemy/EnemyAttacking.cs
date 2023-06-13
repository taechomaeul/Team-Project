using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    // 적 정보
    EnemyInfo enemyInfo;
    // 현재 공격 진행도
    float attackProgress;

    [Header("공격 판정 범위")]
    [Tooltip("공격 판정 범위")]
    [SerializeField] GameObject attackRange;


    void Start()
    {
        // enemyInfo 초기화
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // 공격 판정 off
        attackRange.SetActive(false);
        // 공격 진행도 초기화
        attackProgress = 0f;
        // 공격 가능 초기화
        enemyInfo.SetCanAttack(true);

    }

    void FixedUpdate()
    {
        // 살아있는 상태라면
        if (!enemyInfo.GetIsDead())
        {
            // 탐지 대상을 인식하고 있는 중이라면
            if (enemyInfo.GetIsTracking())
            {
                // 탐지 대상이 시야각 안에 존재 && 공격 사거리 안에 존재한다면
                if ((Mathf.Acos(Vector3.Dot(transform.forward, (enemyInfo.target.transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= enemyInfo.GetDetectAngle() * 0.5f
                    && Vector3.Distance(transform.position, enemyInfo.target.transform.position) <= enemyInfo.GetAttackRange())
                {
                    // 공격 사거리 진입 -> true
                    enemyInfo.SetIsInAttackRange(true);
                    // 공격 가능이 true라면
                    if (enemyInfo.GetCanAttack())
                    {
                        // 공격 중이 true가 아니라면
                        if (enemyInfo.GetIsAttacking() != true)
                        {
                            // 공격 중 -> true
                            enemyInfo.SetIsAttacking(true);
                            // 공격 가능 -> false
                            enemyInfo.SetCanAttack(false);
                            // 공격 시작(공격 시전 시간은 임시로 임의값 넣음)
                            StartCoroutine(Attack(enemyInfo.GetAttackCycle() * 0.5f));
                            StartCoroutine(AttackTimer());
                        }
                    }
                }
                // 탐지 대상이 공격 사거리 밖이라면
                else
                {
                    // 공격 사거리 진입 -> false
                    enemyInfo.SetIsInAttackRange(false);
                }
            }
        }
        // 죽었다면
        else
        {
            // 공격 멈춤
            StopAllCoroutines();
            // 공격 판정 off
            attackRange.SetActive(false);
        }
    }

    // 공격(공격 시전 시간)
    IEnumerator Attack(float attackTime)
    {
        // 공격 중이 true라면
        if (enemyInfo.GetIsAttacking())
        {
            // 공격 판정 on
            attackRange.SetActive(true);
            // attackTime 만큼 기다림(공격 모션 시작부터 끝까지의 시간)
            yield return new WaitForSeconds(attackTime);
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

    // 공격 가능 계산 타이머
    IEnumerator AttackTimer()
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

    private void OnDrawGizmos()
    {
        // 디버그용 enemyInfo 초기화
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }

        // 디버그 스위치가 켜져있다면
        if (enemyInfo.GetIsDebug())
        {
            // 공격 사거리 기즈모 표시(빨간색)
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, enemyInfo.GetAttackRange());
        }
    }
}
