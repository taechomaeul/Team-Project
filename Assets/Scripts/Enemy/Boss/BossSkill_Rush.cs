using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Rush : MonoBehaviour
{
    Enemy enemyInfo;
    Boss bossInfo;

    private void Awake()
    {
        enemyInfo = transform.parent.GetComponent<BossInfo>().stat;
        bossInfo = enemyInfo as Boss;
    }

    private void FixedUpdate()
    {
        transform.parent.Translate(0, 0, bossInfo.GetMovingSpeed() * Time.deltaTime * 2);
    }
}
