using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    [Header("빙의 가능한 적 외형의 플레이어 Prefab")]
    [Tooltip("EnemyPrefabs: 미리 등록해야하는 Prefab")]
    public GameObject[] enemyPrefabs;

    [Header("시야 Prefab")]
    [Tooltip("Camera포함 시야 Prefab")]
    public GameObject sightPrefab;
}
