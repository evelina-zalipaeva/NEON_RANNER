using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject[] obstaclePrefabs; // Машины
    public GameObject coinPrefab;        // Монета

    [Header("Настройки")]
    public Transform player;
    public float spawnDistance = 100f;   // Как далеко впереди создавать
    public float spawnInterval = 25f;    // Расстояние между "секциями"

    private float nextSpawnZ = 30f;      // Где появится первая секция
    private float[] lanes = { -2.5f, 0f, 2.5f }; // Координаты X для трех полос

    void Update()
    {
        if (player == null) return;

        // Если игрок подбегает к точке спавна
        if (player.position.z + spawnDistance > nextSpawnZ)
        {
            SpawnChunk();
            nextSpawnZ += spawnInterval;
        }
    }

    void SpawnChunk()
    {
        // 1. Выбираем случайную полосу для ПРЕПЯТСТВИЯ
        int obstacleLane = Random.Range(0, 3);

        if (obstaclePrefabs.Length > 0)
        {
            GameObject obs = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(obs, new Vector3(lanes[obstacleLane], 0.25f, nextSpawnZ), Quaternion.identity);
        }

        // 2. Выбираем полосу для МОНЕТ, но так, чтобы она НЕ совпадала с препятствием
        int coinLane = Random.Range(0, 3);
        while (coinLane == obstacleLane)
        {
            coinLane = Random.Range(0, 3); // Перевыбираем, если попали в машину
        }

        // 3. Создаем линию из 3 монет на свободной полосе
        for (int i = 0; i < 3; i++)
        {
            // Z - 4f сдвигает монеты чуть вперед, чтобы они не были ровно на линии машины
            Instantiate(coinPrefab, new Vector3(lanes[coinLane], 1f, nextSpawnZ - 4f + (i * 2f)), Quaternion.Euler(90, 0, 0));
        }
    }
}