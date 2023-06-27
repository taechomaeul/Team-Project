using UnityEngine;

public class SaveTest : MonoBehaviour
{
    private void Start()
    {
        //SaveManager.Instance.SaveCurrentData(1);
        //SaveManager.Instance.LoadSaveData();
        Debug.Log(SaveManager.Instance.saveClass.GetCurrentAttack());
        Debug.Log(SaveManager.Instance.saveClass.GetCurrentHp());
    }
}
