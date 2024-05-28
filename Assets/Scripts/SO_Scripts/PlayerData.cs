using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "SO/New Player", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float baseSpeed = 5;
    public float runSpeedRate = 2;
    public float runStaminaDeltaValue = 10f;
    public float jumpForce = 80;
    public float jumpStaminaValue = 10f;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public float lookSensitivity = 0.1f; //마우스 민감도.
    public float maxXLook = 85; //max 각도 정하고
    [HideInInspector()]public float minXLook => -maxXLook; //min 자동변환. 
}
