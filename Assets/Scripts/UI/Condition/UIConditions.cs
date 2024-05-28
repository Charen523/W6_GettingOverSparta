using UnityEngine;

public class UIConditions : MonoBehaviour 
{
    public Condition health;
    public Condition stamina;

    private void Awake()
    {
        /*�ν����� None, ��ũ��Ʈ�� �ڵ� �ο�.*/
        health = transform.GetChild(0).GetComponent<Condition>();
        stamina = transform.GetChild(1).GetComponent<Condition>();
    }

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;    
    }
}