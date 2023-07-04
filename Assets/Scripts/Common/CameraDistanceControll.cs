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
        // 초기 세팅
        originZ = transform.localPosition.z;
        cameraZposition = transform.localPosition.z;
    }

    private void FixedUpdate()
    {
        // Ray 세팅
        Ray ray = new(transform.position, player.transform.position - transform.position);

        // 카메라에서 플레이어에게 쏜 Ray가 레이어 오브젝트에 닿거나, 카메라 OverlapSphere 범위에 레이어 오브젝트가 충돌했다면
        if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(transform.position, player.transform.position), layerMask)
        || Physics.OverlapSphereNonAlloc(transform.position, CameraCollisionDetectRadiusForDecrease, tempColliderArray, layerMask) != 0)
        {
            // 카메라 거리 비율만큼 감소
            cameraZposition = Mathf.Lerp(cameraZposition, 0, changeRatio);
        }
        // 카메라에서 플레이어에게 쏜 Ray가 레이어 오브젝트에 닿지 않고, 카메라 OverlapShpere 범위에 레이어 오브젝트가 충돌하지 않았다면
        if (!Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, player.transform.position), layerMask)
        && Physics.OverlapSphereNonAlloc(transform.position, CameraCollisionDetectRadiusForIncrease, tempColliderArray, layerMask) == 0)
        {
            // 카메라 거리 비율만큼 증가
            cameraZposition = Mathf.Lerp(cameraZposition, originZ, changeRatio);
        }
        // 변경된 카메라 거리 적용
        transform.localPosition = new Vector3(0, 2, cameraZposition);
    }
}
