using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpell : MonoBehaviour
{
    [SerializeField] Spell spell;
    [SerializeField] GameObject marker;
    [SerializeField] GameObject VFX;

    [SerializeField] bool isMarker = true;

    Collider trigger;
    private void Awake()
    {
        marker.SetActive(true);
        trigger = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (isMarker)
            {
                other.GetComponent<Enemy>().Selector.SetActive(true);
            }
            else
            {
                other.GetComponent<Enemy>().Selector.SetActive(false);
                other.GetComponent<Enemy>().ReceiveDamage(spell.damage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (isMarker)
            {
                other.GetComponent<Enemy>().Selector.SetActive(false);
            }
        }
    }

    public IEnumerator PlayAOE(float time)
    {
        marker.SetActive(false);
        trigger.isTrigger = false;
        yield return new WaitForSeconds(time);
        VFX.SetActive(true);
        //yield return new WaitForSeconds(1f);
        trigger.isTrigger = true;
        isMarker = false;
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
