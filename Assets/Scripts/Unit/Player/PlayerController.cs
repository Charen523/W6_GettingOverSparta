using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float staminahealdelta = 50f; //TODO: 위치변경 필요.
    public int jumpAgain;

    //current Input Values
    private Vector2 moveInput;
    private Vector2 mouseDelta;
    private float currentCamXRot;

    //Unity Variables
    private PlayerData playerData;
    private PlayerCondition playerCondition;
    private Rigidbody rb;
    private Transform cameraContainer;
    private Animator playerAnim;

    //current Action bools
    private bool canLook = true;
    [HideInInspector()] public bool canRun = true;
    private bool isRunning = false;
    private bool perspectiveFirst = true;

    private Transform platformTransform;
    private Vector3 previousplatformPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        cameraContainer = transform.GetChild(0); //0:cameraContainer.
    }

    private void Start()
    {
        playerData = CharacterManager.Instance.Player.playerData;
        playerCondition = CharacterManager.Instance.Player.condition;
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
            playerCondition.changeStaminaDelta(staminahealdelta); 
            canRun = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void OnPerspective(InputAction.CallbackContext context)
    {
        if (perspectiveFirst)
        {
            perspectiveFirst = !perspectiveFirst;
            Camera.main.cullingMask |= 1 << 13;
            Camera.main.transform.localPosition = new Vector3(0, 0.5f, -1.5f);
        }
        else
        {
            perspectiveFirst = !perspectiveFirst;
            Camera.main.cullingMask &= ~(1 << 13);
            Camera.main.transform.localPosition = Vector3.zero;
        }
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

        //움직이는 것 위에 있을 때 그 움직임을 따라감.
        if (platformTransform != null)
        {
            Vector3 carMovement = platformTransform.position - previousplatformPosition;
            transform.position += carMovement;
            previousplatformPosition = platformTransform.position;
        }
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
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * playerData.jumpForce, ForceMode.Impulse);
            playerCondition.UseStamina(playerData.jumpStaminaValue);
           
        }
    }

    private bool IsGrounded()
    {
        float directionOffset = 0.1f;
        float heightOffest = 0.1f;
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
        
        if (jumpAgain > 0)
        {
            jumpAgain = Mathf.Max(--jumpAgain, 0);
            return true;
        }

        //공중에 떠 있는 상태에서는 가속 불가 예외처리.
        if (!isRunning)
        {
            canRun = false;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("IsPlayer");
            platformTransform = collision.transform;
            previousplatformPosition = platformTransform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            platformTransform = null;
        }
    }

}
