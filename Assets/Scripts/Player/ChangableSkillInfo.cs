using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableSkillInfo : MonoBehaviour
{
    public Sprite[] sImage = new Sprite[4];
    public GameObject[] effectPrefabs = new GameObject[4];
    public Vector3 effectPos = new Vector3();
    public Quaternion effectRot = new Quaternion();
}
