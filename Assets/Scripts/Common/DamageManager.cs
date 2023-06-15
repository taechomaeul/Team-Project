using UnityEngine;

public class DamageManager : MonoBehaviour
{
    static DamageManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static DamageManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }



    // 기준값(damage)에서 +- 범위(rangeValue) 사이의 랜덤한 값 반환, rangeValue는 0~1사이의 소수값을 가짐
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
        damage = (int)((1+ Random.Range(-rangeValue, rangeValue)) *damage);
        Debug.Log(damage);
        return damage;
    }
}
