using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushClear : MonoBehaviour
{
    public PushManager pushManager;
    private void Start()
    {
        pushManager = GameObject.Find("PushManager").GetComponent<PushManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            pushManager.isClear = true;
            Debug.Log("Clear True!");
        }
    }
}
