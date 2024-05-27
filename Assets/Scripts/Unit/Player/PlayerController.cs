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
    public float lookSensitivity; //���콺 �ΰ���.
    public float maxXLook = 85; //max ���� ���ϰ�
    private float minXLook => -maxXLook; //min �ڵ���ȯ. 

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
        //TODO: �κ��丮 UI �����
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //TODO: ��ȣ�ۿ� ��� �����
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        //TODO: �޴�UI �����
    }

    private void Move()
    {
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x; //����
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

        //���߿� �� �ִ� ���¿����� ���� �Ұ� ����ó��.
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
