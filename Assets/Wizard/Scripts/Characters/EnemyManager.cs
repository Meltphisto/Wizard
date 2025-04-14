using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    
    [SerializeField] private List<SpawnArea> spawnPositions = new List<SpawnArea>();
    [SerializeField] int enemyLoad,enemyLeft,spawnLoad;
    [SerializeField] private float spawnTime = 1f;

    public int EnemyLeft { get { return enemyLeft; } }

    public bool willSpawn = true;
    public bool isVictory = false;
    private void Awake()
    {
        //获取所有可生成怪物的地点
        SpawnArea[] areas = FindObjectsOfType<SpawnArea>();
        foreach (SpawnArea area in areas)
        {
            if (area.canSpawn)
            {
                spawnPositions.Add(area);
            }
        }

    }

    public void SpawnEnemy(GameObject unit, int positionCode)
    {
        
        //在可生成怪物的地点生成怪物
        if (!spawnPositions[positionCode].spawnedThisTurn)
        {
            spawnPositions[positionCode].SpawnUnit(unit,this);
        }
        //如果输入的地点本轮已生成过怪物，则重新选择地点
        else
        {
            if (SearchForValidArea() >= 0)
            {
                spawnPositions[SearchForValidArea()].SpawnUnit(unit, this);
            }
        }
    }

    //获得本轮未生成过怪物的地点
    int SearchForValidArea()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnPositions[i].spawnedThisTurn) { return i; }
        }
        return -1;
    }
    //重置地点是否生成过怪物的状态
    void ResetAllArea()
    {
        foreach (SpawnArea area in spawnPositions)
        {
            if (area.spawnedThisTurn)
            {
                area.spawnedThisTurn = false;
            }
        }
    }

    //回合开始时生成怪物
    public IEnumerator SpawnAtTurnBegin()
    {
        //每回合最多生成怪物的数量
        int spawnCount = spawnLoad;
        //当本关还有敌人剩余且场景内敌人数量不超过场景承载，生成敌人
        if (transform.childCount < enemyLoad && enemyLeft > 0)
        {
            
            while (spawnCount > 0)
            {
                int randomEnemyCode = Random.Range(0, enemyPrefabs.Count);
                int randomPos = Random.Range(0, spawnPositions.Count);
                SpawnEnemy(enemyPrefabs[randomEnemyCode], randomPos);
                spawnCount--;
                enemyLeft--;
                yield return new WaitForSeconds(spawnTime);
            }
        }
        ResetAllArea();
    }

    public bool WillSpawnEnemy()
    {
        return transform.childCount < enemyLoad && enemyLeft > 0;
    }

    public IEnumerator MoveAllEnemy()
    {
        //场景内存在敌人时控制所有怪物移动
        if (transform.childCount > 0)
        {
            foreach (Transform enemyUnit in transform)
            {
                if (enemyUnit.GetComponent<Enemy>().canMove)
                {
                    StartCoroutine(enemyUnit.GetComponent<Enemy>().StepForward());
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }

    public void SetEnemyMoveState()
    {
        if(transform.childCount > 0)
        {
            foreach(Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            {
                enemy.canMove = true;
            }
        }
    }

    private void Update()
    {
        if (enemyLeft == 0 && transform.childCount == 0)
        {
            isVictory = true;
        }
    }
}
