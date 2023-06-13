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
    /// 플레이어의 혼력은 최대치(maxHp = 100)를 넘길 수 없다.
    /// 
    /// ex. 최대 체력 100, 현재 체력 99, 회복하려는 숫자  2
    /// 2가 1보다 커서 최대체력보다 높은 숫자로 회복하게 된다.
    /// 따라서 100 - 99 한 숫자(= 1)를 현재 체력(99)에 더하고(99+1 = 100),
    /// 영혼석의 숫자(N)에는 빼서 최대 체력을 넘지 않게 & 올바르게 뺀 값(N-1)을 영혼석에 저장한다.
    /// </summary>
    /// <param name="hp">초당 회복하는 HP</param>
    public void FillHpUsingStone(float hp)
    {
        timer.CountSeconds(coolTime); //설정된 쿨타임만큼 기다린 후에 HP 합산
        if (plInfo.maxHp - plInfo.curHp < hp) //만약 최대체력-현재체력 보다 회복하려는 숫자가 더 크다면
        {
            float subHp = plInfo.maxHp - plInfo.curHp;
            plInfo.curHp += subHp;
            plInfo.soulHp -= subHp;
            return;
            //회복하려는 숫자가 아니라 뺀 숫자만큼의 크기를 더하고 뺀다.
        }
        plInfo.curHp += hp;
        plInfo.soulHp -= hp;
    }
}
