using UnityEngine;

public class DamageCalc : MonoBehaviour
{
    private void Start()
    {
        // 테스트용 임시 값
        Debug.Log(DamageRandomCalc(10, 0.3f));
    }

    // 기준값(damage)에서 +- 범위(rangeValue) 사이의 랜덤한 값 반환, rangeValue는 0~1사이의 소수값을 가짐
    public float DamageRandomCalc(float damage, float rangeValue)
    {
        // 범위가 0 미만이거나 1 초과인 경우
        if (rangeValue < 0 || rangeValue > 1)
        {
            // rangeValue 0으로
            Debug.Log("잘못된 입력값");
            rangeValue = 0;
        }
        damage *= (int)(Random.Range(1 - rangeValue, 1 + rangeValue) * 100) * 0.01f;
        return damage;
    }
}
