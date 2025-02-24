using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    [Header("Trees")]
    public List<GameObject> CypressTreePrefabs;
    public List<GameObject> OliveTreePrefabs;

    [Header("Rocks and Stones")]
    public List<GameObject> StonePrefabs;
    public List<GameObject> RockPrefabs;

    [Header("Generation Parameters")]
    public int MaxCypressTrees = 2000;
    public int MaxOliveTrees = 4000;
    public int MaxStones = 4000;
    public int MaxRocks = 1000;

    public float TreeSpawnRadius = 2000f;

    public float MinTreeHeight = 20f;
    public float MaxCypressHeight = 100f;
    public float MaxOliveHeight = 80f;

    public float MinStoneHeight = 1f;
    public float MaxStoneHeight = 25f;
    public float MinRockHeight = 20f;
    public float MaxRockHeight = 90f;

    private List<GameObject> vegetation = new();
    
    public void Start()
    {
        CypressTreePrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Trees/Cypresses/Cypress1"),
            Resources.Load<GameObject>("Trees/Cypresses/Cypress2"),
            Resources.Load<GameObject>("Trees/Cypresses/Cypress3")
        };
        OliveTreePrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Trees/Olives/Olive1"),
            Resources.Load<GameObject>("Trees/Olives/Olive2"),
            Resources.Load<GameObject>("Trees/Olives/Olive3")
        };
        StonePrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Rocks/Stones/Stones1"),
            Resources.Load<GameObject>("Rocks/Stones/Stones2"),
            Resources.Load<GameObject>("Rocks/Stones/Stones3")
        };
        RockPrefabs = new List<GameObject>
        {
            // Add prefabs to the treePrefabs list
            Resources.Load<GameObject>("Rocks/Rocks/Rock1"),
            Resources.Load<GameObject>("Rocks/Rocks/Rock2"),
            Resources.Load<GameObject>("Rocks/Rocks/Rock3")
        };

        GenerateVegetationOfType(CypressTreePrefabs, MaxCypressTrees, MinTreeHeight, MaxCypressHeight, "Cypress", 4);
        GenerateVegetationOfType(OliveTreePrefabs, MaxOliveTrees, MinTreeHeight, MaxOliveHeight, "Olive", 1);
        GenerateVegetationOfType(StonePrefabs, MaxStones, MinStoneHeight, MaxStoneHeight, "Stone", 1);
        GenerateVegetationOfType(RockPrefabs, MaxRocks, MinRockHeight, MaxRockHeight, "Rock", 1);
    }

    private void GenerateVegetationOfType(List<GameObject> prefabs, int maxCount, float minHeight, float maxHeight, string tag, int clusterSize)
    {
        Vector3 islandCenter = transform.position;

        for (int i = 0; i < maxCount; i++)
        {
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * TreeSpawnRadius;

            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit))
            {
                if (hit.point.y >= minHeight && hit.point.y <= maxHeight && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (tag == "Olive" && IsTooCloseToOtherVegetation(randomPoint, "Cypress", 10f))
                    {
                        continue;
                    }

                    for (int j = 0; j < clusterSize; j++)
                    {
                        Vector3 offset = (clusterSize > 1) ? new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)) : Vector3.zero;
                        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
                        GameObject instance = Instantiate(prefab, hit.point + offset, prefab.transform.rotation);
                        instance.transform.parent = transform;
                        vegetation.Add(instance);
                    }
                }
            }
        }
    }

    private bool IsTooCloseToOtherVegetation(Vector3 position, string tag, float minDistance)
    {
        GameObject[] nearbyVegetation = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in nearbyVegetation)
        {
            if (Vector3.Distance(position, obj.transform.position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetVegetation()
    {
        return vegetation;
    }
}
