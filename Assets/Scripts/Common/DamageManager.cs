using UnityEngine;

public class DamageManager : MonoBehaviour
{
    // 싱글톤 세팅
    static DamageManager instance = null;
    public static DamageManager Instance
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



    private void Awake()
    {
        // 싱글톤 세팅
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 기준값(damage)에서 ±범위(rangeValue) 사이의 랜덤한 값 반환, rangeValue는 0~1사이의 소수값을 가짐
    /// </summary>
    /// <param name="damage">기준값</param>
    /// <param name="rangeValue">변동 범위(0 ~ 1)</param>
    /// <returns>범위 내의 랜덤 데미지</returns>
    public int DamageRandomCalc(int damage, float rangeValue)
    {
        // 범위가 0 미만이거나 1 초과인 경우
        if (rangeValue < 0 || rangeValue > 1)
        {
            // rangeValue 0으로
            Debug.Log("잘못된 입력값");
            rangeValue = 0;
        }
        // 데미지 값 계산 후 반환
        damage = (int)((1 + Random.Range(-rangeValue, rangeValue)) * damage);
        return damage;
    }
}