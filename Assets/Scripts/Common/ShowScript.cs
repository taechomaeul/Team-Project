using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ShowScript : MonoBehaviour
{
    public string scriptPath;
    public string recordPath;
    public List<Dictionary<string, object>> script;
    public List<Dictionary<string, object>> record;

    public Text scriptText;
    public Text recordNameText;

    public Text recordText;


    void Start()
    {
        script = CSVReader.Read(scriptPath);
        DOTween.Init();

        scriptText.text = " ";
        recordText.text = " ";
    }

    private void Update()
    {

    }

    public void LoadScriptDataFromCSV()
    {
        script = CSVReader.Read(scriptPath);

       

    }

    public IEnumerator LoadRecordData(string colliName)
    {
        yield return StartCoroutine(LoadRecordDataFromCSV(colliName));
    }

    public IEnumerator LoadRecordDataFromCSV(string colliName)
    {
        record = CSVReader.Read(recordPath);
        recordNameText.text = colliName;
        for (int i = 0; i < record.Count; i++)
        {
            if (colliName.Equals(record[i]["RECORD_NAME"]))
            {
                recordText.text = record[i]["CONTEXT"].ToString();
                if (recordText.text.Contains("/"))
                {
                    string[] sText = recordText.text.Split("/");
                    recordText.text = " ";
                    for (int j = 0; j < sText.Length; j++)
                    {
                        recordText.text += (sText[j] + "\n");
                    }
                    //Debug.Log($"{recordText.text}");
                    break;
                }
            }

        }
        yield return new WaitForSeconds(2f);
    }

}