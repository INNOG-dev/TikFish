using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField]
    private GameObject DialogBoxObject;

    [SerializeField]
    private Button CloseBtn;

    [SerializeField]
    private TextMeshProUGUI Text;

    public void Awake()
    {
        CloseBtn.onClick.AddListener(HideDialogBox);
    }

    public void DisplayDialogBox(string message)
    {
        DialogBoxObject.SetActive(true);
        Text.text = message;
    }

    public void HideDialogBox()
    {
        DialogBoxObject.SetActive(false);
    }
}
