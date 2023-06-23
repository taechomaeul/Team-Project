using System;
using UnityEngine;

public class BossInfo : MonoBehaviour
{
    [Header("보스 정보")]
    [Tooltip("보스 정보")]
    [SerializeField] internal Boss stat = new();

    // 데이터 파일 추가되면 여기서 수치 초기화
    private void Awake()
    {
        // 체력 초기화
        stat.SetCurrentHp(stat.GetMaxHp());
        // 탐지 대상 초기화
        stat.SetTarget(GameObject.Find("Player").transform.GetChild(0).gameObject);
        // 컴포넌트 대상 트랜스폼 초기화
        stat.SetTransform(transform);
    }
}

[Serializable]
internal class Boss : Enemy
{
    [Header("스킬")]
    [Tooltip("스킬 공격력")]
    [SerializeField] int skillDamage;

    [Tooltip("스킬 재사용 대기시간")]
    [SerializeField] float skillCoolDown;

    [Tooltip("스킬 시전 사거리")]
    [SerializeField] float skillCastRange;

    [Tooltip("패턴 시작 체력 비율(%)")]
    [SerializeField][Range(0f, 100f)] float skillPhaseHpRatio;

    [Header("현재 상태")]
    [Tooltip("스킬 사용 가능")]
    [SerializeField, ReadOnly] bool canSkill;

    [Tooltip("스킬 사거리 진입")]
    [SerializeField, ReadOnly] bool isInSkillRange;

    [Tooltip("스킬 시전 중")]
    [SerializeField, ReadOnly] bool isSkillCasting;

    // 외부에서 쓰기 위한 변수 반환 함수들
    #region Get Functions
    public int GetSkillDamage() { return skillDamage; }
    public float GetSkillCoolDown() { return skillCoolDown; }
    public float GetSkillCastRange() { return skillCastRange; }
    public float GetSkillPhaseHpRatio() { return skillPhaseHpRatio * 0.01f; }
    public bool GetCanSkill() { return canSkill; }
    public bool GetIsInSkillRange() { return isInSkillRange; }
    public bool GetIsSkillCasting() { return isSkillCasting; }
    #endregion

    // 변수 세팅 함수들
    #region Set Functions
    public void SetCanSkill(bool tf) { canSkill = tf; }
    public void SetIsInSkillRange(bool tf) { isInSkillRange = tf; }
    public void SetIsSkillCasting(bool tf) { isSkillCasting = tf; }
    #endregion
}
