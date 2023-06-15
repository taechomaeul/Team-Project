using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    public bool isClear = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Player의 위치를 리셋한다.
    /// 기존의 플레이어의 오브젝트를 삭제하고, 새롭게 Instantiate한다.
    /// </summary>
    public void ResetPlayer()
    {
        Destroy(player);
        GameObject newPlayer = Instantiate(playerPrefab);

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject item in boxes)
        {
            item.GetComponent<BoxController>().pPlayerController = newPlayer.GetComponent<PushPlayerController>();
            //Box에 있는 playerController 상태(plState) 연결을 위해 newPlayer의 playerController를 연결시켜준다.
        }
        
        player = newPlayer; //새로 생성된 플레이어를 연결
        player.name = "Player";
    }
}
