using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InstatiateFinalBoss : MonoBehaviour
{
    public GameObject mainBoss;
    public Vector3 prefabPos;

    void Start()
    {
        List<string> sceneDta = SaveManager.Instance.saveClass.GetMoveSceneData();
        int sceneDataCnt = sceneDta.Count;
        for (int i = 0; i < sceneDataCnt; i++)
        {
            if (sceneDta[i].Contains("BossScene"))
            {
                /*GameObject mainBoss = Instantiate(bossPrefab);
                mainBoss.GetComponent<NavMeshAgent>().enabled = false;
                mainBoss.transform.localPosition = prefabPos;
                mainBoss.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                mainBoss.GetComponent<NavMeshAgent>().enabled = true;
                mainBoss.name = "FinalBoss_Minotaur";*/

                mainBoss.SetActive(true);

            }
        }
    }
}
