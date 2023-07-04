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

    public string[] langArr; //언어 이름만을 모은 배열

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

        skills = new Skill[data.Count];
        cSkillInfo = GetComponent<ChangableSkillInfo>();

        for (int i=0; i < data.Count; i++) //스킬 정보 읽어오기 (문자)
        {
            skills[i] = new Skill();
            skills[i].skillIndex = i;
            skills[i].skillName = data[i]["SkillName"].ToString();
            skills[i].skillDescription = data[i]["Description"].ToString();
            if (skills[i].skillDescription.Contains("/"))
            {
                string[] sText = skills[i].skillDescription.Split("/");
                skills[i].skillDescription = "";
                for (int j = 0; j < sText.Length; j++)
                {
                    if (j == sText.Length - 1)
                    {
                        skills[i].skillDescription += sText[j]; // '/'로 나뉘어진 마지막 text의 끝에는 \n을 붙이지 않는다.
                    }
                    else
                    {
                        skills[i].skillDescription += (sText[j] + "\n");
                    }
                }
            }
            if (i >= data.Count / langArr.Length) // i가 5보다 크면
            {
                skills[i].thumnail = cSkillInfo.sImage[i-data.Count/langArr.Length]; //0번부터 다시 돌아가서 적용
            }
            else
            {
                skills[i].thumnail = cSkillInfo.sImage[i]; //기존에 등록해놨던 스킬 이미지로 적용
            }
            skills[i].coolTime = float.Parse(data[i]["CoolTime"].ToString());
            skills[i].duringTime = float.Parse(data[i]["DuringTime"].ToString());
            //skills[i].effectPrefab = cSkillInfo.effectPrefabs[i];
            //skills[i].effectPos = cSkillInfo.effectPos;
            //skills[i].effectRot = cSkillInfo.effectRot;
        }
    }

    
}
