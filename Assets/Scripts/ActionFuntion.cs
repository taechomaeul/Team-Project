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
    /// �÷��̾��� ȥ���� �ִ�ġ(maxHp = 100)�� �ѱ� �� ����.
    /// 
    /// ex. �ִ� ü�� 100, ���� ü�� 99, ȸ���Ϸ��� ����  2
    /// 2�� 1���� Ŀ�� �ִ�ü�º��� ���� ���ڷ� ȸ���ϰ� �ȴ�.
    /// ���� 100 - 99 �� ����(= 1)�� ���� ü��(99)�� ���ϰ�(99+1 = 100),
    /// ��ȥ���� ����(N)���� ���� �ִ� ü���� ���� �ʰ� & �ùٸ��� �� ��(N-1)�� ��ȥ���� �����Ѵ�.
    /// </summary>
    /// <param name="hp">�ʴ� ȸ���ϴ� HP</param>
    public void FillHpUsingStone(float hp)
    {
        timer.CountSeconds(coolTime); //������ ��Ÿ�Ӹ�ŭ ��ٸ� �Ŀ� HP �ջ�
        if (plInfo.maxHp - plInfo.curHp < hp) //���� �ִ�ü��-����ü�� ���� ȸ���Ϸ��� ���ڰ� �� ũ�ٸ�
        {
            float subHp = plInfo.maxHp - plInfo.curHp;
            plInfo.curHp += subHp;
            plInfo.soulHp -= subHp;
            return;
            //ȸ���Ϸ��� ���ڰ� �ƴ϶� �� ���ڸ�ŭ�� ũ�⸦ ���ϰ� ����.
        }
        plInfo.curHp += hp;
        plInfo.soulHp -= hp;
    }
}
