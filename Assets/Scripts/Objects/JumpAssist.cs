using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAssist : MonoBehaviour
{
    [Header("Jump Assister")]
    public LayerMask PlayerMask = 1 << 6;
    public float jumpAssistForce;


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerMask) != 0)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpAssistForce, ForceMode.Impulse);
        }
    }
}
