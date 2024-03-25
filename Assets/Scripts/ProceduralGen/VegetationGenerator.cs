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

    private List<GameObject> hitObjects = new List<GameObject>();
    private GameObject hitObject;

    public void GenerateVegetation()
    {
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
                //Debug.Log(hit.point.y);
                hitObject = hit.transform.gameObject;

                // Check if terrain height is within desired range
                if (hit.point.y >= CypressMinTreeHeight && hit.point.y <= CypressMaxTreeHeight && !hitObjects.Contains(hit.transform.gameObject))
                {
                    // Randomly choose a tree prefab
                    GameObject treePrefab = CypressTreePrefabs[Random.Range(0, CypressTreePrefabs.Count)];

                    // Spawn a cluster of trees
                    for (int j = 0; j < CypressClusterSize; j++)
                    {
                        // Offset the position within the cluster
                        Vector3 clusterOffset = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

                        // Spawn the tree
                        GameObject tree = Instantiate(treePrefab, hit.point + clusterOffset, treePrefab.transform.rotation);
                        tree.transform.parent = transform; // Set the island as the parent of the tree
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
                if (hit.point.y >= OliveMinTreeHeight && hit.point.y <= OliveMaxTreeHeight && !hitObjects.Contains(hit.transform.gameObject))
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
                        tree.transform.parent = transform; // Set the island as the parent of the tree
                    }
                    else
                    {
                        Debug.Log("too fuckin close");
                    }
                }
            }
        }
    }

}