using System;
using System.ComponentModel;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [Header("적 정보")]
    [Tooltip("적 정보")]
    [SerializeField] internal Enemy stat = new();

    // 데이터 파일 추가되면 여기서 수치 초기화
    private void Awake()
    {
        // 체력 초기화
        stat.currentHp = stat.maxHp;
        // 탐지 대상 초기화
        stat.target = GameObject.Find("Player").transform.GetChild(0).gameObject;
        // 컴포넌트 대상 트랜스폼 초기화
        stat.transfom = transform;
    }
}

[Serializable]
internal class Enemy
{
    [Header("디버그")]
    [Tooltip("기즈모 on/off 스위치")]
    [SerializeField] bool isDebug;

    [Header("탐지 대상")]
    [Tooltip("탐지 대상(플레이어)")]
    internal GameObject target;
    // 컴포넌트 부착 대상
    internal Transform transfom;

    [Header("이동")]
    [Tooltip("이동 속도")]
    [SerializeField] internal float movingSpeed;

    [Header("인식")]
    [Tooltip("시야각")]
    [SerializeField] internal float detectAngle;

    [Tooltip("인식 거리")]
    [SerializeField] internal float detectRadius;

    [Header("체력")]
    [Tooltip("최대 체력")]
    [SerializeField] internal int maxHp;

    [Tooltip("현재 체력")]
    [SerializeField] internal int currentHp;

    [Header("공격")]
    [Tooltip("데미지")]
    [SerializeField] internal int damage;

    [Tooltip("공격 주기")]
    [SerializeField] internal float attackCycle;

    [Tooltip("공격 사거리")]
    [SerializeField] internal float attackRange;

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
    public GameObject CurrentTarget() { return target; }
    public bool GetIsDebug() { return isDebug; }
    public bool GetIsTracking() { return isTracking; }
    public bool GetIsInAttackRange() { return isInAttackRange; }
    public bool GetCanAttack() { return canAttack; }
    public bool GetIsAttacking() { return isAttacking; }
    public bool GetIsAttacked() { return isAttacked; }
    public bool GetIsDead() { return isDead; }

    public float GetMovingSpeed() { return movingSpeed; }
    public float GetDetectAngle() { return detectAngle; }
    public float GetDetectRadius() { return detectRadius; }
    public int GetMaxHp() { return maxHp; }
    public int GetCurrentHp() { return currentHp; }
    public int GetDamage() { return damage; }
    public float GetAttackCycle() { return attackCycle; }
    public float GetAttackRange() { return attackRange; }
    public float GetDistanceFromTarget()
    {
        if (transfom != null)
        {
            return Vector3.Distance(transfom.position, target.transform.position);
        }
        else
        {
            return -1;
        }
    }
    public Vector3 GetDirectionVectorFromTarget()
    {
        Vector3 temp = transfom.position;
        temp.y = 0;
        return (target.transform.position - temp).normalized;
    }
    #endregion

    // 변수 세팅 함수들
    #region Set Functions
    public void SetIsTracking(bool tf) { isTracking = tf; }
    public void SetIsInAttackRange(bool tf) { isInAttackRange = tf; }
    public void SetCanAttack(bool tf) { canAttack = tf; }
    public void SetIsAttacking(bool tf) { isAttacking = tf; }
    public void SetIsAttacked(bool tf) { isAttacked = tf; }
    public void SetIsDead(bool tf) { isDead = tf; }
    public void SetCurrentHp(int hp) { currentHp = hp; }
    #endregion
}
