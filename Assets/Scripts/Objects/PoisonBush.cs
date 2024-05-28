using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PoisonBush : MonoBehaviour
{
    [Header("Bush Damage")]
    public int damage;
    public float damageRate;

    private List<IDamagable> things = new List<IDamagable>();
    private Dictionary<IDamagable, Coroutine> exitCoroutines = new Dictionary<IDamagable, Coroutine>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            if (exitCoroutines.ContainsKey(damagable))
            {
                StopCoroutine(exitCoroutines[damagable]);
                exitCoroutines.Remove(damagable);
            }
            else
            {
                things.Add(damagable);
                StartCoroutine(DealDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable) && things.Contains(damagable))
        {
            things.Remove(damagable);
            Coroutine exitRoutine = StartCoroutine(PoisonDamageDelay(damagable));
            exitCoroutines.Add(damagable, exitRoutine);
        }
    }

    private IEnumerator DealDamage()
    {
        while (things.Count > 0)
        {
            for (int i = 0; i < things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }

            yield return new WaitForSeconds(damageRate);
        }
    }

    private IEnumerator PoisonDamageDelay(IDamagable damagable)
    {
        float delay = 3f;
        float checkTime = 0f;

        while (checkTime  < delay)
        {
            damagable.TakePhysicalDamage(damage);
            checkTime += damageRate;
            yield return new WaitForSeconds(damageRate);
        }

        exitCoroutines.Remove(damagable);
    }
}
