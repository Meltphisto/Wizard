using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasleHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    StateManager stateManager;

    private void Start()
    {
        stateManager = FindObjectOfType<StateManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Enemy>() != null)
        {
            currentHealth -= collision.transform.GetComponent<Enemy>().AccessData.castDamage;

            Destroy(collision.gameObject);

            if (currentHealth <= 0) { CalstleFall(); }
        }
    }

    void CalstleFall()
    {
        stateManager.ChangeState(GameState.EndState);
    }
}
