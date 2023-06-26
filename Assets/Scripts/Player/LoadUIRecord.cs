using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadUIRecord : MonoBehaviour
{
    private ShowScript showScript;

    void Start()
    {
        showScript = GameObject.Find("ActionFunction").GetComponent<ShowScript>();
    }

    /// <summary>
    /// 일지에서 record 불러오기용 함수
    /// </summary>
    public void LoadRecord()
    {
        string colliName = gameObject.name;
        //Debug.Log($"ColliName : {colliName}");
        GameObject context = gameObject.transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).gameObject;
        //Debug.Log(context.name);
        StartCoroutine(showScript.LoadRecordData(colliName, context.GetComponent<Text>()));
    }
    //버튼 이동시 instatiate한 것들 없애는 코드 만들어야 합니다!!

    public void LoadTip()
    {
        string colliName = gameObject.name;
        //Debug.Log($"ColliName : {colliName}");
        GameObject context = gameObject.transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).gameObject;
        //Debug.Log(context.name);
        StartCoroutine(showScript.LoadTipData(colliName, context.GetComponent<Text>()));
    }

}
