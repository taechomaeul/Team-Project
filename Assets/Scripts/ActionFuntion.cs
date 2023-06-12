using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFuntion : MonoBehaviour
{

    public PlayerInfo plInfo;

    private void Start()
    {
        plInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    /// <summary>
    /// ��ȥ������ ��ȥ�� ���Ը� �Ű� �÷��̾��� ȥ��(HP)�� ä��� �Լ�
    /// �÷��̾��� ȥ��(HP)�� �ִ� ��ġ(maxHP)�� �ѱ� �� ����.
    /// </summary>
    /// <param name="hp">��ȥ���� ��� ��ȥ�� ����(HP)</param>
    public void MoveSoulToPlayer(float hp)
    {
        plInfo.curHp += hp;
        if (plInfo.curHp > plInfo.maxHp)
        {
            plInfo.curHp = plInfo.maxHp;
        }
    }
}
