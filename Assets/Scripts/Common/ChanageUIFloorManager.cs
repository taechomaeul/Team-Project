using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanageUIFloorManager : MonoBehaviour
{

    [Header("메인 패널 부분 변경 / F키 변경")]
    public GameObject[] fPanels; //FCommonPanel_Search (처럼 묶여져 있는 패널을 직접 연결해야함)

    [Header("일지 패널 부분 변경 / 버튼이름 및 내용 변경")]
    public GameObject[] jPanels;

    private ChangeLanguage changeLanguage;

    void Start()
    {
        changeLanguage = GameObject.Find("UIManager").GetComponent<ChangeLanguage>();
        ChangeAllPanel();
    }

    public void ChangeAllPanel()
    {
        ChangeMainPanel();
        ChangeJournalPanel();
    }

    public void ChangeMainPanel() // F패널용
    {

        for (int i = 0; i < fPanels.Length; i++) //저장된 데이터만큼
        {
            int index = changeLanguage.FindPanelNameOfIndex(fPanels[i].name);
            for (int j = 0; j < fPanels[i].transform.childCount; j++) //3
            {
                string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();
                if (lang.Equals("KR"))
                {
                    fPanels[i].transform.GetChild(j).GetComponent<Text>().text = changeLanguage.langCDataList[index].krName;
                }
                else if (lang.Equals("EN"))
                {
                    fPanels[i].transform.GetChild(j).GetComponent<Text>().text = changeLanguage.langCDataList[index].enName;
                }
            }

        }
    }

    public void ChangeJournalPanel() // 일지용
    {
        for (int i = 0; i < jPanels.Length; i++) //저장된 데이터만큼
        {
            int index = changeLanguage.FindPanelNameOfIndex(jPanels[i].name);
            string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();
            if (lang.Equals("KR"))
            {
                jPanels[i].GetComponent<Text>().text = changeLanguage.langCDataList[index].krName;
            }
            else if (lang.Equals("EN"))
            {
                jPanels[i].GetComponent<Text>().text = changeLanguage.langCDataList[index].enName;
            }
        }
    }
}
