using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Spell spell;
    

    [SerializeField] GameObject projectileVFX, hitVFX;
    bool isMoving = true;

    GameObject target;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Enemy>() != null)
        {
            isMoving = false;
            collision.transform.GetComponent<Enemy>().ReceiveDamage(spell.damage);
        }
        StartCoroutine(PlayHitVFX());
        
    }
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    IEnumerator PlayHitVFX()
    {
        projectileVFX.SetActive(false);
        hitVFX.SetActive(true);
        yield return new WaitForSeconds(spell.animationTime - spell.prepareTime);
        Destroy(gameObject);
    }   

    private void Update()
    {
        if(target != null && isMoving)
        {
            transform.position =
            Vector3.MoveTowards(transform.position, target.transform.position, spell.speed * Time.deltaTime);
            transform.LookAt(target.transform);
        }
    }

    
}
