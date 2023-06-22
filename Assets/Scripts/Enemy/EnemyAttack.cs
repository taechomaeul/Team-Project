using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("스킬 여부")]
    [SerializeField] bool isSkill;
    [Tooltip("랜덤 수치 범위(%)")]
    [SerializeField][Range(0f, 100f)] float atkRandomRatio;

    Enemy enemyInfo;
    Boss bossInfo;

    private void Awake()
    {
        // 일반 몬스터라면
        if (transform.parent.GetComponent<BossInfo>() == null)
        {
            enemyInfo = transform.parent.GetComponent<EnemyInfo>().stat;
            isSkill = false;
        }
        // 보스라면
        else
        {
            enemyInfo = transform.parent.GetComponent<BossInfo>().stat;
            bossInfo = enemyInfo as Boss;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 스킬이라면
            if (isSkill)
            {
                // 스킬 데미지, 설정한 랜덤 범위 수치 계산 후 플레이어 체력에서 차감
                other.GetComponentInParent<PlayerController>().BeAttacked(DamageManager.Instance.DamageRandomCalc(bossInfo.GetSkillDamage(), atkRandomRatio * 0.01f));
            }
            else
            {
                // 평타 데미지, 설정한 랜덤 범위 수치 계산 후 플레이어 체력에서 차감
                other.GetComponentInParent<PlayerController>().BeAttacked(DamageManager.Instance.DamageRandomCalc(enemyInfo.GetDamage(), atkRandomRatio * 0.01f));
            }
        }
    }
}
