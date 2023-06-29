using System;
using UnityEngine;

public class BossInfo : MonoBehaviour
{
    // 인스펙터
    [Header("보스 정보")]
    [Tooltip("보스 정보")]
    [SerializeField] internal Boss stat = new();



    private void Awake()
    {
        // 데이터 파일 추가되면 여기서 수치 초기화

        // 체력 초기화
        stat.SetCurrentHp(stat.GetMaxHp());
        // 탐지 대상 초기화
        stat.SetTarget(GameObject.Find("Player").transform.GetChild(0).gameObject);
        // 컴포넌트 대상 트랜스폼 초기화
        stat.SetTransform(transform);
    }
}

// 보스 관련 클래스
[Serializable]
internal class Boss : Enemy
{
    // 인스펙터
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
    /// <returns>스킬 사용 페이즈 진입 비율(0 ~ 1)</returns>
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
    #endregion
}