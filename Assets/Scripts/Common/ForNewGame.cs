using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForNewGame : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.saveClass.InitSaveClass(DefaultStatManager.Instance.GetPlayerData());
        SoundManager.Instance.BGMChange(0);
    }
}
