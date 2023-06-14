using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ResetPlayer()
    {
        Destroy(player);
        GameObject newPlayer = Instantiate(playerPrefab);
        player = newPlayer;
        player.name = "Player";
    }
}
