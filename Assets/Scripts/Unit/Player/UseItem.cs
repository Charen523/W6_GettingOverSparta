using System.Collections;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private PlayerData playerData;

    private void Start()
    {
        playerData = CharacterManager.Instance.Player.playerData;
    }

    public void UseSpeedPill(ItemData item)
    {
        float value = item.consumables[0].value;
        StartCoroutine(SpeedPillTime(value));
    }

    private IEnumerator SpeedPillTime(float data)
    {
        playerData.baseSpeed += data;
        yield return new WaitForSeconds(3f);
        playerData.baseSpeed -= data;
    }
}
