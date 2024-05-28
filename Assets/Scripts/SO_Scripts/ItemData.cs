using UnityEngine;

[System.Serializable]
public class ItemDataConsumable //Dictionary ������� ������? 
{
    public ConsumableType type; //heal, stamina ��.
    public float value; //�󸶳�.
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