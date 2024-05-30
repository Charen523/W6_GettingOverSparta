using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipObject : MonoBehaviour, IInteractable
{
    public EquipData data;

    private RightHandEquip rightHandEquip;

    private void Start()
    {
        rightHandEquip = CharacterManager.Instance.Player.rightHandEquip;
    }

    public void OnObjectInteract()
    {
        if (rightHandEquip.currentWeapon != null)
        {
            DropEquip();
        }

        rightHandEquip.EquipEnable(data.EquipIndex);

        Destroy(gameObject);
    }

    public void DropEquip()
    {
        GameObject weaponToDrop = rightHandEquip.currentWeapon;

        rightHandEquip.EquipDisable();

        Instantiate(weaponToDrop.GetComponent<EquipObject>().data.equipPrefab,
            rightHandEquip.gameObject.transform.position + (transform.right + transform.up) * 2,
            transform.rotation);
    }
}
