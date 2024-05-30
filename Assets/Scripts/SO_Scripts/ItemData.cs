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
    public InfoData itemInfo;
    
    [Header("Consumable")]
    public ItemDataConsumable consumable;
}