using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //current Input Values
    private Vector2 moveInput;
    private Vector2 mouseDelta;
    private float currentCamXRot;

    //Unity Variables
    public PlayerData playerData; //Instpector에서 할당.
    private PlayerCondition playerCondition;
    private Rigidbody rb;
    private Transform cameraContainer;

    
    //current Action bools
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
        playerCondition = CharacterManager.Instance.Player.condition;
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
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
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
            playerCondition.changeStaminaDelta(-playerData.runStaminaDeltaValue);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            playerCondition.HealStamina();
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
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x; //방향
        dir *= CurrentSpeed();
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    private void CameraLook()
    {
        currentCamXRot += mouseDelta.y * playerData.lookSensitivity;
        currentCamXRot = Mathf.Clamp(currentCamXRot, playerData.minXLook, playerData.maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-currentCamXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * playerData.lookSensitivity, 0);
    }

    private float CurrentSpeed()
    {
        if (canRun && isRunning)
        {
            return playerData.baseSpeed * playerData.runSpeedRate;
        }
        else
        {
            return playerData.baseSpeed;
        }
    }

    private void Jump()
    {
        if (isJumping && IsGrounded())
        {
            rb.AddForce(Vector3.up * playerData.jumpForce, ForceMode.Impulse);
            playerCondition.UseStamina(playerData.jumpStaminaValue);
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
            if (Physics.Raycast(rays[i], rayLength, playerData.groundLayerMask))
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
