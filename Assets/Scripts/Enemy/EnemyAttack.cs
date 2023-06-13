using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject enemy;
    [Range(0f, 1f)] public float atkRandomRatio;
    EnemyInfo enemyInfo;
    DamageCalc damageCalc;

    private void Awake()
    {
        enemyInfo = enemy.GetComponent<EnemyInfo>();
        damageCalc = enemy.GetComponent<DamageCalc>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().BeAttacked(damageCalc.DamageRandomCalc(enemyInfo.GetDamage(), atkRandomRatio));
        }
    }
}
