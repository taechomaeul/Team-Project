using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // 적 정보
    Enemy enemyInfo;
    Boss bossInfo;

    // 인스펙터
    [Tooltip("본체")]
    [SerializeField] GameObject enemyBody;
    [Tooltip("스킬 여부")]
    [SerializeField] bool isSkill;
    [Tooltip("랜덤 수치 범위(%)")]
    [SerializeField][Range(0f, 100f)] float atkRandomRatio;



    private void Awake()
    {
        // 적 정보 초기화
        // 일반 몬스터라면
        if (enemyBody.GetComponent<BossInfo>() == null)
        {
            enemyInfo = enemyBody.GetComponent<EnemyInfo>().stat;
            isSkill = false;
        }
        // 보스라면
        else
        {
            enemyInfo = enemyBody.GetComponent<BossInfo>().stat;
            bossInfo = enemyInfo as Boss;
        }

        // 충돌 trigger 설정
        if (GetComponent<SphereCollider>() != null)
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }
        else if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (GetComponent<CapsuleCollider>() != null)
        {
            GetComponent<CapsuleCollider>().isTrigger = true;
        }
        else if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player와 충돌했다면
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