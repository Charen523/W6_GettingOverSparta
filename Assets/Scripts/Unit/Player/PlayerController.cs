using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 currentMovementInput;
    public float baseSpeed = 5; 
    public float runSpeedRate = 2;
    public float runStaminaDelta = 10f;
    public float jumpForce = 80;
    public float jumpStaminaRate = 10f;
    public LayerMask groundLayerMask;

    [Header("Look")]
    private Vector2 mouseDelta;
    private float currentCamXRot;
    public float lookSensitivity; //마우스 민감도.
    public float maxXLook = 85; //max 각도 정하고
    private float minXLook => -maxXLook; //min 자동변환. 

    private Rigidbody rb;
    private Transform cameraContainer;

    private bool canLook = true;
    [HideInInspector()] public bool canRun = true;
    private bool isRunning = false;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraContainer = transform.GetChild(0); //0:cameraContainer.
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
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

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
            CharacterManager.Instance.Player.condition.changeStaminaDelta(-runStaminaDelta);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            CharacterManager.Instance.Player.condition.HealStamina();
            canRun = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isJumping = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            isJumping = false;
        }
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

    private void Move()
    {
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x; //방향
        dir *= CurrentSpeed();
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    private void CameraLook()
    {
        currentCamXRot += mouseDelta.y * lookSensitivity;
        currentCamXRot = Mathf.Clamp(currentCamXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-currentCamXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private float CurrentSpeed()
    {
        if (canRun && isRunning)
        {
            return baseSpeed * runSpeedRate;
        }
        else
        {
            return baseSpeed;
        }
    }

    private void Jump()
    {
        if (isJumping && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    private bool IsGrounded()
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
                canRun = true;
                return true;
            }
        }

        //공중에 떠 있는 상태에서는 가속 불가 예외처리.
        if (!isRunning)
        {
            canRun = false;
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
