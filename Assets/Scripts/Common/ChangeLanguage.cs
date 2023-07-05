using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLanguage : MonoBehaviour
{
    [System.Serializable]
    public class LangCData
    {
        public int index;
        public string panelName;
        public string krName;
        public string enName;
    }

    [Header("스크립트 파일 경로")]
    public string langPath;
    private List<Dictionary<string, object>> lang;
    [Header("저장된 언어 데이터 리스트")]
    [SerializeField]
    public List<LangCData> langCDataList;

    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals("TitleScene"))
        {
            LoadTitleLangCDataFromCSV();
        }

        else if(scene.name.Contains("1F") || scene.name.Contains("2F"))
        {
            LoadFloorLangCDataFromCSV();
        }

    }

    /// <summary>
    /// CSV 파일을 읽고 내용을 클래스에 저장하는 함수 (1F, 2F 전용)
    /// </summary>
    /// <returns></returns>
    public void LoadFloorLangCDataFromCSV()
    {
        lang = CSVReader.Read(langPath);

        langCDataList = new List<LangCData>();

        for (int i = 0; i < lang.Count; i++)
        {
            if (!lang[i]["Scenes"].Equals("TitleScene")) //타이틀씬이 아니라면 1F/2F
            {
                LangCData newLangCData = new LangCData();
                newLangCData.index = langCDataList.Count + 1;
                newLangCData.panelName = lang[i]["Object_Name"].ToString();
                newLangCData.krName = lang[i]["KR"].ToString();
                newLangCData.enName = lang[i]["EN"].ToString();
                langCDataList.Add(newLangCData);
            }
        }
    }

    /// <summary>
    /// CSV 파일을 읽고 내용을 클래스에 저장하는 함수 (Title 전용)
    /// </summary>
    /// <returns></returns>
    public void LoadTitleLangCDataFromCSV()
    {
        lang = CSVReader.Read(langPath);

        langCDataList = new List<LangCData>();

        for (int i = 0; i < lang.Count; i++)
        {
            if (lang[i]["Language"].Equals("TitleScene")) //타이틀씬일 때
            {
                LangCData newLangCData = new LangCData();
                newLangCData.index = langCDataList.Count;
                newLangCData.panelName = lang[i]["Object_Name"].ToString();
                newLangCData.krName = lang[i]["KR"].ToString();
                newLangCData.enName = lang[i]["EN"].ToString();
                langCDataList.Add(newLangCData);
            }
        }
    }

    public int FindPanelNameOfIndex(string panelName)
    {
        int index = 0;
        for (int i=0; i< langCDataList.Count; i++)
        {
            if (langCDataList[i].panelName.Equals(panelName))
            {
                index = langCDataList[i].index;
            }
        }

        return index;
    }

}
