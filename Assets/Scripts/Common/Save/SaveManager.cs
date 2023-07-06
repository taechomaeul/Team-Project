using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // 플레이어 기본 스탯
    private Dictionary<string, object> playerStat;

    // 세이브 경로
    private readonly string path = "Save";

    // 저장 데이터 클래스
    [SerializeField] internal SaveClass saveClass = new();

    // 스크립트와 일지 등등 관리
    ShowScript ss;
    ShowRecord sr;
    ShowTip st;
    ColliderController cc;

    // 세이브 관련 클래스
    [Serializable]
    internal class SaveClass
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
        // 이동 Collider 체크
        [SerializeField] private List<string> moveSceneData = new();




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
        /// 세이브 클래스의 스크립트 체크 인덱스별 설정
        /// </summary>
        /// <param name="index">값을 변경할 스크립트의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetScriptData(int index, bool tf)
        {
            if (scriptData != null)
            {
                if (index < scriptData.Length)
                {
                    scriptData[index] = tf;
                }
                else
                {
                    Debug.LogError("잘못된 인덱스 접근");
                }
            }
            else
            {
                Debug.LogError("scriptData는 null");
            }
        }

        /// <summary>
        /// 스크립트 체크 일괄 설정
        /// </summary>
        /// <param name="scriptDataCheckArray">저장할 스크립트 체크 배열</param>
        internal void SetScriptData(bool[] scriptDataCheckArray)
        {
            scriptData = scriptDataCheckArray;
        }

        /// <summary>
        /// 세이브 클래스의 팁 체크 인덱스별 설정
        /// </summary>
        /// <param name="index">값을 변경할 팁의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetTipData(int index, bool tf)
        {
            if (tipData != null)
            {
                if (index < tipData.Length)
                {
                    tipData[index] = tf;
                }
                else
                {
                    Debug.LogError("잘못된 인덱스 접근");
                }
            }
            else
            {
                Debug.LogError("tipData는 null");
            }
        }

        /// <summary>
        /// 팁 체크 일괄 설정
        /// </summary>
        /// <param name="tipDataCheckArray">저장할 팁 체크 배열</param>
        internal void SetTipData(bool[] tipDataCheckArray)
        {
            tipData = tipDataCheckArray;
        }

        /// <summary>
        /// 세이브 클래스의 일지 체크 인덱스별 설정
        /// </summary>
        /// <param name="index">값을 변경할 일지의 인덱스</param>
        /// <param name="tf">변경할 값(true or false)</param>
        internal void SetRecordData(int index, bool tf)
        {
            if (recordData != null)
            {
                if (index < recordData.Length)
                {
                    recordData[index] = tf;
                }
                else
                {
                    Debug.LogError("잘못된 인덱스 접근");
                }
            }
            else
            {
                Debug.LogError("recordData는 null");
            }
        }

        /// <summary>
        /// 일지 체크 일괄 설정
        /// </summary>
        /// <param name="recordDataCheckArray">저장할 일지 체크 배열</param>
        internal void SetRecordData(bool[] recordDataCheckArray)
        {
            recordData = recordDataCheckArray;
        }

        /// <summary>
        /// 이동 씬 이름 체크 일괄 설정
        /// </summary>
        /// <param name="moveSceneDataCheckList">저장할 씬이동 Collider 이름 List</param>
        internal void SetMoveSceneData(List<string> moveSceneDataCheckList)
        {
            moveSceneData = moveSceneDataCheckList;
        }
        #endregion


        #region Get 함수들
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

        /// <summary>
        /// 현재 빙의체 인덱스 확인
        /// </summary>
        /// <returns>현재 빙의체 인덱스</returns>
        internal int GetCurrentBodyIndex()
        {
            return currentBodyIndex;
        }

        /// <summary>
        /// 현재 스킬 인덱스 확인
        /// </summary>
        /// <returns>현재 스킬 인덱스</returns>
        internal int GetCurrentSkillIndex()
        {
            return currentSkillIndex;
        }

        /// <summary>
        /// 현재 공격력 확인
        /// </summary>
        /// <returns>현재 공격력</returns>
        internal int GetCurrentAttack()
        {
            return currentAttack;
        }

        /// <summary>
        /// 현재 이동 속도 확인
        /// </summary>
        /// <returns>현재 이동 속도</returns>
        internal float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        /// <summary>
        /// 스크립트 체크 확인
        /// </summary>
        /// <returns>스크립트 체크 배열</returns>
        internal bool[] GetScriptData()
        {
            return scriptData;
        }

        /// <summary>
        /// 팁 체크 확인
        /// </summary>
        /// <returns>팁 체크 배열</returns>
        internal bool[] GetTipData()
        {
            return tipData;
        }

        /// <summary>
        /// 일지 체크 확인
        /// </summary>
        /// <returns>일지 체크 배열</returns>
        internal bool[] GetRecordData()
        {
            return recordData;
        }

        /// <summary>
        /// 이동씬 이름 리스트 확인
        /// </summary>
        /// <returns>이동씬 이름 리스트</returns>
        internal List<string> GetMoveSceneData()
        {
            return moveSceneData;
        }
        #endregion


        #region 기타 함수들
        /// <summary>
        /// 새로 시작 시 초기화
        /// </summary>
        internal void InitSaveClass(int currentHp, int currentAttack, float currentSpeed)
        {
            SetLastSavePosition(0);
            // 임시
            this.currentHp = currentHp;
            currentSoulCount = 0;
            currentBodyIndex = 0;
            currentSkillIndex = -1;
            // 임시
            this.currentAttack = currentAttack;
            // 임시
            this.currentSpeed = currentSpeed;
            Array.Fill(scriptData, false);
            Array.Fill(tipData, false);
            Array.Fill(recordData, false);
            moveSceneData.Clear();
        }

        /// <summary>
        /// 이동 씬 이름 체크 항목 추가
        /// </summary>
        /// <param name="colliderObjectName">저장할 씬이동 Collider 이름</param>
        internal void AddMoveCollider(string colliderObjectName)
        {
            moveSceneData.Add(colliderObjectName);
        }
        #endregion
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
        playerStat = DefaultStatManager.Instance.GetPlayerData();
        saveClass.InitSaveClass((int)playerStat["maxHp"], (int)playerStat["attack"], (float)playerStat["movingSpeed"]);

    }

    // 새로운 씬에서 찾기
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 플레이어 정보, 스크립트, 일지 등등 관리 변수 초기화
        if (playerInfo == null)
        {
            playerInfo = FindObjectOfType<PlayerInfo>();
        }
        if (cc == null)
        {
            cc = FindObjectOfType<ColliderController>();
        }
        if (ss == null)
        {
            ss = FindObjectOfType<ShowScript>();
        }
        if (st == null)
        {
            st = FindObjectOfType<ShowTip>();
        }
        if (sr == null)
        {
            sr = FindObjectOfType<ShowRecord>();
        }
    }

    /// <summary>
    /// 세이브 클래스에 저장할 데이터 초기화
    /// </summary>
    /// <param name="lastSavePosition">저장 위치 인덱스</param>
    public void SaveCurrentDataToClass(int lastSavePosition)
    {
        saveClass.SetLastSavePosition(lastSavePosition);
        saveClass.SetCurrentHp(playerInfo.curHp);
        saveClass.SetCurrentSoulCount(playerInfo.soulHp);
        saveClass.SetCurrentBodyIndex(playerInfo.curPrefabIndex);
        saveClass.SetCurrentSkillIndex(playerInfo.curSkill.skillIndex);
        saveClass.SetCurrentAttack(playerInfo.plAtk);
        saveClass.SetCurrentSpeed(playerInfo.plMoveSpd);
        saveClass.SetScriptData(ss.checkScriptComplete);
        saveClass.SetTipData(st.checkTipComplete);
        saveClass.SetRecordData(sr.checkRecordComplete);
    }

    /// <summary>
    /// 현재 데이터 저장
    /// </summary>
    /// <param name="lastSavePosition">저장 위치 인덱스</param>
    /// <returns>작업 결과</returns>
    internal bool SaveCurrentDataToFile(int lastSavePosition)
    {
        try
        {
            if (playerInfo == null)
            {
                playerInfo = FindObjectOfType<PlayerInfo>();
            }

            // 세이브 데이터 Json으로 변환
            SaveCurrentDataToClass(lastSavePosition);
            string saveJson = JsonUtility.ToJson(saveClass);
            Debug.Log(saveJson);
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
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    /// <summary>
    /// 데이터 수치를 게임에 적용
    /// </summary>
    public void ApplyLoadedData()
    {
        if (playerInfo != null)
        {
            playerInfo.curPositionIndex = saveClass.GetLastSavePosition();
            playerInfo.curHp = saveClass.GetCurrentHp();
            playerInfo.soulHp = saveClass.GetCurrentSoulCount();
            playerInfo.curPrefabIndex = saveClass.GetCurrentBodyIndex();
            playerInfo.curSkill.skillIndex = saveClass.GetCurrentSkillIndex();
            playerInfo.plAtk = saveClass.GetCurrentAttack();
            playerInfo.plMoveSpd = saveClass.GetCurrentSpeed();
        }
        else
        {
            Debug.Log("player is not exist");
        }

        if (cc != null)
        {
            ss.GetCheckScriptArr(saveClass.GetScriptData());
            sr.GetCheckRecordArr(saveClass.GetRecordData());
        }
        else
        {
            Debug.Log("cc is not exist");
        }

        cc.OffRecordCollider(saveClass.GetRecordData());
        cc.OffScriptCollider(saveClass.GetScriptData());
        cc.OffMoveSceneCollider(saveClass.GetMoveSceneData());
    }

    /// <summary>
    /// 세이브 데이터 불러오기
    /// </summary>
    /// <returns>작업 결과</returns>
    public bool LoadSaveData()
    {
        try
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
                return true;
            }
            // 세이브 파일이 존재하지 않는 경우
            else
            {
                // 추가 필요
                Debug.Log("세이브 데이터가 존재하지 않음");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    /// <summary>
    /// 세이브 파일이 존재하는지 확인
    /// </summary>
    /// <returns>존재: true, 없음 또는 오류: false</returns>
    public bool SaveFileExistCheck()
    {
        try
        {
            // 세이브 경로에서 파일 정보 가져오기
            FileInfo fi = new($"{path}/saveData.json");
            // 세이브 파일이 존재할 경우
            if (fi.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }
}