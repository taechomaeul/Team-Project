using System.Collections.Generic;
using UnityEngine;

public class DefaultStatManager : MonoBehaviour
{
    private static DefaultStatManager instance;
    public static DefaultStatManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // 플레이어와 적 기본 스탯 데이터
    private List<Dictionary<string, object>> statData;

    // 인스펙터
    [Header("CSV 파일")]
    [Tooltip("경로")]
    [SerializeField] string path;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        statData = CSVReader.Read(path);
    }

    /// <summary>
    /// 플레이어 기본 스탯
    /// </summary>
    /// <returns>maxHp, currentHp, movingSpeed, attack</returns>
    internal Dictionary<string, object> GetPlayerData()
    {
        return statData[0];
    }

    /// <summary>
    /// 일반 몬스터 기본 스탯
    /// </summary>
    /// <returns>maxHp, currentHp, movingSpeed, attack, detectAngle, detectRadius, attackCycle, attackRange</returns>
    internal Dictionary<string, object> GetEnemyData()
    {
        return statData[1];
    }

    /// <summary>
    /// 중간 보스 기본 스탯
    /// </summary>
    /// <returns>
    /// maxHp, currentHp, movingSpeed, attack, detectAngle, detectRadius, attackCycle, attackRange,
    /// skillDamage, skillCoolDown, skillCastRange, skillPhaseHpRatio
    /// </returns>
    internal Dictionary<string, object> GetMiniBossData()
    {
        return statData[2];
    }

    /// <summary>
    /// 최종 보스 기본 스탯
    /// </summary>
    /// <returns>
    /// maxHp, currentHp, movingSpeed, attack, detectAngle, detectRadius, attackCycle, attackRange,
    /// skillDamage, skillCoolDown, skillCastRange, skillPhaseHpRatio
    /// </returns>
    internal Dictionary<string, object> GetFinalBossData()
    {
        return statData[3];
    }
}
