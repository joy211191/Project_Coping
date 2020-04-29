using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataRange : MonoBehaviour {
    public List<GameObject> objects = new List<GameObject>();
    public List<Transform> graphParents = new List<Transform>();
    public Vector3 graphRange;

    DataVisualizer dataVisualizer;
    public GameObject graphElement;

    public List<string> powerUpNames = new List<string>();

    public DataSets dataSets;
    public Transform graphObjectParent;

    const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    int charCount = 5;

    void Awake () {
        GenerateGraphBounds();
        dataVisualizer = FindObjectOfType<DataVisualizer>();
    }

    public void SetZ (float value) {
        graphRange = new Vector3(graphRange.x, graphRange.y, value);
    }

    public void GenerateGraphBounds () {
        for (int i = 0; i < graphRange.x; i++) {
            GameObject go = Instantiate(objects[0], Vector3.right + new Vector3(i, 0, 0), Quaternion.identity);
            go.transform.parent = graphParents[0];
            go.transform.GetComponentInChildren<ValueSet>().OverwriteValue(powerUpNames[i]);
        }
        for (int i = 0; i < graphRange.y; i++) {
            GameObject go = Instantiate(objects[1], Vector3.up + new Vector3(0, i, 0), Quaternion.identity);
            go.transform.parent = graphParents[1];
        }
    }

    public void GenerateGraph () {
        for (int i = 0; i < dataSets.DataSetList.Length; i++) {
            GameObject go = Instantiate(objects[2], Vector3.forward + new Vector3(0, 0, i), Quaternion.identity);
            go.transform.parent = graphParents[2];
            for (int k = 0; k < charCount; k++) {
                go.GetComponentInChildren<Text>().text += glyphs[Random.Range(0, glyphs.Length)];
            }
            for (int j = 0; j < dataSets.DataSetList[i].numericalValues.Length; j++) {
                GameObject go_j = Instantiate(graphElement);
                Transform graphSubElement = go_j.transform.Find("GraphObjects");
                graphSubElement.localScale = new Vector3(0.5f,(float) dataSets.DataSetList[i].numericalValues[j]/10, 0.5f);
                graphSubElement.localPosition = new Vector3(1+1*j, graphSubElement.localScale.y / 2, 1+1*i);
                go_j.transform.parent = graphObjectParent;
                //these can be static
            }
        }
    }
}
