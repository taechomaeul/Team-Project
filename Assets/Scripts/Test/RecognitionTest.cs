using UnityEditor;
using UnityEngine;

public class RecognitionTest : MonoBehaviour
{
    // ����� ��
    [Header("�����(���߿�)")]
    // ����� on/off ����ġ
    [SerializeField] bool isDebug;

    [Header("�ν� ����")]
    // �þ߰�
    [SerializeField] float detectAngle;
    // �ν� �Ÿ�
    [SerializeField] float detectRadius;

    [Header("Ž�� ���")]
    // Ž�� ���(�÷��̾�)
    public GameObject target;

    void FixedUpdate()
    {
        // Ž�� ����� �þ߰� �ȿ� ���� && �ν� �Ÿ� �ȿ� �����Ѵٸ�
        if ((Mathf.Acos(Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= detectAngle * 0.5f
            && Vector3.Distance(transform.position, target.transform.position) <= detectRadius)
        {
            // Ž�� ����� �ٶ�
            transform.LookAt(target.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        // ����� ����ġ�� �����ִٸ�
        if (isDebug)
        {
            // �¿� �þ߰� ����� ǥ��
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(detectAngle * 0.5f, transform.up) * transform.forward) * detectRadius, Color.magenta);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-detectAngle * 0.5f, transform.up) * transform.forward) * detectRadius, Color.magenta);
            // �ٶ󺸴� ���� ����� ǥ��
            Debug.DrawRay(transform.position, transform.forward * detectRadius, Color.yellow);
            // �ν� �Ÿ� ����� ǥ��
            Handles.DrawWireDisc(transform.position, transform.up, detectRadius);
        }
    }
}
