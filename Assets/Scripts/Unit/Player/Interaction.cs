using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Interact")]
    public float checkRate = 0.05f; //��ȣ�ۿ� ���� Ȯ�� ��.
    private float lastCheckTime;
    public float maxCheckDistance; //��ȣ�ۿ� �Ÿ�(���� ����)
    public LayerMask PromptLayerMask; //investigate + interact.

    [Header("UI")]
    public GameObject infoObject; //�ν�����â ����.
    public GameObject currentObject; //null�� ����.
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

            //ȭ���� �߾ӿ��� ���� ���.
            Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, PromptLayerMask))
            {
                if (hit.collider.gameObject != currentObject)
                {
                    currentObject = hit.collider.gameObject;
                    
                    //string tag�� Enum�� �̸��� ��ġ�ؾ� ��.
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