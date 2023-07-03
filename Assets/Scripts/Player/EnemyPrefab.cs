using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    [Header("빙의 가능한 적 외형의 플레이어 Prefab 정보 / 순서 중요!")]
    [Tooltip("EnemyPrefabs: 미리 등록해야하는 Prefab")]
    public GameObject[] enemyPrefabs;

    [Tooltip("enemyImages: 미리 등록해야하는 Image / 순서 중요!")]
    public Sprite[] enemyImages;

    [Tooltip("enemyPrefabNames: 미리 등록해야하는 Name / 순서 중요!")]
    public string[] enemyPrefabNames;
}
