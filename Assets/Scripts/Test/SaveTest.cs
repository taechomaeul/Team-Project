using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.SaveCurrentData(1);
        StartCoroutine(SaveManager.Instance.Test());
        //SaveManager.Instance.LoadSaveData();
    }
}
