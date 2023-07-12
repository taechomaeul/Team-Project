using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour
{
    public GameObject nBoxPrefab;    //움직이지 않는 Prefab
    public GameObject movingBoxPrefab; //움직이는 Prefab
    public int mapDataCnt; // 맵 데이터의 개수
    public Vector3 originPos; //(0, 0) 박스의 위치
    public string mapPath;
    public List<Dictionary<string, object>> map;

    void Awake()
    {
        RandomData();
        LoadMapData();
    }

    public void RandomData()
    {
        int rnd = Random.Range(0, mapDataCnt);
        //Debug.Log($"RND : {rnd+1}");
        mapPath = $"Map/MapData{rnd+1}";
        //Debug.Log($"mapPath : {mapPath}");
    }

    public void LoadMapData()
    {
        //Debug.Log($"mapPath in LoadMapData : {mapPath}");
        map = CSVReader.Read(mapPath);
        int mapObjIdx;
        Vector3 curPos = originPos;
        for (int i = 0; i < 9; i++) //Height
        {
            float z = curPos.z;
            for (int j = 0; j < 8; j++) //Weight
            {
                float x = curPos.x;
                mapObjIdx = int.Parse(map[i][j.ToString()].ToString());
                GenerateMapObj(new Vector3(x, curPos.y, z), mapObjIdx);
                //Debug.Log($"MapIndex [{i}][{j}] : {mapObjIdx}");
                curPos.x += 3f;
            }
            curPos.x = originPos.x;
            curPos.z -= 3f;
        }
    }

    public void GenerateMapObj(Vector3 pos, int prefabIdx)
    {
        GameObject newBox;
        switch (prefabIdx)
        {
            case 0:
                //Debug.Log("No Instatiate Box");
                break;

            case 1:
                newBox = Instantiate(nBoxPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //prefab 생성
                newBox.name = "NoMoveBox"; //이름 변경
                break;

            case 2:
                newBox = Instantiate(movingBoxPrefab, new Vector3(pos.x, 0.39f, pos.z + 1.5f), Quaternion.Euler(-90f, 0, 0)); //prefab 생성
                newBox.name = "MovingBox"; //이름 변경
                break;
        }
        
    }

}
