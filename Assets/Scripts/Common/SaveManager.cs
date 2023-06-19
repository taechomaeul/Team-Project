using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 싱글톤
    static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // 세이브 정보들
    [Serializable]
    private class SaveClass
    {
        // 저장 위치
        [SerializeField] int savePosition;
        // 현재 체력
        [SerializeField] int currentHp;
        // 현재 빙의체
        [SerializeField] int currentBodyIndex;
        // 현재 가지고 있는 스킬
        [SerializeField] int currentSkillIndex;

        /// <summary>
        /// 클래스에 저장할 데이터 초기화
        /// </summary>
        /// <param name="sp">저장 위치</param>
        /// <param name="ch">현재 체력</param>
        /// <param name="cbi">현재 빙의체</param>
        /// <param name="csi">현재 가지고 있는 스킬</param>
        public void SetSaveClass(int sp, int ch, int cbi, int csi)
        {
            savePosition = sp;
            currentHp = ch;
            currentBodyIndex = cbi;
            currentSkillIndex = csi;
        }

        /// <summary>
        /// 저장 데이터 로드
        /// </summary>
        public void DataApply()
        {
            // 데이터를 어떻게 적용시킬지 생각해야함
            Debug.Log(savePosition);
            Debug.Log(currentHp);
            Debug.Log(currentBodyIndex);
            Debug.Log(currentSkillIndex);
        }
    }

    // 세이브 경로
    string path = "Save";


    private void Awake()
    {
        // 싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// 현재 데이터 저장
    /// </summary>
    /// <param name="sp">저장 위치</param>
    /// <param name="ch">현재 체력</param>
    /// <param name="cbi">현재 빙의체</param>
    /// <param name="csi">현재 가지고 있는 스킬</param>
    public void SaveCurrentData(int sp, int ch, int cbi, int csi)
    {
        SaveClass saveClass = new();
        saveClass.SetSaveClass(sp, ch, cbi, csi);
        string saveJson = JsonUtility.ToJson(saveClass);
        DirectoryInfo di = new(path);
        if (!di.Exists)
        {
            di.Create();
        }
        FileStream fs = new($"{path}/saveData.json", FileMode.CreateNew);
        byte[] data = Encoding.UTF8.GetBytes(saveJson);
        fs.Write(data, 0, data.Length);
        fs.Close();
    }

    public void LoadSaveData()
    {
        FileInfo fi = new($"{path}/saveData.json");
        if (fi.Exists)
        {
        FileStream fs = new($"{path}/saveData.json", FileMode.Open);
            byte[] data = new byte[fi.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            string saveJson = Encoding.UTF8.GetString(data);
            JsonUtility.FromJson<SaveClass>(saveJson).DataApply();
        }
        else
        {
            Debug.Log("세이브 데이터가 존재하지 않음");
        }
    }
}
