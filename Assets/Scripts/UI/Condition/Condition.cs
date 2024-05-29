using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    private Image uiBar;

    /*TODO: SO�� ���� �ʿ䰡 ������? �����ϱ�*/
    [Header("Options")]
    public float startValue = 100; //���۰�.
    public float maxValue = 100; //�ִ밪.
    public float deltaRate = 0; //��ȭ��

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