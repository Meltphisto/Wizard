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
        //��ȡ���п����ɹ���ĵص�
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
        
        //�ڿ����ɹ���ĵص����ɹ���
        if (!spawnPositions[positionCode].spawnedThisTurn)
        {
            spawnPositions[positionCode].SpawnUnit(unit,this);
        }
        //�������ĵص㱾�������ɹ����������ѡ��ص�
        else
        {
            if (SearchForValidArea() >= 0)
            {
                spawnPositions[SearchForValidArea()].SpawnUnit(unit, this);
            }
        }
    }

    //��ñ���δ���ɹ�����ĵص�
    int SearchForValidArea()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnPositions[i].spawnedThisTurn) { return i; }
        }
        return -1;
    }
    //���õص��Ƿ����ɹ������״̬
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

    //�غϿ�ʼʱ���ɹ���
    public IEnumerator SpawnAtTurnBegin()
    {
        //ÿ�غ�������ɹ��������
        int spawnCount = spawnLoad;
        //�����ػ��е���ʣ���ҳ����ڵ��������������������أ����ɵ���
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
        //�����ڴ��ڵ���ʱ�������й����ƶ�
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
