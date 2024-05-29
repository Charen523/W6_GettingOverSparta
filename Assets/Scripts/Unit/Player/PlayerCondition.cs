using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{ 
    public UIConditions uiCondition; //�ν�����â.
    public event Action OnDamage;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private bool isStaminaDepleted = false;
    [HideInInspector] public bool isInvincible = false;

    private void Update()
    {
        StaminaUpdate();
        
        if (health.CurrentValue == 0.0f)
        {
            Die();
        }
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        if (isInvincible)
            return;

        health.changeValue(-damageAmount);
        OnDamage?.Invoke();
    }

    public void HealHealth(float amount)
    {
        health.changeValue(amount);
    }

    //�ܺο��� ���׹̳� ��Ÿ ������ ��(�޸���).
    public void changeStaminaDelta(float amount)
    {
        stamina.deltaRate = amount;
    }

    //�ܺο��� ���׹̳� ���밪 ������ ��(����).
    public void UseStamina(float amount)
    {
        stamina.changeValue(-amount);
    }

    public void HealStamina(float amount)
    {
        stamina.changeValue(amount); 
    }

    private void StaminaUpdate()
    {
        if (stamina.CurrentValue == 0.0f && !isStaminaDepleted)
        {
            CharacterManager.Instance.Player.controller.canRun = false;
            isStaminaDepleted = true; //���׹̳� ��.
        }
        else if (stamina.CurrentValue > 0.0f && isStaminaDepleted)
        {
            isStaminaDepleted = false; //���׹̳� �� ����.
        }

        //HP�� Ǯ�̰� delta�� ���� �ƴϸ� �� �̻� changeValue���� ����.
        if (!(stamina.CurrentValue == stamina.maxValue && stamina.deltaRate >= 0))
        {
            stamina.changeValue(stamina.deltaRate * Time.deltaTime);
        }
    }

    private void Die()
    {
        Application.Quit();

#if UNITY_EDITOR
        // ����Ƽ �����Ͷ�� �̰͵� ����.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}