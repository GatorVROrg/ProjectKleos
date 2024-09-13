using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    [Header("Cypress Trees")]
    public List<GameObject> CypressTreePrefabs; // Array of tree prefabs to choose from
    public int CypressMaxTrees = 100; // Maximum number of trees to spawn
    public float CypressMinTreeHeight = 20f; // Minimum height of terrain for tree spawning
    public float CypressMaxTreeHeight = 50f; // Maximum height of terrain for tree spawning
    public float CypressSpawnRadius = 480f; // Radius from the center of the island to spawn trees
    public int CypressClusterSize = 4; // Number of trees to spawn in each cluster

    [Header("Olive Trees")]
    public List<GameObject> OliveTreePrefabs; // Array of tree prefabs to choose from
    public int OliveMaxTrees = 200; // Maximum number of trees to spawn
    public float OliveMinTreeHeight = 20f; // Minimum height of terrain for tree spawning
    public float OliveMaxTreeHeight = 40f; // Maximum height of terrain for tree spawning
    public float OliveSpawnRadius = 480f; // Radius from the center of the island to spawn trees

    [Header("Stones")]
    public List<GameObject> StonesPrefabs; // Array of stones prefabs to choose from
    public int StonesMax = 2000; // Maximum number of stones to spawn
    public float StonesMinHeight = 10f; // Minimum height of terrain for stones spawning
    public float StonesMaxHeight = 25f; // Maximum height of terrain for stones spawning
    public float StonesSpawnRadius = 500f; // Radius from the center of the island to spawn stones

    [Header("Rocks")]
    public List<GameObject> RocksPrefabs; // Array of rock prefabs to choose from
    public int RocksMax = 500; // Maximum number of rocks to spawn
    public float RocksMinHeight = 35f; // Minimum height of terrain for rock spawning
    public float RocksMaxHeight = 45f; // Maximum height of terrain for rock spawning
    public float RocksSpawnRadius = 480f; // Radius from the center of the island to spawn rocks

    private List<GameObject> hitObjects = new();
    private GameObject hitObject;

    public void GenerateVegetation()
    {
        GenerateRocks();
        GenerateStones();
        GenerateOliveTrees();
        GenerateCypressTrees();
    }

    public void GenerateCypressTrees()
    {
        CypressTreePrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Trees/Cypresses/Cypress1"),
            Resources.Load<GameObject>("Trees/Cypresses/Cypress2"),
            Resources.Load<GameObject>("Trees/Cypresses/Cypress3")
        };

        Vector3 islandCenter = transform.position; // Center of the island

        // Loop to spawn trees
        for (int i = 0; i < CypressMaxTrees; i++)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * CypressSpawnRadius;

            // Cast a ray to find the terrain height at the random position
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                hitObject = hit.transform.gameObject;

                // Check if terrain height is within desired range
                if (hit.point.y >= CypressMinTreeHeight && hit.point.y <= CypressMaxTreeHeight && !hitObjects.Contains(hit.transform.gameObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Randomly choose a tree prefab
                    GameObject treePrefab = CypressTreePrefabs[Random.Range(0, CypressTreePrefabs.Count)];

                    // Spawn a cluster of trees
                    for (int j = 0; j < CypressClusterSize; j++)
                    {
                        // Offset the position within the cluster
                        Vector3 clusterOffset = new(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

                        // Spawn the tree
                        GameObject tree = Instantiate(treePrefab, hit.point + clusterOffset, treePrefab.transform.rotation);
                        tree.transform.parent = transform;
                    }
                }
            }
        }
        hitObjects.Add(hitObject);
    }

    public void GenerateOliveTrees()
    {
        OliveTreePrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Trees/Olives/Olive1"),
            Resources.Load<GameObject>("Trees/Olives/Olive2"),
            Resources.Load<GameObject>("Trees/Olives/Olive3")
        };

        Vector3 islandCenter = transform.position; // Center of the island

        // Loop to spawn trees
        for (int i = 0; i < OliveMaxTrees; i++)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * OliveSpawnRadius;

            // Cast a ray to find the terrain height at the random position
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                // Check if terrain height is within desired range
                if (hit.point.y >= OliveMinTreeHeight && hit.point.y <= OliveMaxTreeHeight && !hitObjects.Contains(hit.transform.gameObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Check distance from cypress trees
                    bool tooCloseToCypress = false;
                    GameObject[] cypressTreesInScene = GameObject.FindGameObjectsWithTag("Cypress");
                    foreach (GameObject cypressTree in cypressTreesInScene)
                    {
                        if (Vector3.Distance(randomPoint, cypressTree.transform.position) < 10)
                        {
                            tooCloseToCypress = true;
                            break;
                        }
                    }

                    if (!tooCloseToCypress)
                    {
                        // Randomly choose a tree prefab
                        GameObject treePrefab = OliveTreePrefabs[Random.Range(0, OliveTreePrefabs.Count)];

                        // Spawn the tree
                        GameObject tree = Instantiate(treePrefab, hit.point, treePrefab.transform.rotation);
                        tree.transform.parent = transform;
                    }
                    else
                    {
                        Debug.Log("too fuckin close");
                    }
                }
            }
        }
    }

    public void GenerateStones()
    {
        StonesPrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Rocks/Stones/Stones1"),
            Resources.Load<GameObject>("Rocks/Stones/Stones2"),
            Resources.Load<GameObject>("Rocks/Stones/Stones3")
        };

        Vector3 islandCenter = transform.position; // Center of the island

        // Loop to spawn trees
        for (int i = 0; i < StonesMax; i++)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * StonesSpawnRadius;

            // Cast a ray to find the terrain height at the random position
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                // Check if terrain height is within desired range
                if (hit.point.y >= StonesMinHeight && hit.point.y <= StonesMaxHeight && !hitObjects.Contains(hit.transform.gameObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Randomly choose a tree prefab
                    GameObject StonesPrefab = StonesPrefabs[Random.Range(0, StonesPrefabs.Count)];

                    // Spawn the tree
                    GameObject stones = Instantiate(StonesPrefab, hit.point, StonesPrefab.transform.rotation);
                    stones.transform.parent = transform;
                    float minScale = 5f;
                    float maxScale = 10f;
                    float random = Random.Range(minScale, maxScale);

                    Vector3 randomScale = new(random, random, random);
                    stones.transform.localScale = randomScale;   
                    stones.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);             
                }
            }
        }
    }

    public void GenerateRocks()
    {
        RocksPrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Rocks/Rocks/Rock1"),
            Resources.Load<GameObject>("Rocks/Rocks/Rock2"),
            Resources.Load<GameObject>("Rocks/Rocks/Rock3")
        };

        Vector3 islandCenter = transform.position; // Center of the island

        // Loop to spawn trees
        for (int i = 0; i < RocksMax; i++)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * RocksSpawnRadius;

            // Cast a ray to find the terrain height at the random position
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                // Check if terrain height is within desired range
                if (hit.point.y >= RocksMinHeight && hit.point.y <= RocksMaxHeight && !hitObjects.Contains(hit.transform.gameObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Randomly choose a tree prefab
                    GameObject RocksPrefab = RocksPrefabs[Random.Range(0, RocksPrefabs.Count)];

                    // Spawn the tree
                    GameObject Rock = Instantiate(RocksPrefab, hit.point, RocksPrefab.transform.rotation);
                    Rock.transform.parent = transform;
                    Vector3 minScale = new(50f, 50f, 50f);
                    Vector3 maxScale = new(200f, 200f, 200f);

                    Vector3 randomScale = new(
                        Random.Range(minScale.x, maxScale.x),
                        Random.Range(minScale.y, maxScale.y),
                        Random.Range(minScale.z, maxScale.z)
                    );
                    Rock.transform.localScale = randomScale;
                    Rock.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);
                }
            }
        }
    }

}