using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Interact")]
    public float checkRate = 0.05f; //��ȣ�ۿ� ���� Ȯ�� ��.
    private float lastCheckTime;
    public float maxCheckDistance; //��ȣ�ۿ� �Ÿ�(���� ����)
    public LayerMask PromptLayerMask;

    [Header("UI")]
    public GameObject infoPrompt; //�ν�����â ����.

    private Camera mainCam;
    private GameObject currentObject; //null�� ����.
    private InfoPanel infoPanel;
    private IInteractable currentInteractable;

    private void Awake()
    {
        infoPanel = infoPrompt.GetComponent<InfoPanel>();
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
            
            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, PromptLayerMask))
            {
                if (hit.collider.gameObject != currentObject)
                {
                    currentObject = hit.collider.gameObject;
                    
                    if (currentObject.TryGetComponent(out IInteractable interactable)) 
                    {
                        currentInteractable = interactable;
                    }

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentInteractable != null)
        {
            currentInteractable.OnInteract();
            currentObject = null;
            currentInteractable = null;
        }
    }
}