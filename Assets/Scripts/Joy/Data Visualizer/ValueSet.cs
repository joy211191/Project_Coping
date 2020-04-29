using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueSet : MonoBehaviour
{
    public Text valueText;
    public bool graphElement;
    public RectTransform canvas;
    public Transform graphObjectTransform;
    public bool y_Axis;
    public bool x_Axis;
    // Start is called before the first frame update
    void Start()
    {
        if (!y_Axis&&!x_Axis)
            valueText.text = (transform.localScale.y * 10).ToString();
        else if(!x_Axis&&y_Axis)
            valueText.text = (transform.localPosition.y * 10).ToString();
        if (graphElement) {
            canvas.sizeDelta = new Vector2(100, 100);
            canvas.localPosition = graphObjectTransform.position + new Vector3(0, graphObjectTransform.localScale.y/2 + 0.5f, 0);
        }
    }

    public void OverwriteValue(string characterString) {
        valueText.text = characterString;
    }
}
