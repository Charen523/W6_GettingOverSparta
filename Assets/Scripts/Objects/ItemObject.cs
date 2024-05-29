using UnityEngine;
using static UnityEditor.Progress;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.itemInfo.displayName}\n{data.itemInfo.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        CharacterManager.Instance.Player.useItem.UseSpeedPill(data);
        Destroy(gameObject);
    }
}