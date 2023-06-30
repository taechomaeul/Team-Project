using UnityEngine;

public class BossSkill_Rush : MonoBehaviour
{
    // 적 정보
    private Enemy enemyInfo;
    private Boss bossInfo;



    private void Awake()
    {
        // 적 정보 초기화
        enemyInfo = transform.parent.GetComponent<BossInfo>().stat;
        bossInfo = enemyInfo as Boss;
    }

    private void FixedUpdate()
    {
        // 부모 오브젝트(적)를 앞으로 이동 시킴
        transform.parent.Translate(0, 0, bossInfo.GetMovingSpeed() * Time.deltaTime * 2);
    }
}