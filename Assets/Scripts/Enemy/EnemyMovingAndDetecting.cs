using UnityEditor;
using UnityEngine;

public class EnemyMovingAndDetecting : MonoBehaviour
{
    // �� ����
    EnemyInfo enemyInfo;
    // ���� ��ġ
    Vector3 originPosition;
    Quaternion originRotation;

    void Start()
    {
        // enemyInfo �ʱ�ȭ
        if (enemyInfo == null)
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
        // originPosition, originRotation �ʱ�ȭ
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        // ����ִ� ���¶��
        if (!enemyInfo.GetIsDead())
        {
            // Ž�� ����� �þ߰� �ȿ� ���� && �ν� �Ÿ� �ȿ� �����Ѵٸ�
            if ((Mathf.Acos(Vector3.Dot(transform.forward, (enemyInfo.target.transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= enemyInfo.GetDetectAngle() * 0.5f
                && Vector3.Distance(transform.position, enemyInfo.target.transform.position) <= enemyInfo.GetDetectRadius())
            {
                // �߰� ���� true�� �ƴ϶��
                if (enemyInfo.GetIsTracking() != true)
                {
                    // �߰� �� -> true
                    enemyInfo.SetIsTracking(true);
                }
                // Ž�� ����� �ٶ�
                transform.LookAt(enemyInfo.target.transform.position);
            }

            // �߰� ���̶��
            if (enemyInfo.GetIsTracking())
            {
                // Ž�� ����� �ν� �Ÿ� �ۿ� �ִٸ�
                if (Vector3.Distance(transform.position, enemyInfo.target.transform.position) > enemyInfo.GetDetectRadius())
                {
                    // �߰� �� -> false
                    enemyInfo.SetIsTracking(false);
                    // ~~~~~ �νİŸ��� �� ���� ������ ���� �������� �߰� �����ϰ� ū �������� �߰� �����ϰ� �ϸ�?
                }

                // Ž�� ����� �ٶ�
                transform.LookAt(enemyInfo.target.transform.position);

                // ���� ��Ÿ� �ۿ� �ְ� ���� ���� �ƴ϶��
                if (!enemyInfo.GetIsInAttackRange() && !enemyInfo.GetIsAttacking())
                {
                    // �߰�(���Ŀ� NavMeshAgent ����ϰ� �Ǹ� ����)
                    transform.Translate(new Vector3(0, 0, enemyInfo.GetMovingSpeed() * Time.deltaTime));
                }
            }
            // �߰� ���� �ƴ϶��
            else
            {
                // ���� ��Ÿ� �ۿ� �ְ� ���� ���� �ƴ϶��
                if (!enemyInfo.GetIsInAttackRange() && !enemyInfo.GetIsAttacking())
                {
                    // ���� ��ġ���� ���� ��ġ������ �Ÿ��� 0.1 �̻��̶��
                    if (Vector3.Distance(transform.position, originPosition) > 0.1f)
                    {
                        // ���ڸ��� ���ư���(���Ŀ� NavMeshAgent ����ϰ� �Ǹ� ����)
                        transform.LookAt(originPosition);
                        transform.Translate(new Vector3(0, 0, enemyInfo.GetMovingSpeed() * Time.deltaTime));
                    }
                    // ���� ��ġ�� �����ߴٸ�
                    else
                    {
                        // ���� ȸ�� �������� ���ư�
                        transform.rotation = originRotation;
                    }
                }
            }
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
            // �¿� �þ߰� ����� ǥ��(�����)
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(enemyInfo.GetDetectAngle() * 0.5f, transform.up) * transform.forward) * enemyInfo.GetDetectRadius(), Color.magenta);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-enemyInfo.GetDetectAngle() * 0.5f, transform.up) * transform.forward) * enemyInfo.GetDetectRadius(), Color.magenta);
            // �ٶ󺸴� ���� ����� ǥ��(�����)
            Debug.DrawRay(transform.position, transform.forward * enemyInfo.GetDetectRadius(), Color.yellow);
            // �ν� �Ÿ� ����� ǥ��(���)
            Handles.DrawWireDisc(transform.position, transform.up, enemyInfo.GetDetectRadius());
        }
    }
}
