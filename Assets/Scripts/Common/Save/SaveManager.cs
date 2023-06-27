using System;
using System.Collections;
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

    // 플레이어 정보
    private PlayerInfo playerInfo;

    // 세이브 경로
    private readonly string path = "Save";

    // 저장 데이터 클래스
    [SerializeField] SaveClass saveClass = new();

    // 세이브 관련 클래스
    [Serializable]
    private class SaveClass
    {
        // 저장 위치
        [SerializeField] private int lastSavePosition;
        // 현재 체력
        [SerializeField] private int currentHp;
        // 현재 영혼
        [SerializeField] private int currentSoulCount;
        // 현재 빙의체
        [SerializeField] private int currentBodyIndex;
        // 현재 가지고 있는 스킬
        [SerializeField] private int currentSkillIndex;
        // 현재 공격력?
        [SerializeField] private int currentAttack;
        // 현재 속도?
        [SerializeField] private float currentSpeed;
        // 스크립트 체크
        [SerializeField] private bool[] scriptData;
        // 팁 체크
        [SerializeField] private bool[] tipData;
        // 일지 체크
        [SerializeField] private bool[] recordData;



        #region Set 함수들
        /// <summary>
        /// 현재 저장 위치 설정
        /// </summary>
        /// <param name="savePositionIndex">저장 위치</param>
        internal void SetLastSavePosition(int savePositionIndex)
        {
            lastSavePosition = savePositionIndex;
        }

        /// <summary>
        /// 현재 체력 설정
        /// </summary>
        /// <param name="hp">현재 체력</param>
        internal void SetCurrentHp(int hp)
        {
            currentHp = hp;
        }

        /// <summary>
        /// 현재 영혼 설정
        /// </summary>
        /// <param name="soulCount">현재 영혼</param>
        internal void SetCurrentSoulCount(int soulCount)
        {
            currentSoulCount = soulCount;
        }

        /// <summary>
        /// 현재 빙의체 인덱스 설정
        /// </summary>
        /// <param name="bodyIndex">현재 빙의체 인덱스</param>
        internal void SetCurrentBodyIndex(int bodyIndex)
        {
            currentBodyIndex = bodyIndex;
        }

        /// <summary>
        /// 현재 스킬 설정
        /// </summary>
        /// <param name="skillIndex">현재 스킬 인덱스</param>
        internal void SetCurrentSkillIndex(int skillIndex)
        {
            currentSkillIndex = skillIndex;
        }

        /// <summary>
        /// 현재 공격력 설정
        /// </summary>
        /// <param name="attack">현재 공격력</param>
        internal void SetCurrentAttack(int attack)
        {
            currentAttack = attack;
        }

        /// <summary>
        /// 현재 속도 설정
        /// </summary>
        /// <param name="speed">현재 속도</param>
        internal void SetCurrentSpeed(float speed)
        {
            currentSpeed = speed;
        }

        /// <summary>
        /// 스크립트 체크 설정
        /// </summary>
        /// <param name="index">값을 변경할 스크립트의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetScriptData(int index, bool tf)
        {
            if (index < scriptData.Length)
            {
                scriptData[index] = tf;
            }
            else
            {
                Debug.Log("잘못된 인덱스 접근");
            }
        }

        /// <summary>
        /// 팁 체크 설정
        /// </summary>
        /// <param name="index">값을 변경할 팁의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetTipData(int index, bool tf)
        {
            if (index < tipData.Length)
            {
                tipData[index] = tf;
            }
            else
            {
                Debug.Log("잘못된 인덱스 접근");
            }
        }

        /// <summary>
        /// 일지 체크 설정
        /// </summary>
        /// <param name="index">값을 변경할 일지의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetRecordData(int index, bool tf)
        {
            if (index < recordData.Length)
            {
                recordData[index] = tf;
            }
            else
            {
                Debug.Log("잘못된 인덱스 접근");
            }
        }
        #endregion

        /// <summary>
        /// 마지막 저장 지점 확인
        /// </summary>
        /// <returns>마지막 저장 지점 인덱스</returns>
        internal int GetLastSavePosition()
        {
            return lastSavePosition;
        }

        /// <summary>
        /// 현재 체력 확인
        /// </summary>
        /// <returns>현재 체력</returns>
        internal int GetCurrentHp()
        {
            return currentHp;
        }

        /// <summary>
        /// 현재 영혼 확인
        /// </summary>
        /// <returns>현재 영혼</returns>
        internal int GetCurrentSoulCount()
        {
            return currentSoulCount;
        }
    }



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

        if (playerInfo == null)
        {
            playerInfo = FindObjectOfType<PlayerInfo>();
        }
        // 첫 시작 위치 저장
        saveClass.SetLastSavePosition(0);
    }

    /// <summary>
    /// 클래스에 저장할 데이터 초기화
    /// </summary>
    /// <param name="lastSavePosition">저장 위치</param>
    public void SetSaveClass(int lastSavePosition)
    {
        saveClass.SetLastSavePosition(lastSavePosition);
        saveClass.SetCurrentHp(playerInfo.curHp);
        saveClass.SetCurrentBodyIndex(playerInfo.curPrefabIndex);
        saveClass.SetCurrentSoulCount(playerInfo.soulHp);
        saveClass.SetCurrentAttack(playerInfo.plAtk);
        saveClass.SetCurrentSpeed(playerInfo.plMoveSpd);
    }

    /// <summary>
    /// 현재 데이터 저장
    /// </summary>
    /// <param name="lastSavePosition">저장 위치</param>
    internal void SaveCurrentData(int lastSavePosition)
    {
        if (playerInfo == null)
        {
            playerInfo = FindObjectOfType<PlayerInfo>();
        }

        // 세이브 데이터 Json으로 변환
        SetSaveClass(lastSavePosition);
        string saveJson = JsonUtility.ToJson(saveClass);

        // 세이브 폴더 생성
        DirectoryInfo di = new(path);
        if (!di.Exists)
        {
            di.Create();
        }

        // 세이브 파일 생성
        FileStream fs = new($"{path}/saveData.json", FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(saveJson);
        fs.Write(data, 0, data.Length);
        fs.Close();
        Debug.Log("세이브 파일 생성됨");
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
            saveClass = JsonUtility.FromJson<SaveClass>(saveJson);
            Debug.Log("세이브 파일 불러옴");
            Debug.Log(saveJson);
        }
        // 세이브 파일이 존재하지 않는 경우
        else
        {
            // 추가 필요
            Debug.Log("세이브 데이터가 존재하지 않음");
        }
    }

    public IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        LoadSaveData();
    }
}