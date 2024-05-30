using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void OnObjectInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        CharacterManager.Instance.Player.useItem.UseThisItem(data);
        Destroy(gameObject);
    }
}