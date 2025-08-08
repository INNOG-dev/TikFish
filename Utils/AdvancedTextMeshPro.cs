using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class AdvancedTextMeshPro : MonoBehaviour
{

    private TextMeshProUGUI Txt;

    private string currentTxt;

    public float padding = 10f;

    public enum Direction
    {
        Horizontal,
        Vertical
    };

    public Direction direction;



    void Awake()
    {
        Txt = GetComponent<TextMeshProUGUI>();
        currentTxt = Txt.text;
        ResizeTextArea();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTxt != Txt.text)
        {
            currentTxt = Txt.text;
            ResizeTextArea();
        }
    }

    private void ResizeTextArea()
    {
        if(direction == Direction.Horizontal)
        {
            float preferredWidth = Txt.GetPreferredValues().x;
            Txt.rectTransform.sizeDelta = new Vector2(preferredWidth + padding, Txt.rectTransform.rect.height);
        }
        else
        {
            float preferredHeight = Txt.GetPreferredValues().y;
            Txt.rectTransform.sizeDelta = new Vector2(Txt.rectTransform.rect.width, preferredHeight + padding);
        }
    }
}
