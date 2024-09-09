using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        public float spawnInterval = 60f; // 1분마다 스폰
        public float lastSpawnTime;
    }

    public GameObject monsterPrefab;
    public List<SpawnPoint> spawnPoints;
    public int poolSize = 50;
    public float gameTime = 0f;
    public bool isWaveSpawning = false;

    private List<GameObject> monsterPool;
    private int activeMonsters = 0;
    private bool isInitialPhase = true;

    void Start()
    {
        monsterPrefab = Resources.Load<GameObject>("Enemy/DeathKnight");
        InitializePool();
        StartCoroutine(GameTimer());
        StartCoroutine(SpawnRoutine());
    }

    void InitializePool()
    {
        monsterPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gameTime += 1f;

            if (gameTime >= 300f && isInitialPhase) // 5분 경과
            {
                isInitialPhase = false;
                StartCoroutine(SpawnWave());
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (isInitialPhase && !isWaveSpawning)
            {
                foreach (SpawnPoint point in spawnPoints)
                {
                    if (Time.time - point.lastSpawnTime >= point.spawnInterval)
                    {
                        SpawnMonster(point.transform.position);
                        point.lastSpawnTime = Time.time;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            isWaveSpawning = true;
            for (int i = 0; i < 30; i++)
            {
                SpawnPoint randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                SpawnMonster(randomPoint.transform.position);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitUntil(() => activeMonsters == 0);
            isWaveSpawning = false;
            yield return new WaitForSeconds(60f); // 1분 대기 후 다음 웨이브
        }
    }

    void SpawnMonster(Vector3 position)
    {
        GameObject monster = GetPooledMonster();
        if (monster != null)
        {
            monster.transform.position = position;
            monster.SetActive(true);
            activeMonsters++;

            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.OnDeath += HandleMonsterDeath;
            }
        }
    }

    GameObject GetPooledMonster()
    {
        foreach (GameObject monster in monsterPool)
        {
            if (!monster.activeSelf)
            {
                return monster;
            }
        }
        return null;
    }

    void HandleMonsterDeath(Monster monster)
    {
        monster.OnDeath -= HandleMonsterDeath;
        activeMonsters--;
        monster.gameObject.SetActive(false);
    }
}

public class Monster : MonoBehaviour
{
    public event System.Action<Monster> OnDeath;

    public void Die()
    {
        OnDeath?.Invoke(this);
    }
}