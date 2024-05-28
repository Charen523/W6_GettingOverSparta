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
    public string displayName = "None";
    public string description = "No Description";
    public Sprite icon;
    
    [Header("Stacking")]
    public bool canStack = true;
    public int maxStackAmount = 99;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}