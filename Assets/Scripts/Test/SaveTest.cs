using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.SaveCurrentData(1, 2, 5, 3, 4);
        //SaveManager.Instance.LoadSaveData();
    }
}
