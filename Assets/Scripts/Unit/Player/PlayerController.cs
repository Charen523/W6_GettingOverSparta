using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 currentMovementInput;
    public float moveSpeed;
    public float runSpeed;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    private Vector2 mouseDelta;
    private float currentCamXRot;
    public float lookSensitivity; //마우스 민감도.
    public float maxXLook; //max 각도 정하고
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
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;

        Debug.Log(dir);
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

    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnInventory(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    public void OnMenu(InputAction.CallbackContext context)
    {

    }
}
