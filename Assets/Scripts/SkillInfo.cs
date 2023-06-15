using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public string skillName;
    public Sprite thumnail; //Image or sprite
    public float coolTime;
    public float duringTime;
    public GameObject effectPrefab;
    public Vector3 effectPos;
    public Quaternion effectRot;
}

public class SkillInfo : MonoBehaviour
{
    public Skill[] skills;
    public ChangableSkillInfo cSkillInfo;

    public string dataPath;
    public List<Dictionary<string, object>> data;

    //읽어온 csv파일 데이터 저장
    private void Start()
    {
        data = CSVReader.Read(dataPath);
        
        skills = new Skill[data.Count];

        cSkillInfo = GetComponent<ChangableSkillInfo>();


        for (int i=0; i < data.Count; i++) //스킬 정보 읽어오기 (문자)
        {
            skills[i] = new Skill();
            skills[i].skillName = data[i]["SkillName"].ToString();
            skills[i].thumnail = cSkillInfo.sImage[i];
            skills[i].coolTime = float.Parse(data[i]["CoolTime"].ToString());
            skills[i].duringTime = float.Parse(data[i]["DuringTime"].ToString());
            //skills[i].effectPrefab = cSkillInfo.effectPrefabs[i];
            //skills[i].effectPos = cSkillInfo.effectPos;
            //skills[i].effectRot = cSkillInfo.effectRot;
        }
    }

    
}
