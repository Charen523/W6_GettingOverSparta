using UnityEngine;

public class UIConditions : MonoBehaviour 
{
    public Condition health;
    public Condition stamina;

    private void Awake()
    {
        /*인스펙터 None, 스크립트로 자동 부여.*/
        health = transform.GetChild(0).GetComponent<Condition>();
        stamina = transform.GetChild(1).GetComponent<Condition>();
    }

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;    
    }
}