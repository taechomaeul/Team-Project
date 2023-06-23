using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 싱글톤 세팅
    private static SaveManager instance;
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

    // 세이브 관련 클래스
    [Serializable]
    private class SaveClass
    {
        // 저장 위치
        private int savePosition;
        // 현재 체력
         private int currentHp;
        // 현재 영혼 수
        private int currentSoulCount;
        // 현재 빙의체
        private int currentBodyIndex;
        // 현재 가지고 있는 스킬
        private int currentSkillIndex;

        /// <summary>
        /// 클래스에 저장할 데이터 초기화
        /// </summary>
        /// <param name="sp">Save Position</param>
        /// <param name="ch">Current Hp</param>
        /// <param name="csc">Current Soul Count</param>
        /// <param name="cbi">Current Body Index</param>
        /// <param name="csi">Current Skill Index</param>
        public void SetSaveClass(int sp, int ch, int csc, int cbi, int csi)
        {
            savePosition = sp;
            currentHp = ch;
            currentSoulCount = csc;
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
            Debug.Log(currentSoulCount);
            Debug.Log(currentBodyIndex);
            Debug.Log(currentSkillIndex);
        }
    }

    // 세이브 경로
    private readonly string path = "Save";



    private void Awake()
    {
        // 싱글톤 세팅
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
    /// <param name="sp">Save Position</param>
    /// <param name="ch">Current Hp</param>
    /// <param name="csc">Current Soul Count</param>
    /// <param name="cbi">Current Body Index</param>
    /// <param name="csi">Current Skill Index</param>
    public void SaveCurrentData(int sp, int ch, int csc, int cbi, int csi)
    {
        // 저장 데이터 Json 형태로 변환
        SaveClass saveClass = new();
        saveClass.SetSaveClass(sp, ch, csc, cbi, csi);
        string saveJson = JsonUtility.ToJson(saveClass);

        // 세이브 폴더 생성
        DirectoryInfo di = new(path);
        if (!di.Exists)
        {
            di.Create();
        }

        // 세이브 파일 생성
        FileStream fs = new($"{path}/saveData.json", FileMode.CreateNew);
        byte[] data = Encoding.UTF8.GetBytes(saveJson);
        fs.Write(data, 0, data.Length);
        fs.Close();
    }

    /// <summary>
    /// 세이브 데이터 불러오기
    /// </summary>
    public void LoadSaveData()
    {
        // 세이브 경로에서 파일 정보 가져오기
        FileInfo fi = new($"{path}/saveData.json");

        // 세이브 파일이 존재할 경우
        if (fi.Exists)
        {
            // 파일 데이터 불러오기
            FileStream fs = new($"{path}/saveData.json", FileMode.Open);
            byte[] data = new byte[fi.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Json 형태의 정보 변환
            string saveJson = Encoding.UTF8.GetString(data);
            JsonUtility.FromJson<SaveClass>(saveJson).DataApply();
        }
        // 세이브 파일이 존재하지 않는 경우
        else
        {
            // 추가 필요
            Debug.Log("세이브 데이터가 존재하지 않음");
        }
    }
}
