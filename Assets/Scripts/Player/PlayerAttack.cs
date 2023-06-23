using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject player;
    [Range(0f, 1f)] public float atkRandomRatio;

    PlayerInfo plInfo;

    private void Awake()
    {
        plInfo = player.GetComponent<PlayerInfo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 범위에 적이 닿았을 때
        if (other.CompareTag("Enemy"))
        {
            // 랜덤 데미지 계산 후 적 체력 감소
            other.GetComponent<EnemyBeAttacked>().BeAttacked(DamageManager.Instance.DamageRandomCalc(plInfo.plAtk, atkRandomRatio));
        }
    }
}
