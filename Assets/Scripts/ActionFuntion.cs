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
    /// ��ȥ������ ��ȥ�� ���Ը� �Ű� ��ȥ��(HP)�� ä��� �Լ�
    /// ��ȥ���� ����(HP)�� �ִ� ��ġ(soulHp = 666)�� �ѱ� �� ����.
    /// </summary>
    /// <param name="hp">��ȥ���� ��� ��ȥ�� ����(HP)</param>
    public void MoveSoulToStone(float hp)
    {
        plInfo.soulHp += hp;
    }

    /// <summary>
    /// ��ȥ���� ����Ͽ� �÷��̾��� ȥ��(HP)�� ä��� �Լ�
    /// �÷��̾��� ȥ���� �ִ�ġ(maxHp)�� �ѱ� �� ����.
    /// </summary>
    /// <param name="hp">�ʴ� ȸ���ϴ� HP</param>
    public void FillHpUsingStone(float hp)
    {
        timer.CountSeconds(coolTime); //������ ��Ÿ�Ӹ�ŭ ��ٸ� �Ŀ� HP �ջ�
        plInfo.curHp += hp;
        if (plInfo.curHp > plInfo.maxHp)
        {
            plInfo.curHp = plInfo.maxHp;
        }
    }
}
