using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlayerDataSO", menuName = "Unit/PlayerData", order = 0)]
public class PlayerDataSO : ScriptableObject, IDamagable
{
    [Header("Movement")]
    public float baseSpeed = 5;
    public float runSpeedRate = 2;
    public float runStaminaDeltaValue = 10f;
    public float jumpForce = 80;
    public float jumpStaminaValue = 10f;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public float lookSensitivity = 0.1f; //���콺 �ΰ���.
    public float maxXLook = 85; //max ���� ���ϰ�
    [HideInInspector()]public float minXLook => -maxXLook; //min �ڵ���ȯ. 

    public void TakePhysicalDamage(int damageAmount)
    {
       
    }
}
