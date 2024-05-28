using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public List<InfoData> InfoList = new List<InfoData>(); //Inspectorâ
    
    private TextMeshProUGUI infoName;
    private TextMeshProUGUI infoDescription;

    private void Awake()
    {
        infoName = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        infoDescription = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        infoName.text = "No Item";
        infoDescription.text = "No Description";
    }

    public void ShowPanel(InfoTag tag)
    {
        int index = (int)tag;
        gameObject.SetActive(true);
        SetInfoData(InfoList[index]);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void SetInfoData(InfoData infoData)
    {
        infoName.text = infoData.displayName;
        infoDescription.text = infoData.description;
    }
}
