using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    // �� ����
    EnemyInfo enemyInfo;
    // ���� ���� ���൵
    float attackProgress;

    [Header("���� ���� ����")]
    [Tooltip("���� ���� ����")]
    [SerializeField] GameObject attackRange;


    void Start()
    {
        // enemyInfo �ʱ�ȭ
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // ���� ���� off
        attackRange.SetActive(false);
        // ���� ���൵ �ʱ�ȭ
        attackProgress = 0f;
        // ���� ���� �ʱ�ȭ
        enemyInfo.SetCanAttack(true);

    }

    void FixedUpdate()
    {
        // ����ִ� ���¶��
        if (!enemyInfo.GetIsDead())
        {
            // Ž�� ����� ���� ��Ÿ� �ȿ� �����Ѵٸ�
            if (Vector3.Distance(transform.position, enemyInfo.target.transform.position) <= enemyInfo.GetAttackRange())
            {
                // ���� ��Ÿ� ���� -> true
                enemyInfo.SetIsInAttackRange(true);
                // ���� ������ true���
                if (enemyInfo.GetCanAttack())
                {
                    // ���� ���� true�� �ƴ϶��
                    if (enemyInfo.GetIsAttacking() != true)
                    {
                        // ���� �� -> true
                        enemyInfo.SetIsAttacking(true);
                        // ���� ���� -> false
                        enemyInfo.SetCanAttack(false);
                        // ���� ����(���� ���� �ð��� �ӽ÷� ���ǰ� ����)
                        StartCoroutine(Attack(enemyInfo.GetAttackCycle() * 0.5f));
                        StartCoroutine(AttackTimer());
                    }

                }
            }
            // Ž�� ����� ���� ��Ÿ� ���̶��
            else
            {
                // ���� ��Ÿ� ���� -> false
                enemyInfo.SetIsInAttackRange(false);
            }
        }
    }

    // ����(���� ���� �ð�)
    IEnumerator Attack(float attackTime)
    {
        // ���� ���� true���
        if (enemyInfo.GetIsAttacking())
        {
            // ���� ���� on
            attackRange.SetActive(true);
            // attackTime ��ŭ ��ٸ�(���� ��� ���ۺ��� �������� �ð�)
            yield return new WaitForSeconds(attackTime);
            // ���� ���� off
            attackRange.SetActive(false);
            // ���� �� -> false
            enemyInfo.SetIsAttacking(false);
        }
        // ���� ���� false���
        else
        {
            // ���� �ڷ�ƾ ����
            yield return null;
        }
    }

    // ���� ���� ��� Ÿ�̸�
    IEnumerator AttackTimer()
    {
        while (true)
        {
            // ���� ���൵�� ���� �ֱ⸦ ������
            if (attackProgress >= enemyInfo.GetAttackCycle())
            {
                // �������൵ �ʱ�ȭ
                attackProgress = 0f;
                // ���� ���� -> true
                enemyInfo.SetCanAttack(true);
                // Ÿ�̸� �ڷ�ƾ ����
                break;
            }
            // �� ������ ���� ���൵�� ��� �ð� �߰�
            attackProgress += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        // ����׿� enemyInfo �ʱ�ȭ
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }

        // ����� ����ġ�� �����ִٸ�
        if (enemyInfo.GetIsDebug())
        {
            // ���� ��Ÿ� ����� ǥ��(������)
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, enemyInfo.GetAttackRange());
        }
    }
}
