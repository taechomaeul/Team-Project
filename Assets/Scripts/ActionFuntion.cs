using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFuntion : MonoBehaviour
{
    public float coolTime = 2f;
    public PlayerInfo plInfo;
    public SoulController soulController;
    public Timer timer;

    private void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    /// <summary>
    /// 영혼석에서 영혼의 무게를 옮겨 영혼석(HP)을 채우는 함수
    /// 영혼석의 무게(HP)은 최대 수치(soulHp = 666)를 넘길 수 없다.
    /// </summary>
    /// <param name="hp">영혼석에 담긴 영혼의 무게(HP)</param>
    public void MoveSoulToStone(float hp)
    {
        plInfo.soulHp += hp;
    }

    /// <summary>
    /// 영혼석을 사용하여 플레이어의 혼력(HP)을 채우는 함수
    /// 플레이어의 혼력은 최대치(maxHp)를 넘길 수 없다.
    /// </summary>
    /// <param name="hp">초당 회복하는 HP</param>
    public void FillHpUsingStone(float hp)
    {
        timer.CountSeconds(coolTime); //설정된 쿨타임만큼 기다린 후에 HP 합산
        plInfo.curHp += hp;
        if (plInfo.curHp > plInfo.maxHp)
        {
            plInfo.curHp = plInfo.maxHp;
        }
    }
}
