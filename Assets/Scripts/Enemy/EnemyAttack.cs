using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject enemy;
    [Tooltip("랜덤 범위")]
    [SerializeField][Range(0f, 1f)] float atkRandomRatio;

    EnemyInfo enemyInfo;
    DamageCalc damageCalc;

    private void Awake()
    {
        // 일반 몬스터라면
        if (GetComponent<BossInfo>() == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // 보스라면
        else
        {
            enemyInfo = GetComponent<BossInfo>();
        }
        // 데미지 계산용
        damageCalc = enemy.GetComponent<DamageCalc>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 설정된 기준값, 범위로 데미지 계산 후 플레이어 체력에서 차감
            other.GetComponentInParent<PlayerController>().BeAttacked(damageCalc.DamageRandomCalc(enemyInfo.GetDamage(), atkRandomRatio));
        }
    }
}
