using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{ 
    public UIConditions uiCondition; //인스펙터창.

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private bool isStaminaDepleted = false;

    private void Update()
    {
        if (stamina.GetCurrentValue() == 0.0f && !isStaminaDepleted)
        {
            CharacterManager.Instance.Player.controller.canRun = false;
            isStaminaDepleted = true;
        }
        else if (stamina.GetCurrentValue() > 0.0f && isStaminaDepleted)
        {
            isStaminaDepleted = false;
        }

        if (!(stamina.GetCurrentValue() == stamina.maxValue && stamina.deltaRate > 0))
        {
            stamina.changeValue(stamina.deltaRate * Time.deltaTime);
        }
        

        if (health.GetCurrentValue() == 0.0f)
        {
            Die();
        }
    }

    public void changeStaminaDelta(float amount)
    {
        stamina.deltaRate = amount;
    }

    public void HealStamina()
    {
        stamina.deltaRate = 5f;
    }

    public void Die()
    {
        Debug.Log("좀비상태 ON");
    }
}