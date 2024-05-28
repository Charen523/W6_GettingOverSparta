using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBush : MonoBehaviour
{
    public int damage;
    public float damageRate;

    private List<IDamagable> things = new List<IDamagable>();

    private IEnumerator DealDamage()
    {
        while (things.Count != 0)
        {
            for (int i = 0; i < things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
                yield return new WaitForSeconds(damageRate);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("����");
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            Debug.Log("������ ����");
            things.Add(damagable);
            StartCoroutine(DealDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
