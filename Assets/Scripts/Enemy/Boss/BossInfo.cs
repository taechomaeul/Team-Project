using System;
using System.Collections.Generic;
using UnityEngine;

public class BossInfo : MonoBehaviour
{

    // 인스펙터
    [Header("보스 정보")]
    [Tooltip("보스 정보")]
    [SerializeField] internal Boss stat = new();



    private void Start()
    {
        InitStat();
    }

    /// <summary>
    /// 스탯 초기 설정
    /// </summary>
    private void InitStat()
    {
        Dictionary<string, object> statData = null;
        switch (stat.GetBossType())
        {
            case 0:
                statData = DefaultStatManager.Instance.GetMiniBossData();
                break;
            case 1:
                statData = DefaultStatManager.Instance.GetFinalBossData();
                break;
        }

        // 탐지 대상 초기화
        stat.SetTarget(GameObject.Find("Player").transform.GetChild(0).gameObject);
        // 컴포넌트 대상 트랜스폼 초기화
        stat.SetTransform(transform);
        // 체력 초기화
        stat.SetMaxHp(int.Parse(statData["maxHp"].ToString()));
        stat.SetCurrentHp(stat.GetMaxHp());
        // 이동 속도 초기화
        stat.SetMovingSpeed(float.Parse(statData["movingSpeed"].ToString()));
        // 탐지 범위 초기화
        stat.SetDetectAngle(float.Parse(statData["detectAngle"].ToString()));
        stat.SetDetectRadius(float.Parse(statData["detectRadius"].ToString()));
        // 공격 수치 초기화
        stat.SetDamage(int.Parse(statData["attack"].ToString()));
        stat.SetAttackCycle(float.Parse(statData["attackCycle"].ToString()));
        stat.SetAttackRange(float.Parse(statData["attackRange"].ToString()));
        // 스킬 수치 초기화
        stat.SetSkillDamage(int.Parse(statData["skillDamage"].ToString()));
        stat.SetSkillCoolDown(float.Parse(statData["skillCoolDown"].ToString()));
        stat.SetSkillCastRange(float.Parse(statData["skillCastRange"].ToString()));
        stat.SetSkillPhaseRatio(float.Parse(statData["skillPhaseHpRatio"].ToString()));
    }
}



// 보스 관련 클래스
[Serializable]
internal class Boss : Enemy
{
    // 보스 종류
    private enum BossType
    {
        MiniBoss,
        FinalBoss
    }

    // 인스펙터
    [Header("보스 종류")]
    [Tooltip("보스 종류")]
    [SerializeField] private BossType bossType;

    [Header("스킬")]
    [Tooltip("스킬 공격력")]
    [SerializeField] private int skillDamage;

    [Tooltip("스킬 재사용 대기시간")]
    [SerializeField] private float skillCoolDown;

    [Tooltip("스킬 시전 사거리")]
    [SerializeField] private float skillCastRange;

    [Tooltip("패턴 시작 체력 비율(%)")]
    [SerializeField][Range(0f, 100f)] private float skillPhaseHpRatio;

    [Header("현재 상태")]
    [Tooltip("스킬 사용 가능")]
    [SerializeField, ReadOnly] private bool canSkill;

    [Tooltip("스킬 사거리 진입")]
    [SerializeField, ReadOnly] private bool isInSkillRange;

    [Tooltip("스킬 시전 중")]
    [SerializeField, ReadOnly] private bool isSkillCasting;



    // 외부에서 쓰기 위한 변수 반환 함수들
    #region Get Functions
    /// <summary>
    /// 스킬 데미지 값 가져오기
    /// </summary>
    /// <returns>스킬 데미지</returns>
    public int GetSkillDamage() { return skillDamage; }

    /// <summary>
    /// 스킬 재사용 대기시간 값 가져오기
    /// </summary>
    /// <returns>재사용 대기시간</returns>
    public float GetSkillCoolDown() { return skillCoolDown; }

    /// <summary>
    /// 스킬 시전 가능 사거리 값 가져오기
    /// </summary>
    /// <returns>스킬 시전 가능 사거리</returns>
    public float GetSkillCastRange() { return skillCastRange; }

    /// <summary>
    /// 스킬 사용 페이즈 진입 비율 값 가져오기
    /// </summary>
    /// <returns>스킬 사용 페이즈 진입 체력 비율(0 ~ 1)</returns>
    public float GetSkillPhaseHpRatio() { return skillPhaseHpRatio * 0.01f; }

    /// <summary>
    /// 스킬 사용 가능 여부 반환
    /// </summary>
    /// <returns>스킬 사용 가능 여부</returns>
    public bool GetCanSkill() { return canSkill; }

    /// <summary>
    /// 스킬 사거리 진입 여부 반환
    /// </summary>
    /// <returns>스킬 사거리 진입 여부</returns>
    public bool GetIsInSkillRange() { return isInSkillRange; }

    /// <summary>
    /// 스킬 시전 중 여부 반환
    /// </summary>
    /// <returns>스킬 시전 중 여부</returns>
    public bool GetIsSkillCasting() { return isSkillCasting; }

    public int GetBossType() { return (int)bossType; }
    #endregion

    // 변수 세팅 함수들
    #region Set Functions

    /// <summary>
    /// 스킬 사용 가능 여부 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetCanSkill(bool tf) { canSkill = tf; }

    /// <summary>
    /// 스킬 사거리 진입 여부 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsInSkillRange(bool tf) { isInSkillRange = tf; }

    /// <summary>
    /// 스킬 시전 중 여부 설정
    /// </summary>
    /// <param name="tf">true or false</param>
    public void SetIsSkillCasting(bool tf) { isSkillCasting = tf; }

    /// <summary>
    /// 스킬 공격력 설정
    /// </summary>
    /// <param name="skillDamage">스킬 공격력</param>
    public void SetSkillDamage(int skillDamage) { this.skillDamage = skillDamage; }

    /// <summary>
    /// 스킬 재사용 대기시간 설정
    /// </summary>
    /// <param name="skillCoolDown">스킬 재사용 대기시간</param>
    public void SetSkillCoolDown(float skillCoolDown) { this.skillCoolDown = skillCoolDown; }

    /// <summary>
    /// 스킬 시전 사거리 설정
    /// </summary>
    /// <param name="skillCastRange">스킬 시전 사거리</param>
    public void SetSkillCastRange(float skillCastRange) { this.skillCastRange = skillCastRange; }

    /// <summary>
    /// 스킬 사용 페이즈 진입 체력 비율 설정
    /// </summary>
    /// <param name="skillPhaseHpRatio">스킬 사용 페이즈 진입 체력 비율(0 ~ 100)</param>
    public void SetSkillPhaseRatio(float skillPhaseHpRatio) { this.skillPhaseHpRatio = skillPhaseHpRatio; }
    #endregion
}