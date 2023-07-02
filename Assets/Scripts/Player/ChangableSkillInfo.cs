using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableSkillInfo : MonoBehaviour
{
    [Tooltip("스킬 이미지 배열")]
    public Sprite[] sImage = new Sprite[4];
    [Tooltip("효과 prefab 배열")]
    public GameObject[] effectPrefabs = new GameObject[4];
    [Tooltip("효과 오브젝트 위치값")]
    public Vector3 effectPos = new Vector3();
    [Tooltip("효과 오브젝트 회전값")]
    public Quaternion effectRot = new Quaternion();
}
