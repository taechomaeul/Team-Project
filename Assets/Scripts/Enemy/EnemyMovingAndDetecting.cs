using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovingAndDetecting : MonoBehaviour
{
    // 적 정보
    Enemy enemyInfo;
    // 원래 위치
    Vector3 originPosition;
    Quaternion originRotation;

    // NavMeshAgent
    NavMeshAgent navMeshAgent;

    // 애니메이터 컨트롤
    EnemyAnimationControll eac;


    void Start()
    {
        // enemyInfo 초기화
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
        // originPosition, originRotation 초기화
        originPosition = transform.position;
        originRotation = transform.rotation;

        // NavMeshAgent 세팅
        navMeshAgent = GetComponent<NavMeshAgent>();
        // NavMeshAgent 이동 속도 조절
        navMeshAgent.speed = enemyInfo.GetMovingSpeed();
        navMeshAgent.acceleration = 50;
        // 회전 속도는 탐지 대상에 고정되게 설정
        navMeshAgent.angularSpeed = 360;
        // NavMeshAgent 정지 거리
        //navMeshAgent.stoppingDistance = enemyInfo.GetAttackRange();
        navMeshAgent.stoppingDistance = 0.1f;

        //navMeshAgent.updateRotation = false;

        // 애니메이터 컨트롤 세팅
        eac = GetComponent<EnemyAnimationControll>();
    }

    private void FixedUpdate()
    {
        // 살아있는 상태라면
        if (!enemyInfo.GetIsDead())
        {
            // 탐지 대상이 시야각 안에 존재 && 인식 거리 안에 존재한다면
            if ((Mathf.Acos(Vector3.Dot(transform.forward, (enemyInfo.target.transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= enemyInfo.GetDetectAngle() * 0.5f
                && enemyInfo.GetDistanceFromTarget() <= enemyInfo.GetDetectRadius())
            {
                // 추격 중이 true가 아니라면
                if (enemyInfo.GetIsTracking() != true)
                {
                    // 추격 중 -> true
                    enemyInfo.SetIsTracking(true);
                    // 이동 애니메이션
                    eac.SetAnimationState(EnemyAnimationControll.Animation_State.Move);
                }
            }
            // 피격 중이 아니라면
            if (!enemyInfo.GetIsAttacked())
            {
                // 추격 중이라면
                if (enemyInfo.GetIsTracking())
                {
                    // 탐지 대상이 인식 거리 밖에 있다면
                    if (enemyInfo.GetDistanceFromTarget() > enemyInfo.GetDetectRadius())
                    {
                        // 추격 중 -> false
                        enemyInfo.SetIsTracking(false);
                        // ~~~~~ 인식거리를 두 개로 나눠서 작은 범위에서 추격 시작하고 큰 범위에서 추격 중지하게 하면?
                    }


                    // 공격 사거리 안이라면
                    if (enemyInfo.GetIsInAttackRange())
                    {
                        // 제자리 정지
                        navMeshAgent.updateRotation = false;
                        navMeshAgent.speed = 0;
                        navMeshAgent.velocity = Vector3.zero;

                        // 공격 중이 아니라면
                        if (!enemyInfo.GetIsAttacking())
                        {
                            //navMeshAgent.SetDestination(enemyInfo.target.transform.position);
                            transform.LookAt(enemyInfo.target.transform);
                            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Idle);

                        }
                        // 공격 중이라면
                        else
                        {
                        }
                    }
                    // 공격 사거리 밖이라면
                    else
                    {
                        if (!enemyInfo.GetIsAttacking())
                        {
                            navMeshAgent.updateRotation = true;
                            navMeshAgent.speed = enemyInfo.GetMovingSpeed();
                            // 탐지 대상을 바라봄
                            //transform.LookAt(enemyInfo.target.transform);
                            // 추격
                            navMeshAgent.SetDestination(enemyInfo.target.transform.position);
                            // 이동 애니메이션
                            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Move);
                        }
                    }
                }
                // 추격 중이 아니라면
                else
                {
                    // 공격 사거리 밖에 있고 공격 중이 아니라면
                    if (!enemyInfo.GetIsInAttackRange() && !enemyInfo.GetIsAttacking())
                    {
                        // 현재 위치부터 원래 위치까지의 거리가 navmesh 정지 거리 이상이라면
                        if (Vector3.Distance(transform.position, originPosition) > navMeshAgent.stoppingDistance)
                        {
                            navMeshAgent.updateRotation = true;
                            navMeshAgent.speed = enemyInfo.GetMovingSpeed();
                            // 제자리로 돌아가기
                            navMeshAgent.SetDestination(originPosition);
                            // 이동 애니메이션
                            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Move);
                        }
                        // 원래 위치에 도착했다면
                        else
                        {
                            // 정지
                            //navMeshAgent.velocity = Vector3.zero;
                            // 원래 회전 방향으로 돌아감
                            transform.rotation = originRotation;
                            eac.SetAnimationState(EnemyAnimationControll.Animation_State.Idle);
                        }
                    }
                }
            }
            // 피격 중이라면
            else
            {
                // 정지
                navMeshAgent.velocity = Vector3.zero;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 디버그용 enemyInfo 초기화
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
            // 좌우 시야각 기즈모 표시(보라색)
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(enemyInfo.GetDetectAngle() * 0.5f, transform.up) * transform.forward) * enemyInfo.GetDetectRadius(), Color.magenta);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-enemyInfo.GetDetectAngle() * 0.5f, transform.up) * transform.forward) * enemyInfo.GetDetectRadius(), Color.magenta);
            // 바라보는 방향 기즈모 표시(노란색)
            Debug.DrawRay(transform.position, transform.forward * enemyInfo.GetDetectRadius(), Color.yellow);
#if UNITY_EDITOR
            // 인식 거리 기즈모 표시(흰색)
            Handles.DrawWireDisc(transform.position, transform.up, enemyInfo.GetDetectRadius());
#endif
        }
    }
}
