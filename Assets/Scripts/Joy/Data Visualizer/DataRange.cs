using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRange : MonoBehaviour {
    public List<GameObject> objects = new List<GameObject>();
    public List<Transform> graphParents = new List<Transform>();
    public Vector3 graphRange;
    [SerializeField]
    float distance;

    DataVisualizer dataVisualizer;
    public GameObject graphElement;

    public List<string> powerUpNames = new List<string>();

    public DataSets dataSets;

    void Awake () {
        GenerateGraphBounds();
        dataVisualizer = FindObjectOfType<DataVisualizer>();
    }

    public void GenerateGraphBounds () {
        for (int i = 0; i < graphRange.x; i++) {
            GameObject go = Instantiate(objects[0], Vector3.right + new Vector3(1 * i, 0, 0), Quaternion.identity);
            go.transform.parent = graphParents[0];
        }
        for (int i = 0; i < graphRange.y; i++) {
            GameObject go = Instantiate(objects[1], Vector3.up + new Vector3(0, 1 * i, 0), Quaternion.identity);
            go.transform.parent = graphParents[1];
        }
        for (int i = 0; i < graphRange.z; i++) {
            GameObject go = Instantiate(objects[2], Vector3.forward + new Vector3(0, 0, 1 * i), Quaternion.identity);
            go.transform.parent = graphParents[2];
        }
    }

    public void GenerateGraph () {
        for (int i = 0; i < dataSets.DataSetList.Length; i++) {
            for (int j = 0; j < dataSets.DataSetList[i].numericalValues.Length; j++) {
                GameObject go = Instantiate(graphElement);
                go.transform.localScale = new Vector3(0.5f,(float) dataSets.DataSetList[i].numericalValues[j]/10, 0.5f);
                go.transform.localPosition = new Vector3(1+1*j, go.transform.localScale.y / 2, 1+1*i);
            }
        }
    }
}
