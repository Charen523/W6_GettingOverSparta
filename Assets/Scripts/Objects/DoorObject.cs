using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool isOpen;

    private void Awake()
    {
        isOpen = false;
        doorAnim = GetComponent<Animator>();
    }

    public void OnObjectInteract()
    {
        isOpen = !isOpen;
        doorAnim.SetBool("IsOpen", isOpen);
    }
}
