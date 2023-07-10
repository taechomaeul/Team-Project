using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Tooltip("�÷��̾� ���ݷ�")]
    public int plAtk;
    [Tooltip("�÷��̾� ���� ȥ��(HP)")]
    public int curHp;
    [Tooltip("�÷��̾� �ִ� ȥ��(HP)")]
    public int maxHp;
    [Tooltip("��ȥ���� ��� ��ȥ�� ����")]
    public int soulHp;
    [Tooltip("�÷��̾� �̵� �ӵ�")]
    public float plMoveSpd;
    [SerializeField]
    [Tooltip("���� ���� ���� ��ų")]
    public Skill curSkill;
    [Tooltip("�÷��̾� ����ü IDX")]
    public int curPrefabIndex;
    [Tooltip("�÷��̾� ��ġ IDX")]
    public int curPositionIndex;
}
