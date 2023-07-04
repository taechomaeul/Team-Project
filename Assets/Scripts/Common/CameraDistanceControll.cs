using UnityEngine;

public class CameraDistanceControll : MonoBehaviour
{
    // 충돌 감지용 Collider 배열
    private Collider[] tempColliderArray = new Collider[1];
    // 현재 카메라 Z 위치
    private float cameraZposition;
    // 원래 카메라 Z 위치
    private float originZ;

    // 인스펙터
    [Header("플레이어")]
    [Tooltip("플레이어")]
    [SerializeField] GameObject player;

    [Header("카메라 관련")]
    [Tooltip("거리 감소 충돌 감지 반경")]
    [SerializeField] float CameraCollisionDetectRadiusForDecrease;
    [Tooltip("거리 증가 충돌 감지 반경")]
    [SerializeField] float CameraCollisionDetectRadiusForIncrease;
    [Tooltip("감지할 레이어")]
    [SerializeField] LayerMask layerMask;
    [Tooltip("거리 변경 비율")]
    [SerializeField][Range(0f, 1f)] float changeRatio;



    private void Start()
    {
        originZ = transform.localPosition.z;
        cameraZposition = transform.localPosition.z;
    }

    private void FixedUpdate()
    {
        Ray ray = new(transform.position, player.transform.position - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(transform.position, player.transform.position), layerMask)
    || Physics.OverlapSphereNonAlloc(transform.position, CameraCollisionDetectRadiusForDecrease, tempColliderArray, layerMask) != 0)
        {
            cameraZposition = Mathf.Lerp(cameraZposition, 0, changeRatio);
        }
        if (!Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, player.transform.position), layerMask)
        && Physics.OverlapSphereNonAlloc(transform.position, CameraCollisionDetectRadiusForIncrease, tempColliderArray, layerMask) == 0)
        {
            cameraZposition = Mathf.Lerp(cameraZposition, originZ, changeRatio);
        }

        transform.localPosition = new Vector3(0, 2, cameraZposition);
    }
}
