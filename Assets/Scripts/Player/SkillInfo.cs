using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class Skill
{
    [Tooltip("스킬 인덱스")]
    public int skillIndex;
    [Tooltip("스킬 이름")]
    public string skillName;
    [Tooltip("스킬 설명")]
    public string skillDescription;
    [Tooltip("스킬 이미지")]
    public Sprite thumnail;
    [Tooltip("스킬 쿨타임")]
    public float coolTime;
    [Tooltip("스킬 지속시간")]
    public float duringTime;
    [Tooltip("스킬 이펙트 Prefab")]
    public GameObject effectPrefab;
    [Tooltip("스킬 이펙트 위치 값")]
    public Vector3 effectPos;
    [Tooltip("스킬 이펙트 회전 값")]
    public Quaternion effectRot;
}

public class SkillInfo : MonoBehaviour
{
    public Skill[] skills;
    public ChangableSkillInfo cSkillInfo;

    public string dataPath;
    public List<Dictionary<string, object>> data;

    private string[] langArr; //언어 이름만을 모은 배열

    //읽어온 csv파일 데이터 저장
    private void Awake()
    {
        data = CSVReader.Read(dataPath);

        //배열 초기화
        langArr = new string[data.Count];

        //언어 배열 생성
        for (int i = 0; i < data.Count; i++)
        {
            langArr[i] = data[i]["Language"].ToString();
        }

        //중복제거
        langArr = langArr.Distinct().ToArray();

        skills = new Skill[data.Count / langArr.Length];
        cSkillInfo = GetComponent<ChangableSkillInfo>();

        int index = 0;
        for (int i=0; i < data.Count; i++) //스킬 정보 읽어오기 (문자)
        {
            string lang = "EN"; //SettingManager에서 끌어올 수 있게 만들어줌
            
            if (lang.Equals(data[i]["Language"]))
            {
                skills[index] = new Skill();
                skills[index].skillIndex = index;
                skills[index].skillName = data[i]["SkillName"].ToString();
                skills[index].skillDescription = data[i]["Description"].ToString();
                if (skills[index].skillDescription.Contains("/"))
                {
                    string[] sText = skills[index].skillDescription.Split("/");
                    skills[index].skillDescription = "";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        if (j == sText.Length - 1)
                        {
                            skills[index].skillDescription += sText[j]; // '/'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                        }
                        else
                        {
                            skills[index].skillDescription += (sText[j] + "\n");
                        }
                    }
                }
                skills[index].thumnail = cSkillInfo.sImage[index]; //기존에 등록해놨던 스킬 이미지로 적용
                skills[index].coolTime = float.Parse(data[i]["CoolTime"].ToString());
                skills[index].duringTime = float.Parse(data[i]["DuringTime"].ToString());
                //skills[i].effectPrefab = cSkillInfo.effectPrefabs[i];
                //skills[i].effectPos = cSkillInfo.effectPos;
                //skills[i].effectRot = cSkillInfo.effectRot;
                index++;
            }
        }
    }

    
}
