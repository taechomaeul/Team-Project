using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Tooltip("플레이어 공격력")]
    public int plAtk;
    [Tooltip("플레이어 현재 혼력(HP)")]
    public int curHp;
    [Tooltip("플레이어 최대 혼력(HP)")]
    public int maxHp;
    [Tooltip("영혼석에 담긴 영혼의 무게")]
    public int soulHp;
    [Tooltip("플레이어 이동 속도")]
    public float plMoveSpd;
    [SerializeField]
    [Tooltip("현재 적용 중인 스킬")]
    public Skill curSkill;
    [Tooltip("플레이어 빙의체 IDX")]
    public int curPrefabIndex;
    [Tooltip("플레이어 위치 IDX")]
    public int curPositionIndex;
}
