using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    //获取单位数据
    [SerializeField] EnemyConfig data;
    [SerializeField] float currentHealth;
    [SerializeField] Animator animator;
    [SerializeField] GameObject selector;

    public EnemyConfig AccessData => data;
    public float CurrentHealth => currentHealth;
    public GameObject Selector => selector;
    public bool canMove = false;
    Vector3 targetPosition;
    
    private void Awake()
    {
        currentHealth = data.maximumHealth;
        targetPosition = transform.position;
        animator= GetComponentInChildren<Animator>();
    }

    public void ReceiveDamage(float dmg)
    {
        currentHealth -= dmg;
        animator.SetTrigger("Hit");
        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    public IEnumerator StepForward()
    {
        canMove = false;
        Vector3 startposition = transform.position;
        targetPosition += transform.forward * data.step;
        animator.SetBool("isMoving", true);
        float travelPercent = 0;
        while (travelPercent < 1)
        {
            travelPercent += Time.deltaTime * data.speed;
            transform.position = Vector3.Lerp(startposition, targetPosition, travelPercent);
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("isMoving", false);
    }

    IEnumerator HandleDeath()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


}
