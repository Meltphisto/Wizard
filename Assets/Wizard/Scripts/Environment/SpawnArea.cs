using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public bool canSpawn = true;

    public bool spawnedThisTurn = false;

    public void SpawnUnit(GameObject unit, EnemyManager manager)
    {
        Instantiate(unit, transform.position,
                unit.transform.rotation, manager.transform);
        spawnedThisTurn = true;
    }

}
