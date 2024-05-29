using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    private Image uiBar;

    /*TODO: SO로 만들 필요가 있을까? 질문하기*/
    [Header("Options")]
    public float startValue = 100; //시작값.
    public float maxValue = 100; //최대값.
    public float deltaRate = 0; //변화값

    public float CurrentValue { get; private set; }

    private void Awake()
    {
        uiBar = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        CurrentValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetFillAmount();
    }

    public void changeValue(float amount)
    {
        CurrentValue = Mathf.Clamp(CurrentValue + amount, 0, maxValue);
    }

    private float GetFillAmount()
    {
        return CurrentValue / maxValue;
    }
}