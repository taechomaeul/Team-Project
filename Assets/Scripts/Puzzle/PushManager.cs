using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    public Vector3 originPos;
    public bool isClear = false;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    /// <summary>
    /// Player의 위치를 리셋한다.
    /// 기존의 플레이어의 오브젝트를 삭제하고, 새롭게 Instantiate한다.
    /// </summary>
    public void ResetPlayer()
    {
        Destroy(player);
        GameObject newPlayer = Instantiate(playerPrefab);

        newPlayer.GetComponentInChildren<CharacterController>().enabled = false;
        newPlayer.transform.localPosition = originPos;
        newPlayer.transform.GetChild(0).localPosition = Vector3.zero;
        newPlayer.GetComponentInChildren<CharacterController>().enabled = true;

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject item in boxes)
        {
            item.GetComponent<BoxController>().pPlayerController = newPlayer.GetComponent<PushPlayerController>();
            //Box에 있는 playerController 상태(plState) 연결을 위해 newPlayer의 playerController를 연결시켜준다.
        }
        
        player = newPlayer; //새로 생성된 플레이어를 연결
        player.name = "Player";
    }

    public void ResetBox()
    {
        GameObject[] movingBoxes = GameObject.FindGameObjectsWithTag("Box");
        for (int i = 0; i < movingBoxes.Length; i++)
        {
            movingBoxes[i].transform.position = movingBoxes[i].GetComponent<BoxController>().originPos;
        }
    }
}
