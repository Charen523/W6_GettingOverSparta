using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 currentMovementInput;
    public float baseSpeed = 5; 
    public float runSpeedRate = 2;
    private float currentSpeed;
    public float jumpForce = 80;
    public LayerMask groundLayerMask;

    [Header("Look")]
    private Vector2 mouseDelta;
    private float currentCamXRot;
    public float lookSensitivity; //마우스 민감도.
    public float maxXLook = 85; //max 각도 정하고
    private float minXLook => -maxXLook; //min 자동변환. 

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rb;
    private Transform cameraContainer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraContainer = transform.GetChild(0); //0:cameraContainer.
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        Move();   
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMovementInput = Vector2.zero;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x; //방향
        dir *= currentSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        currentCamXRot += mouseDelta.y * lookSensitivity;
        currentCamXRot = Mathf.Clamp(currentCamXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-currentCamXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        //TODO: 스테미나 바와 연계.
        if (context.phase == InputActionPhase.Performed)
        {
            currentSpeed = baseSpeed * runSpeedRate;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            currentSpeed = baseSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }
    }

    bool IsGrounded()
    {
        float directionOffset = 0.1f;
        float heightOffest = 1.45f;
        float rayLength = 0.1f;

        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * directionOffset) - (transform.up * heightOffest), Vector3.down),
            new Ray(transform.position - (transform.forward * directionOffset) - (transform.up * heightOffest), Vector3.down),
            new Ray(transform.position + (transform.right * directionOffset) - (transform.up * heightOffest), Vector3.down),
            new Ray(transform.position - (transform.right * directionOffset) - (transform.up * heightOffest), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out RaycastHit hit, rayLength, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        //TODO: 인벤토리 UI 만들기
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //TODO: 상호작용 기능 만들기
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        //TODO: 메뉴UI 만들기
    }
}
