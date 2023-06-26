using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosInfo : MonoBehaviour
{
    public struct SavePosData
    {
        public int saveIndex;
        public string saveArea;
        public Vector3 savePoints;
    }

    public SavePosData[] savePosDatas;
}
