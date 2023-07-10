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
        StartCoroutine(showRecord.LoadRecordData(colliName, showRecord.recordContext));
    }

    /// <summary>
    /// 일지에서 Tip(나의 일지) 불러오기용 함수
    /// </summary>
    public void LoadTip()
    {
        string colliName = gameObject.name;
        StartCoroutine(showTip.LoadTipData(colliName, showTip.tipContext));
    }

}
