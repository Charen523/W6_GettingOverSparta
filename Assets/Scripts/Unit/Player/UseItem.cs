using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UseItem : MonoBehaviour
{
    public float SpeedItemTime = 5f;

    private PlayerData playerData;
    private PlayerCondition condition;
    private PlayerController controller;

    private void Start()
    {
        playerData = CharacterManager.Instance.Player.playerData;
        condition = CharacterManager.Instance.Player.condition;
        controller = CharacterManager.Instance.Player.controller;  
    }

    public void UseThisItem(ItemData data)
    {
        ItemDataConsumable consumable = data.consumable;

        switch(consumable.type)
        {
            case ConsumableType.Health:
                UseHealItem(consumable.value);
                break;
            case ConsumableType.Stamina:
                UseStaminaItem(consumable.value);
                break;
            case ConsumableType.Speed:
                StartCoroutine(UseSpeedItem(consumable.value));
                break;
            case ConsumableType.Jump:
                UseJumpItem(consumable.value);
                break;
            case ConsumableType.Invincible:
                StartCoroutine(UseInvincibleItem(consumable.value));
                break;
            default:
                Debug.LogError("일치하는 타입 없음");
                break;
        }
    }

    private void UseHealItem(float data)
    {
        condition.HealHealth(data);
    }

    private void UseStaminaItem(float data)
    {
        condition.HealStamina(data);
    }

    private IEnumerator UseSpeedItem(float data)
    {
        playerData.baseSpeed += data;
        yield return new WaitForSeconds(SpeedItemTime);
        playerData.baseSpeed -= data;
    }

    private void UseJumpItem(float data)
    {
        controller.jumpAgain += (int)data;
    }

    private IEnumerator UseInvincibleItem(float data)
    {
        condition.isInvincible = true;
        yield return new WaitForSeconds(data);
        condition.isInvincible = false;
    }
}
