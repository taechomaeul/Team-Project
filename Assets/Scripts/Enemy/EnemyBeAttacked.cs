using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // �� ����
    EnemyInfo enemyInfo;

    void Start()
    {
        // enemyInfo �ʱ�ȭ
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
    }

    void FixedUpdate()
    {
        // �� ���� ü���� 0 ���϶��
        if (enemyInfo.GetCurrentHp() <= 0)
        {
            // �� ���
            enemyInfo.SetIsDead(true);
        }
    }

    // damage��ŭ ���� ����
    public void BeAttacked(float damage)
    {
        // ���� ü�¿��� damage��ŭ ����
        enemyInfo.SetCurrentHp(enemyInfo.GetCurrentHp() - damage);
    }
}
