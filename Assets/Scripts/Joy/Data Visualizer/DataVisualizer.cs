using System.IO;
using UnityEngine;

[System.Serializable]
public class DataSets {
    public DataSet[] DataSetList;
}


public class DataVisualizer : MonoBehaviour {
    public DataSets dataSets;

    void Awake () {
        LoadData();
    }

    public void LoadData () {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/GameplayData.json");
        Debug.Log(jsonString);
        dataSets = new DataSets();
        JsonUtility.FromJsonOverwrite(jsonString, dataSets);
        
    }
}