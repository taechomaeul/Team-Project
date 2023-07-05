using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanageUITitleManager : MonoBehaviour
{
    [Header("일지 패널 부분 변경 / 버튼이름 및 내용 변경")]
    public GameObject[] jPanels;

    private ChangeLanguage changeLanguage;

    void Start()
    {
        changeLanguage = GameObject.Find("ActionFunction").GetComponent<ChangeLanguage>();
    }

    public void ChangeAllPanel()
    {
        ChangeJournalPanel();
    }

    public void ChangeJournalPanel() // 일지용
    {
        for (int i = 0; i < jPanels.Length; i++) //저장된 데이터만큼
        {
            int index = changeLanguage.FindPanelNameOfIndex(jPanels[i].name);
            Debug.Log($"Index : {index}");

            string lang = SettingManager.Instance.GetCurrentLanguageIndexToString();
            if (lang.Equals("KR"))
            {
                jPanels[i].GetComponent<Text>().text = changeLanguage.langCDataList[index].krName;
                Debug.Log($"changeLanguage.langCDataList[{index}].krName : {changeLanguage.langCDataList[index].krName}");
            }
            else if (lang.Equals("EN"))
            {
                jPanels[i].GetComponent<Text>().text = changeLanguage.langCDataList[index].enName;
                Debug.Log($"changeLanguage.langCDataList[{index}].enName : {changeLanguage.langCDataList[index].enName}");
            }
        }

    }
}
