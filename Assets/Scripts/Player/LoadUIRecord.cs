using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadUIRecord : MonoBehaviour
{
    private ShowRecord showRecord;
    private ShowTip showTip;

    void Start()
    {
        showRecord = GameObject.Find("ActionFunction").GetComponent<ShowRecord>();
        showTip = GameObject.Find("ActionFunction").GetComponent<ShowTip>();
    }

    /// <summary>
    /// 일지에서 record(누군가의 일지) 불러오기용 함수
    /// </summary>
    public void LoadRecord()
    {
        string colliName = gameObject.name;
        GameObject context = gameObject.transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).gameObject;
        StartCoroutine(showRecord.LoadRecordData(colliName, context.GetComponent<Text>()));
    }

    /// <summary>
    /// 일지에서 Tip(나의 일지) 불러오기용 함수
    /// </summary>
    public void LoadTip()
    {
        string colliName = gameObject.name;
        GameObject context = gameObject.transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).gameObject;
        StartCoroutine(showTip.LoadTipData(colliName, context.GetComponent<Text>()));
    }

}
