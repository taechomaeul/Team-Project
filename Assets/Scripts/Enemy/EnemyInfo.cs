using System;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    // 인스펙터
    [Header("적 정보")]
    [Tooltip("적 정보")]
    [SerializeField] internal Enemy stat = new();



    private void Awake()
    {
        // 데이터 파일 추가되면 여기서 수치 초기화

        // 체력 초기화
        stat.SetCurrentHp(stat.GetMaxHp());
        // 탐지 대상 초기화
        stat.SetTarget(GameObject.Find("Player").transform.GetChild(0).gameObject);
        // 컴포넌트 대상 트랜스폼 초기화
        stat.SetTransform(transform);
    }
}

// 적 관련 클래스
[Serializable]
internal class Enemy
{
    // 인스펙터
    [Header("디버그")]
    [Tooltip("기즈모 on/off 스위치")]
    [SerializeField] bool isDebug;

    [Header("탐지 대상")]
    [Tooltip("탐지 대상(플레이어)")]
    GameObject target;
    // 컴포넌트 부착 대상
    Transform transform;

    [Header("이동")]
    [Tooltip("이동 속도")]
    [SerializeField] float movingSpeed;

    [Header("인식")]
    [Tooltip("시야각")]
    [SerializeField] float detectAngle;

    [Tooltip("인식 거리")]
    [SerializeField] float detectRadius;

    [Header("체력")]
    [Tooltip("최대 체력")]
    [SerializeField] int maxHp;

    [Tooltip("현재 체력")]
    [SerializeField, ReadOnly(EReadOnlyType.EDITABLE_RUNTIME)] int currentHp;

    [Header("공격")]
    [Tooltip("데미지")]
    [SerializeField] int damage;

    [Tooltip("공격 주기")]
    [SerializeField] float attackCycle;

    [Tooltip("공격 사거리")]
    [SerializeField] float attackRange;

    [Header("현재 상태")]
    [Tooltip("추격 중")]
    [SerializeField, ReadOnly] bool isTracking;

    [Tooltip("공격 사거리 진입")]
    [SerializeField, ReadOnly] bool isInAttackRange;

    [Tooltip("공격 가능")]
    [SerializeField, ReadOnly] bool canAttack;

    [Tooltip("공격 중")]
    [SerializeField, ReadOnly] bool isAttacking;

    [Tooltip("피격 중")]
    [SerializeField, ReadOnly] bool isAttacked;

    [Tooltip("사망")]
    [SerializeField, ReadOnly] bool isDead;



    // 외부에서 쓰기 위한 변수 반환 함수들
    #region Get Functions
    /// <summary>
    /// 탐지 대상 오브젝트 가져오기
    /// </summary>
    /// <returns>탐지 대상 오브젝트</returns>
    public GameObject GetCurrentTarget() { return target; }

    /// <summary>
    /// 적 오브젝트의 트랜스폼 가져오기
    /// </summary>
    /// <returns>적 오브젝트의 트랜스폼</returns>
    public Transform GetTransform() { return transform; }

    /// <summary>
    /// 디버그용 기즈모 on / off 상태 가져오기
    /// </summary>
    /// <returns>기즈모 on / off bool값</returns>
    public bool GetIsDebug() { return isDebug; }

    /// <summary>
    /// 탐지 대상 추적 상태 가져오기
    /// </summary>
    /// <returns>탐지 대상 추적 on / off bool값</returns>
    public bool GetIsTracking() { return isTracking; }

    /// <summary>
    /// 공격 사거리 진입 상태 가져오기
    /// </summary>
    /// <returns>공격 사거리 진입 on / off bool값</returns>
    public bool GetIsInAttackRange() { return isInAttackRange; }

    /// <summary>
    /// 공격 가능 상태 가져오기
    /// </summary>
    /// <returns>공격 가능 상태 on / off bool값</returns>
    public bool GetCanAttack() { return canAttack; }

    /// <summary>
    /// 공격 중 상태 가져오기
    /// </summary>
    /// <returns>공격 중 상태 on / off bool값</returns>
    public bool GetIsAttacking() { return isAttacking; }

    /// <summary>
    /// 피격 중 상태 가져오기
    /// </summary>
    /// <returns>피격 중 상태 on / off bool값</returns>
    public bool GetIsAttacked() { return isAttacked; }

    /// <summary>
    /// 사망 상태 가져오기
    /// </summary>
    /// <returns>사망 상태 on / off bool값</returns>
    public bool GetIsDead() { return isDead; }

    /// <summary>
    /// 적 이동 속도 가져오기
    /// </summary>
    /// <returns>적 이동 속도</returns>
    public float GetMovingSpeed() { return movingSpeed; }

    /// <summary>
    /// 적 시야각 가져오기
    /// </summary>
    /// <returns>적 시야각</returns>
    public float GetDetectAngle() { return detectAngle; }

    /// <summary>
    /// 적 탐지 거리 가져오기
    /// </summary>
    /// <returns>적 탐지 거리</returns>
    public float GetDetectRadius() { return detectRadius; }

    /// <summary>
    /// 적 최대 체력 가져오기
    /// </summary>
    /// <returns>적 최대 체력</returns>
    public int GetMaxHp() { return maxHp; }

    /// <summary>
    /// 적 현재 체력 가져오기
    /// </summary>
    /// <returns>적 현재 체력</returns>
    public int GetCurrentHp() { return currentHp; }

    /// <summary>
    /// 적 평타 공격 데미지 가져오기
    /// </summary>
    /// <returns>적 평타 공격 데미지</returns>
    public int GetDamage() { return damage; }

    /// <summary>
    /// 적 평타 공격 주기 가져오기
    /// </summary>
    /// <returns>적 평타 공격 주기</returns>
    public float GetAttackCycle() { return attackCycle; }

    /// <summary>
    /// 적 공격 사거리 가져오기
    /// </summary>
    /// <returns>적 공격 사거리</returns>
    public float GetAttackRange() { return attackRange; }

    /// <summary>
    /// 탐지 대상까지의 거리 가져오기
    /// </summary>
    /// <returns>탐지 대상까지의 거리</returns>
    public float GetDistanceFromTarget()
    {
        // 적 오브젝트의 트랜스폼 체크
        if (GetTransform() != null)
        {
            // 적부터 탐지 대상까지의 거리 반환
            return Vector3.Distance(GetTransform().position, target.transform.position);
        }
        // 에러 처리
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 탐지 대상을 향한 방향 벡터 가져오기
    /// </summary>
    /// <returns>탐지 대상 방향의 방향 벡터</returns>
    public Vector3 GetDirectionVectorFromTarget()
    {
        Vector3 temp = GetTransform().position;
        temp.y = 0;
        return (target.transform.position - temp).normalized;
    }
    #endregion

    // 변수 세팅 함수들
    #region Set Functions
    /// <summary>
    /// 탐지 대상 설정
    /// </summary>
    /// <param name="target">탐지 대상 오브젝트</param>
    public void SetTarget(GameObject target) { this.target = target; }

    /// <summary>
    /// 적 트랜스폼 설정
    /// </summary>
    /// <param name="transform">적 오브젝트의 트랜스폼</param>
    public void SetTransform(Transform transform) { this.transform = transform; }

    /// <summary>
    /// 탐지 대상 추적 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsTracking(bool tf) { isTracking = tf; }

    /// <summary>
    /// 공격 사거리 진입 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsInAttackRange(bool tf) { isInAttackRange = tf; }

    /// <summary>
    /// 공격 가능 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetCanAttack(bool tf) { canAttack = tf; }

    /// <summary>
    /// 공격 중 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsAttacking(bool tf) { isAttacking = tf; }

    /// <summary>
    /// 피격 중 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsAttacked(bool tf) { isAttacked = tf; }

    /// <summary>
    /// 사망 상태 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsDead(bool tf) { isDead = tf; }

    /// <summary>
    /// 현재 체력 설정
    /// </summary>
    /// <param name="hp">현재 체력</param>
    public void SetCurrentHp(int hp) { currentHp = hp; }
    #endregion
}