using UnityEngine;

public class RightHandEquip : MonoBehaviour
{
    public GameObject currentWeapon;

    private void Start()
    {
        currentWeapon = null;
    }

    public void EquipDisable()
    {
        if (currentWeapon.activeSelf)
            currentWeapon.SetActive(false);

        currentWeapon = null;
    }

    public void EquipEnable(int newEquipIndex)
    {
        currentWeapon = transform.GetChild(1).GetChild(newEquipIndex).gameObject;

        if (currentWeapon == null)
        {
            Debug.LogError("존재하지 않는 인덱스.");
            return;
        }

        currentWeapon.SetActive(true);
    }
}
