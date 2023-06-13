using UnityEditor;
using UnityEngine;

public class RecognitionTest : MonoBehaviour
{
    // 디버그 용
    [Header("기즈모(개발용)")]
    // 기즈모 on/off 스위치
    [SerializeField] bool isDebug;

    [Header("인식 범위")]
    // 시야각
    [SerializeField] float detectAngle;
    // 인식 거리
    [SerializeField] float detectRadius;

    [Header("탐지 대상")]
    // 탐지 대상(플레이어)
    public GameObject target;

    void FixedUpdate()
    {
        // 탐지 대상이 시야각 안에 존재 && 인식 거리 안에 존재한다면
        if ((Mathf.Acos(Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized)) * Mathf.Rad2Deg) <= detectAngle * 0.5f
            && Vector3.Distance(transform.position, target.transform.position) <= detectRadius)
        {
            // 탐지 대상을 바라봄
            transform.LookAt(target.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        // 디버그 스위치가 켜져있다면
        if (isDebug)
        {
            // 좌우 시야각 기즈모 표시
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(detectAngle * 0.5f, transform.up) * transform.forward) * detectRadius, Color.magenta);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-detectAngle * 0.5f, transform.up) * transform.forward) * detectRadius, Color.magenta);
            // 바라보는 방향 기즈모 표시
            Debug.DrawRay(transform.position, transform.forward * detectRadius, Color.yellow);
            // 인식 거리 기즈모 표시
            Handles.DrawWireDisc(transform.position, transform.up, detectRadius);
        }
    }
}
