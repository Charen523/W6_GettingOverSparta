using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{ 
    public UIConditions uiCondition; //인스펙터창.
    public event Action OnDamage;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private bool isStaminaDepleted = false;

    private void Update()
    {
        if (stamina.CurrentValue == 0.0f && !isStaminaDepleted)
        {
            CharacterManager.Instance.Player.controller.canRun = false;
            isStaminaDepleted = true; //스테미나 고갈.
        }
        else if (stamina.CurrentValue > 0.0f && isStaminaDepleted)
        {
            isStaminaDepleted = false; //스테미나 고갈 종료.
        }

        //HP가 풀이고 delta가 음이 아니면 더 이상 changeValue하지 않음.
        if (!(stamina.CurrentValue == stamina.maxValue && stamina.deltaRate >= 0))
        {
            stamina.changeValue(stamina.deltaRate * Time.deltaTime);
        }
        

        if (health.CurrentValue == 0.0f)
        {
            Die();
        }
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.changeValue(-damageAmount);
        OnDamage?.Invoke();
    }

    //외부에서 스테미나 델타 변경할 때(달리기).
    public void changeStaminaDelta(float amount)
    {
        stamina.deltaRate = amount;
    }

    //외부에서 스테미나 절대값 변경할 때(점프).
    public void UseStamina(float amount)
    {
        stamina.changeValue(-amount);
    }

    public void HealStamina()
    {
        stamina.deltaRate = 50f; //TODO: SO로 바꾸기.
    }

    public void Die()
    {
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터라면 이것도 종료.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}