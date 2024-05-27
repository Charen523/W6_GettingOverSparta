using System.Collections.Generic;
using UnityEngine;

public class InfoToOverLay : MonoBehaviour
{
    private Camera mainCamera;
    private Dictionary<string, GameObject> InfoDictionary;
    private RectTransform currentPanel;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void ShowPanel(string tag)
    {
        InfoDictionary[tag].SetActive(true);
        currentPanel = InfoDictionary[tag].GetComponent<RectTransform>();
    }

    public void HidePanel(string tag)
    {
        InfoDictionary[tag].SetActive(false);
        currentPanel = null;
    }

    private void SetPosition()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(currentPanel.position);

        Vector2 overlayPosition = new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height);
    }
}
