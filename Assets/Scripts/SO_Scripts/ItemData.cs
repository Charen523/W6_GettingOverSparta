using UnityEngine;

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; //heal, stamina 등.
    public float value; //얼마나.
}

[CreateAssetMenu(fileName = "Item", menuName = "SO/New Item", order = 2)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public Sprite icon;
    
    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}