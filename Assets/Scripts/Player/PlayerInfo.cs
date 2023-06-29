using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int plAtk;
    public int curHp;
    public int maxHp;
    public int soulHp;
    public float plMoveSpd;
    [SerializeField]
    public Skill curSkill;
    public int curPrefabIndex;
    public int curPositionIndex;
}
