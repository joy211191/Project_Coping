using System.IO;
using UnityEngine;

[System.Serializable]
public class DataSets {
    public DataSet[] DataSetList;
}


public class DataVisualizer : MonoBehaviour {
    public DataSets dataSets;

    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    int charCount = 5;

    public DataRange dataRange;

    void Awake () {
        LoadData();
    }

    public void LoadData () {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/GameplayData.json");
        dataSets = new DataSets();
        JsonUtility.FromJsonOverwrite(jsonString, dataSets);
        for (int i = 0; i < dataSets.DataSetList.Length; i++) {
            for (int j = 0; j < charCount; j++) {
                dataSets.DataSetList[i].dataSetName += glyphs[Random.Range(0, glyphs.Length)];
            }
        }
        dataRange.dataSets = dataSets;
        dataRange.GenerateGraph();
    }
}