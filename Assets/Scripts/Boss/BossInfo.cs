using System;
using UnityEngine;

public class BossInfo : EnemyInfo
{
    [Serializable]
    public class BossSkill
    {
        [Header("스킬")]
        [Tooltip("스킬 공격력")]
        [SerializeField] internal int skillDamage;

        [Tooltip("스킬 재사용 대기시간")]
        [SerializeField] internal float skillCoolDown;

        [Tooltip("패턴 시작 체력")]
        [SerializeField] internal int skillPhaseHp;
    }

    [Header("보스 스킬")]
    [Tooltip("보스 스킬 관련 정보들")]
    [SerializeField] BossSkill bossSkill;

    [Header("현재 상태")]
    [Tooltip("스킬 사용 가능")]
    [SerializeField] bool canSkill;

    #region Get Functions
    public bool GetCanSkill() { return canSkill; }
    public int GetSkillDamage() { return bossSkill.skillDamage; }
    public float GetSkillCoolDown() {  return bossSkill.skillCoolDown; }
    public int GetSkillPhaseHp() { return bossSkill.skillPhaseHp; }
    #endregion
}
