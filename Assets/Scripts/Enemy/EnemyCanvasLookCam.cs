using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasLookCam : MonoBehaviour
{
    [SerializeField] private EnemyInfo enemyInfo;
    public GameObject Cam;

    private void Awake()
    {
        enemyInfo = transform.parent.parent.GetComponent<EnemyInfo>();
    }

    private void OnEnable()
    {
        Cam = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        if (enemyInfo.stat.GetIsDead())
        {
            transform.parent.gameObject.SetActive(false);
        }
        transform.LookAt(Cam.transform);
    }
}