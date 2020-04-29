using System.IO;
using UnityEngine;

[System.Serializable]
public class DataSets {
    public DataSet[] DataSetList;
}


public class DataVisualizer : MonoBehaviour {
    public DataSets dataSets;

    public DataRange dataRange;

    void Awake () {
        LoadData();
    }

    public void LoadData () {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/GameplayData.json");
        dataSets = new DataSets();
        JsonUtility.FromJsonOverwrite(jsonString, dataSets);
        dataRange.dataSets = dataSets;
        dataRange.SetZ(dataSets.DataSetList.Length);
        dataRange.GenerateGraph();
    }
}