using UnityEngine;

[CreateAssetMenu(fileName = "Interact", menuName = "SO/New Interact", order = 1)]
public class InteractData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
}
