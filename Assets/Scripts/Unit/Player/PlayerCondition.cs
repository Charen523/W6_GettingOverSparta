using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{ 
    public UIConditions uiCondition; //�ν�����â.
    public event Action OnDamage;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private bool isStaminaDepleted = false;

    private void Update()
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

    public void HealStamina()
    {
        stamina.deltaRate = 50f; //TODO: SO�� �ٲٱ�.
    }

    public void Die()
    {
        Application.Quit();

#if UNITY_EDITOR
        // ����Ƽ �����Ͷ�� �̰͵� ����.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}