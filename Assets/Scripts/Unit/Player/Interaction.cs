using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Interact")]
    public float checkRate = 0.05f; //상호작용 가능 확인 빈도.
    private float lastCheckTime;
    public float maxCheckDistance; //상호작용 거리(레이 길이)
    public LayerMask PromptLayerMask;

    [Header("UI")]
    public GameObject infoPrompt; //인스펙터창 설정.

    private Camera mainCam;
    private GameObject currentObject; //null로 시작.
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

            //화면의 중앙에서 레이 쏘기.
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