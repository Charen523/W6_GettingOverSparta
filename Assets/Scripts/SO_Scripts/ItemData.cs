using UnityEngine;

[System.Serializable]
public class ItemDataConsumable //Dictionary 대용으로 좋은듯? 
{
    public ConsumableType type; //heal, stamina 등.
    public float value; //얼마나.
}

[CreateAssetMenu(fileName = "Item", menuName = "SO/New Item", order = 2)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public int ItemIndex;
    public InfoData itemInfo;
    
    [Header("Consumable")]
    public ItemDataConsumable consumable;
}