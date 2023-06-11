using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // 적 정보
    EnemyInfo enemyInfo;

    void Start()
    {
        // enemyInfo 초기화
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
    }

    void FixedUpdate()
    {
        // 적 현재 체력이 0 이하라면
        if (enemyInfo.GetCurrentHp() <= 0)
        {
            // 적 사망
            enemyInfo.SetIsDead(true);
        }
    }

    // damage만큼 공격 받음
    public void BeAttacked(float damage)
    {
        // 현재 체력에서 damage만큼 차감
        enemyInfo.SetCurrentHp(enemyInfo.GetCurrentHp() - damage);
    }
}
