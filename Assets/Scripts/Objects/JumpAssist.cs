using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAssist : MonoBehaviour
{
    [Header("Jump Assister")]
    public LayerMask PlayerMask = 1 << 6;
    public float jumpAssistForce;

    private GameObject player;

    private Coroutine jumpCoroutine;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerMask) != 0)
        {
            player = other.gameObject;
            StartCoroutine(JumpDelay());
        }
        
    }

    private IEnumerator JumpDelay()
    {
        anim.SetTrigger("PlayerOn");
        yield return new WaitForSeconds(0.3f);

        //점프대에서 1초 기다렸을 때.
        if (CharacterManager.Instance.Player.GetComponent<Transform>().position == player.transform.position)
        {
            player.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpAssistForce, ForceMode.Impulse);
        }
    }
}
