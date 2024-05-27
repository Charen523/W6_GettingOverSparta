using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    private Image uiBar;

    [Header("Condition Options")]
    public float startValue = 100; //시작값.
    public float maxValue = 100; //최대값.
    public float deltaRate = 0; //변화값

    private float currentValue;

    private void Awake()
    {
        uiBar = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        currentValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetFillAmount();
    }

    public void changeValue(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public float GetFillAmount()
    {
        return currentValue / maxValue;
    }
}