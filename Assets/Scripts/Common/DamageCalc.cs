using UnityEngine;

public class DamageCalc : MonoBehaviour
{
    private void Start()
    {
        // �׽�Ʈ�� �ӽ� ��
        Debug.Log(DamageRandomCalc(10, 0.3f));
    }

    // ���ذ�(damage)���� +- ����(rangeValue) ������ ������ �� ��ȯ, rangeValue�� 0~1������ �Ҽ����� ����
    public float DamageRandomCalc(float damage, float rangeValue)
    {
        // ������ 0 �̸��̰ų� 1 �ʰ��� ���
        if (rangeValue < 0 || rangeValue > 1)
        {
            // rangeValue 0����
            Debug.Log("�߸��� �Է°�");
            rangeValue = 0;
        }
        damage *= (int)(Random.Range(1 - rangeValue, 1 + rangeValue) * 100) * 0.01f;
        return damage;
    }
}
