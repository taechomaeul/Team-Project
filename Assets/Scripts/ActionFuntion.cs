using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFuntion : MonoBehaviour
{

    public PlayerInfo plInfo;
    public SoulController soulController;

    private void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    /// <summary>
    /// 영혼석에서 영혼의 무게를 옮겨 영혼석(HP)을 채우는 함수
    /// 플레이어의 혼력(HP)은 최대 수치(maxHP)를 넘길 수 없다.
    /// </summary>
    /// <param name="hp">영혼석에 담긴 영혼의 무게(HP)</param>
    public void MoveSoulToStone(float hp)
    {
        plInfo.soulHp += hp;
    }
}
