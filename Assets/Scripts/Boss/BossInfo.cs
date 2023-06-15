using System;
using UnityEngine;

public class BossInfo : MonoBehaviour
{
    [Header("보스 정보")]
    [Tooltip("보스 정보")]
    [SerializeField] internal Boss stat = new();

    private void Awake()
    {
        stat.currentHp = stat.maxHp;
    }
}

[Serializable]
internal class Boss : Enemy
{
    [Header("스킬")]
    [Tooltip("스킬 공격력")]
    [SerializeField] internal int skillDamage;

    [Tooltip("스킬 재사용 대기시간")]
    [SerializeField] internal float skillCoolDown;

    [Tooltip("패턴 시작 체력")]
    [SerializeField] internal int skillPhaseHp;

    [Header("현재 상태")]
    [Tooltip("스킬 사용 가능")]
    [SerializeField] bool canSkill;

    // 외부에서 쓰기 위한 변수 반환 함수들
    #region Get Functions
    public bool GetCanSkill() { return canSkill; }
    public int GetSkillDamage() { return skillDamage; }
    public float GetSkillCoolDown() { return skillCoolDown; }
    public int GetSkillPhaseHp() { return skillPhaseHp; }
    #endregion

    // 변수 세팅 함수들
    #region Set Functions
    public void SetCanSkill(bool tf) { canSkill = tf; }
    #endregion
}
