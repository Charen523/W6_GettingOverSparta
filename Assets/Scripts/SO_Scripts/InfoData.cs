using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "SO/New Info", order = 1)]
public class InfoData : ScriptableObject
{
    [Header("Info")]
    public InfoTag tag; 
    public string displayName;
    public string description;
}
