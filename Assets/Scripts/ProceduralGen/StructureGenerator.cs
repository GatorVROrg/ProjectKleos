using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [Header("Minotaur Entrance")]
    public GameObject MinotaurEntrancePrefab;
    public float MinotaurEntranceMinHeight = 45; // Minimum height of terrain for tree spawning
    public float MinotaurEntranceSpawnRadius = 100f; // Radius from the center of the island to spawn trees

    private List<GameObject> hitObjects = new();
    private GameObject hitObject;

    public void GenerateStructures()
    {
        GenerateMinotaurEntrance();
    }

    public void GenerateMinotaurEntrance()
    {
        MinotaurEntrancePrefab = Resources.Load<GameObject>("Structures/BossStructures/MinotaurEntrance");

        Vector3 islandCenter = transform.position; // Center of the island

        // Loop to spawn trees
        for (int i = 0; i < 2; i++)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * MinotaurEntranceSpawnRadius;

            // Cast a ray to find the terrain height at the random position
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                hitObject = hit.transform.gameObject;

                // Check if terrain height is within desired range
                if (hit.point.y >= MinotaurEntranceMinHeight && !hitObjects.Contains(hit.transform.gameObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                        // Spawn the tree
                        GameObject tree = Instantiate(MinotaurEntrancePrefab, hit.point, MinotaurEntrancePrefab.transform.rotation);
                        tree.transform.parent = transform;
                }
            }
        }
        hitObjects.Add(hitObject);
    }
}