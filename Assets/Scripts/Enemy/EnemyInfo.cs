using System;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [Header("�����")]
    [Tooltip("����� on/off ����ġ")]
    [SerializeField] bool isDebug;

    [Header("Ž�� ���")]
    [Tooltip("Ž�� ���(�÷��̾�)")]
    public GameObject target;

    [Serializable]
    public class Stat
    {
        [Header("�̵�")]
        [Tooltip("�̵� �ӵ�")]
        [SerializeField] internal float movingSpeed;

        [Header("�ν�")]
        [Tooltip("�þ߰�")]
        [SerializeField] internal float detectAngle;

        [Tooltip("�ν� �Ÿ�")]
        [SerializeField] internal float detectRadius;

        [Header("ü��")]
        [Tooltip("�ִ� ü��")]
        [SerializeField] internal float maxHp;

        [Tooltip("���� ü��")]
        [SerializeField] internal float currentHp;

        [Header("����")]
        [Tooltip("������")]
        [SerializeField] internal float damage;

        [Tooltip("���� �ֱ�")]
        [SerializeField] internal float attackCycle;

        [Tooltip("���� ��Ÿ�")]
        [SerializeField] internal float attackRange;
    }

    [Header("�� ����")]
    [SerializeField] Stat stat;

    [Header("���� ����")]
    [Tooltip("�߰� ��")]
    [SerializeField] bool isTracking;

    [Tooltip("���� ��Ÿ� ����")]
    [SerializeField] bool isInAttackRange;

    [Tooltip("���� ����")]
    [SerializeField] bool canAttack;

    [Tooltip("���� ��")]
    [SerializeField] bool isAttacking;

    [Tooltip("�ǰ� ��")]
    [SerializeField] bool isAttacked;

    [Tooltip("���")]
    [SerializeField] bool isDead;

    // �ܺο��� ���� ���� ���� ��ȯ �Լ���
    #region Get Functions
    public GameObject CurrentTarget() { return target; }
    public bool GetIsDebug() { return isDebug; }
    public bool GetIsTracking() { return isTracking; }
    public bool GetIsInAttackRange() {  return isInAttackRange; }
    public bool GetCanAttack() {  return canAttack; }
    public bool GetIsAttacking() { return isAttacking; }
    public bool GetIsAttacked() { return isAttacked; }
    public bool GetIsDead() { return isDead; }

    public float GetMovingSpeed() { return stat.movingSpeed; }
    public float GetDetectAngle() { return stat.detectAngle; }
    public float GetDetectRadius() { return stat.detectRadius; }
    public float GetMaxHp() { return stat.maxHp; }
    public float GetCurrentHp() { return stat.currentHp; }
    public float GetDamage() { return stat.damage; }
    public float GetAttackCycle() { return stat.attackCycle; }
    public float GetAttackRange() { return stat.attackRange; }
    #endregion

    // ���� ���� �Լ���
    #region Set Functions
    public void SetIsTracking(bool tf) { isTracking = tf; }
    public void SetIsInAttackRange(bool tf) { isInAttackRange = tf;}
    public void SetCanAttack(bool tf) {  canAttack = tf; }
    public void SetIsAttacking(bool  tf) { isAttacking = tf; }
    public void SetIsAttacked(bool tf) { isAttacked = tf;}
    public void SetIsDead(bool tf) { isDead = tf;}
    public void SetCurrentHp(float hp) { stat.currentHp = hp; }
    #endregion

    // ������ ���� �߰��Ǹ� ���⼭ ��ġ �ʱ�ȭ
    private void Awake()
    {
        stat.currentHp = stat.maxHp;
    }
}
