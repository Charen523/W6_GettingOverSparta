using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{ 
    public UIConditions uiCondition; //�ν�����â.

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private bool isStaminaDepleted = false;

    private void Update()
    {
        if (stamina.GetCurrentValue() == 0.0f && !isStaminaDepleted)
        {
            CharacterManager.Instance.Player.controller.canRun = false;
            isStaminaDepleted = true; //���׹̳� ��.
        }
        else if (stamina.GetCurrentValue() > 0.0f && isStaminaDepleted)
        {
            isStaminaDepleted = false; //���׹̳� �� ����.
        }

        //HP�� Ǯ�̰� delta�� 0���� ũ��(=ȸ���̸�) �� �̻� changeValue���� ����.
        if (!(stamina.GetCurrentValue() == stamina.maxValue && stamina.deltaRate > 0))
        {
            stamina.changeValue(stamina.deltaRate * Time.deltaTime);
        }
        

        if (health.GetCurrentValue() == 0.0f)
        {
            Die();
        }
    }

    //�ܺο��� ���׹̳� ��Ÿ ������ ��(�޸���).
    public void changeStaminaDelta(float amount)
    {
        stamina.deltaRate = amount;
    }

    //�ܺο��� ���׹̳� ���밪 ������ ��(����).
    public void UseStamina(float amount)
    {
        stamina.changeValue(amount);
    }

    public void HealStamina()
    {
        stamina.deltaRate = 50f; //TODO: SO�� �ٲٱ�.
    }

    public void Die()
    {
        Debug.Log("������� ON");
    }
}