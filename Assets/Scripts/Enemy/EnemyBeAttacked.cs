using System.Collections;
using UnityEngine;

public class EnemyBeAttacked : MonoBehaviour
{
    // �� ����
    EnemyInfo enemyInfo;

    // ��ȥ��
    public GameObject soulStone;

    void Start()
    {
        // enemyInfo �ʱ�ȭ
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // ��ȥ�� off
        soulStone.SetActive(false);
    }

    //void FixedUpdate()
    //{
    //}

    // damage��ŭ ���� ����
    public void BeAttacked(float damage)
    {
        // ���� ü�¿��� damage��ŭ ����
        enemyInfo.SetCurrentHp(enemyInfo.GetCurrentHp() - damage);

        // �� ���� ü���� 0 ���϶��
        if (enemyInfo.GetCurrentHp() <= 0)
        {
            // �� ���
            enemyInfo.SetIsDead(true);
        }

        // ���� �ڷ�ƾ ����
        StopAllCoroutines();

        // �׾��ٸ�
        if (enemyInfo.GetIsDead())
        {
            // ��� �ִϸ��̼� ���
            StartCoroutine(HitAnimation());
        }
        // ����ִٸ�
        else
        {
            // �ǰ� �ִϸ��̼� ���(�׽�Ʈ�� ���ǽð���)
            StartCoroutine(HitAnimation(1));
        }
    }

    // �ǰ� �ִϸ��̼�(�ִϸ��̼� �ð�)
    IEnumerator HitAnimation(float time)
    {
        // ����ִٸ�
        if (!enemyInfo.GetIsDead())
        {
            // ���� ���� �ƴ϶��
            if (!enemyInfo.GetIsAttacking())
            {
                // �ǰ� �ִϸ��̼� ���
                Debug.Log("��������");

                // �ִϸ��̼� ���� ������ ��ٸ�
                yield return new WaitForSeconds(time);
                Debug.Log("��������2");
            }

            // Ž�� ����� �ν����� ���ϰ� �ִٸ�
            if (!enemyInfo.GetIsTracking())
            {
                // �߰� �� -> true
                enemyInfo.SetIsTracking(true);
                // Ž�� ��󿡰� ȸ����
                transform.LookAt(enemyInfo.target.transform.position);
            }
        }
    }

    IEnumerator HitAnimation()
    {
        // �׾��ٸ�
        if (enemyInfo.GetIsDead())
        {
            // ��� �ִϸ��̼� ���
            Debug.Log("��� �ִϸ��̼�");

            // �ִϸ��̼� ���� ������ ��ٸ�(���ǰ�)
            yield return new WaitForSeconds(2);

            // ��ȥ�� on
            soulStone.SetActive(true);
        }
    }
}
