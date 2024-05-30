using UnityEngine;

[CreateAssetMenu(fileName = "Equip", menuName = "SO/New Equip", order = 3)]
public class EquipData : ScriptableObject
{
    [Header("Info")]
    public int EquipIndex;
    public InfoData EquipInfo;

    [Header("Equip")]
    public GameObject equipPrefab;
}
