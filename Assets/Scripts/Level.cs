using UnityEngine;

public class Level : MonoBehaviour
{
    [Range(1f, 100f)]
    [SerializeField]
    private int obstacleSpawnStep;
    [Range(1f, 100f)]
    [SerializeField]
    private int objectSpawnStep;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject obstacle1Prefab;
    [SerializeField]
    private GameObject obstacle2Prefab;
    [SerializeField]
    private GameObject bonusPrefab;
    [SerializeField]
    private GameObject penaltyPrefab;

    [Header("References")]
    [SerializeField]
    private Transform obstaclesHolder;
    [SerializeField]
    private Transform objectsHolder;

    private void Start()
    {
        //Генерация препятствий и бонусов и штрафов в промежутках между ними.
        for (int i = 1; i < 1000 / obstacleSpawnStep; i++)
        {
            int side = Random.Range(0, 3);
            switch (side)
            {
                case 0:
                    Instantiate(obstacle2Prefab, new Vector3(-2f, 0.5f, i * obstacleSpawnStep), Quaternion.identity, obstaclesHolder);
                    break;
                case 1:
                    Instantiate(obstacle2Prefab, new Vector3(2f, 0.5f, i * obstacleSpawnStep), Quaternion.identity, obstaclesHolder);
                    break;
                case 2:
                    Instantiate(obstacle1Prefab, new Vector3(-3.5f, 0.5f, i * obstacleSpawnStep), Quaternion.identity, obstaclesHolder);
                    Instantiate(obstacle1Prefab, new Vector3(3.5f, 0.5f, i * obstacleSpawnStep), Quaternion.identity, obstaclesHolder);
                    break;
            }
            for (int j = 1; j < obstacleSpawnStep / objectSpawnStep; j++)
            {
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        Instantiate(bonusPrefab, new Vector3(Random.Range(-4, 4), 0.5f, i * obstacleSpawnStep + j * objectSpawnStep), Quaternion.identity, objectsHolder);
                        break;
                    case 1:
                        Instantiate(penaltyPrefab, new Vector3(Random.Range(-4, 4), 0.5f, i * obstacleSpawnStep + j * objectSpawnStep), Quaternion.identity, objectsHolder);
                        break;
                }
            }
        }
    }
}