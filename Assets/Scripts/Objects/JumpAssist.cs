using System.Collections;
using UnityEngine;

public class JumpAssist : MonoBehaviour
{
    [Header("Jump Assister")]
    public LayerMask PlayerMask = 1 << 6;
    public float jumpAssistForce = 300;
    public float tolerance = 0.1f;

    private bool playerInTrigger = false;
    private GameObject player;
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
            playerInTrigger = true;
            StartCoroutine(JumpDelay());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = false;
        }
    }

    private IEnumerator JumpDelay()
    {
        anim.SetTrigger("PlayerOn");

        yield return new WaitForSeconds(1f);

        // 플레이어가 아직 발판 범위 안에 있는지 확인
        if (playerInTrigger && player != null)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpAssistForce, ForceMode.Impulse);
        }
    }
}