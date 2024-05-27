using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    }
}