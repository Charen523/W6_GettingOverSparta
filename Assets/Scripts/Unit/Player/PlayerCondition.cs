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
            isStaminaDepleted = true; //스테미나 고갈.
        }
        else if (stamina.GetCurrentValue() > 0.0f && isStaminaDepleted)
        {
            isStaminaDepleted = false; //스테미나 고갈 종료.
        }

        //HP가 풀이고 delta가 0보다 크면(=회복이면) 더 이상 changeValue하지 않음.
        if (!(stamina.GetCurrentValue() == stamina.maxValue && stamina.deltaRate > 0))
        {
            stamina.changeValue(stamina.deltaRate * Time.deltaTime);
        }
        

        if (health.GetCurrentValue() == 0.0f)
        {
            Die();
        }
    }

    //외부에서 스테미나 델타 변경할 때(달리기).
    public void changeStaminaDelta(float amount)
    {
        stamina.deltaRate = amount;
    }

    //외부에서 스테미나 절대값 변경할 때(점프).
    public void UseStamina(float amount)
    {
        stamina.changeValue(amount);
    }

    public void HealStamina()
    {
        stamina.deltaRate = 50f; //TODO: SO로 바꾸기.
    }

    public void Die()
    {
        Debug.Log("좀비상태 ON");
    }
}