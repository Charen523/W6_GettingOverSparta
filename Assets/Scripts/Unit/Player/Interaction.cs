using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Interact")]
    public float checkRate = 0.05f; //상호작용 가능 확인 빈도.
    private float lastCheckTime;
    public float maxCheckDistance; //상호작용 거리(레이 길이)
    public LayerMask PromptLayerMask; //investigate + interact.

    [Header("UI")]
    public GameObject infoObject; //인스펙터창 설정.
    public GameObject currentObject; //null로 시작.
    private Camera mainCam;

    private InfoPanel infoPanel;

    private void Awake()
    {
        infoPanel = infoObject.GetComponent<InfoPanel>();
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            //화면의 중앙에서 레이 쏘기.
            Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, PromptLayerMask))
            {
                if (hit.collider.gameObject != currentObject)
                {
                    currentObject = hit.collider.gameObject;
                    
                    //string tag와 Enum의 이름이 일치해야 함.
                    if (Enum.TryParse(currentObject.tag, out InfoTag infoTag))
                    {
                        infoPanel.ShowPanel(infoTag);
                    }
                }
            }
            else
            {
                currentObject = null;
                infoPanel.HidePanel();
            }
        }
    }
}