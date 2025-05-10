using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 1.5f;
    [SerializeField] private float maxSpawnTime = 3.5f;

    [Header("Obstacle Settings")]
    [SerializeField] private Vector3 startPosition = new Vector3(-10, 0, 0);  
    [SerializeField] private Vector3 endPosition = new Vector3(10, 0, 0);  
    [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0);   
    [SerializeField] private float moveSpeed = 5f;

    private bool isSpawning = false;

    void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;

        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            SpawnObstacle();
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is missing!");
            return;
        }

        GameObject obstacle = Instantiate(obstaclePrefab, startPosition, Quaternion.identity);

        ObstacleMove obstacleMove = obstacle.GetComponent<ObstacleMove>();
        if (obstacleMove != null)
        {
            obstacleMove.Initialize(moveDirection, endPosition);
        }
    }
}
