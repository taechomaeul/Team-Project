using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovingAndDetecting : MonoBehaviour
{
    // �� ����
    EnemyInfo enemyInfo;
    // ���� ��ġ
    Vector3 originPosition;
    Quaternion originRotation;
    NavMeshAgent navMeshAgent;
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

        // NavMeshAgent ����
        navMeshAgent = GetComponent<NavMeshAgent>();
        // NavMeshAgent �̵� �ӵ� ����
        navMeshAgent.speed = enemyInfo.GetMovingSpeed();
        // ȸ�� �ӵ��� Ž�� ��� �����ǰ� ����
        navMeshAgent.angularSpeed = 360;
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

                // ���� ���� �ƴ϶��
                if (!enemyInfo.GetIsAttacking())
                {

                    // ���� ��Ÿ� ���̶��
                    if (enemyInfo.GetIsInAttackRange())
                    {
                        // ����
                        navMeshAgent.speed = 0;
                    }
                    // ���� ��Ÿ� ���̶��
                    else
                    {
                        // Ž�� ����� �ٶ�
                        transform.LookAt(enemyInfo.target.transform.position);
                        // �߰�
                        navMeshAgent.speed = enemyInfo.GetMovingSpeed();
                        navMeshAgent.SetDestination(enemyInfo.target.transform.position);
                    }
                }
                // ���� ���̶��
                else
                {
                    // ���ڸ� ����
                    navMeshAgent.speed = 0;
                }
            }
            // �߰� ���� �ƴ϶��
            else
            {
                // ���� ��Ÿ� �ۿ� �ְ� ���� ���� �ƴ϶��
                if (!enemyInfo.GetIsInAttackRange() && !enemyInfo.GetIsAttacking())
                {
                    // ���� ��ġ���� ���� ��ġ������ �Ÿ��� navmesh ���� �Ÿ� �̻��̶��
                    if (Vector3.Distance(transform.position, originPosition) > navMeshAgent.stoppingDistance)
                    {
                        // ���ڸ��� ���ư���
                        navMeshAgent.SetDestination(originPosition);
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
