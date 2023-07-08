using System.Collections;
using UnityEngine;

public class BossRoomGratingControll : MonoBehaviour
{
    Enemy enemy;
    Boss boss;
    private Vector3 originPosition;

    private void Awake()
    {
        enemy = FindObjectOfType<BossInfo>().stat;
        boss = enemy as Boss;
    }

    void Start()
    { 
        originPosition = transform.position;
        int temp = SaveManager.Instance.saveClass.GetLastSavePosition();
        if (temp == 8 || temp == 9)
        {
            float tempY = originPosition.y - 6;
            transform.SetLocalPositionAndRotation(new Vector3(transform.position.x, tempY, transform.position.z), transform.localRotation);
        }
        StartCoroutine(CheckBossDead());
    }

    IEnumerator CheckBossDead()
    {
        while(true)
        {
            if (boss.GetIsDead())
            {
                StartCoroutine(OpenTheDoor());
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator OpenTheDoor()
    {
        while(true)
        {
            if (transform.position.y >= originPosition.y)
            {
                break;
            }
            transform.SetLocalPositionAndRotation(Vector3.Lerp(transform.position, originPosition, 0.1f), transform.localRotation);
            yield return null;
        }
    }
}
