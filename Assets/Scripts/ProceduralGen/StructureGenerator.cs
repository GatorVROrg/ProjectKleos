using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [Header("Minotaur Entrance")]
    public GameObject MinotaurEntrancePrefab;
    public float MinotaurEntranceMinHeight = 45f; // Minimum height of terrain for spawning
    public float MinotaurEntranceSpawnRadius = 100f; // Radius for spawn location
    public float maxSlopeAngle = 5f; // Maximum allowed slope (degrees) for flatness
    public int maxAttempts = 10; // Maximum number of attempts before giving up

    private List<GameObject> hitObjects = new();
    private GameObject hitObject;

    private VegetationGenerator vegetationGenerator;
    private List<GameObject> vegetation = new();

    public void GenerateStructures()
    {
        vegetationGenerator = GetComponent<VegetationGenerator>();
        vegetation = vegetationGenerator.getVegetation(); // Assuming this gets all the generated vegetation objects
        GenerateMinotaurEntrance();
    }

    public void GenerateMinotaurEntrance()
    {
        MinotaurEntrancePrefab = Resources.Load<GameObject>("Structures/BossStructures/MinotaurEntrance");
        Vector3 islandCenter = transform.position; // Center of the island

        int attempts = 0;
        bool placed = false;

        while (attempts < maxAttempts && !placed)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * MinotaurEntranceSpawnRadius;

            // Raycast to terrain to get the height at the random point
            if (Physics.Raycast(new Vector3(randomPoint.x, 1000f, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                hitObject = hit.transform.gameObject;

                // Check if terrain height is within the desired range and on the correct layer
                if (hit.point.y >= MinotaurEntranceMinHeight && !hitObjects.Contains(hitObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Check if the terrain is flat enough for the structure
                    if (IsFlatArea(hit.point, MinotaurEntrancePrefab))
                    {
                        // Destroy trees or vegetation that overlap with the structure's placement area
                        DestroyNearbyVegetation(hit.point, MinotaurEntrancePrefab);

                        // Spawn the structure if the area is flat
                        GameObject entrance = Instantiate(MinotaurEntrancePrefab, hit.point, MinotaurEntrancePrefab.transform.rotation);
                        entrance.transform.parent = transform;
                        placed = true; // Mark as placed to stop further attempts
                    }
                }
            }

            attempts++; // Increment the number of attempts
        }

        if (hitObject != null)
        {
            hitObjects.Add(hitObject);
        }
    }

    // Helper method to check if the area is flat enough
    private bool IsFlatArea(Vector3 centerPoint, GameObject structure)
    {
        float halfWidth = structure.GetComponentInChildren<Renderer>().bounds.extents.x;
        float halfDepth = structure.GetComponentInChildren<Renderer>().bounds.extents.z;

        // Sample points at the edges of the structure's bounds
        Vector3[] pointsToCheck = new Vector3[]
        {
            centerPoint + new Vector3(halfWidth, 0, halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, halfDepth),
            centerPoint + new Vector3(halfWidth, 0, -halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, -halfDepth)
        };

        float centerHeight = TerrainHeightAtPoint(centerPoint);

        foreach (var point in pointsToCheck)
        {
            float pointHeight = TerrainHeightAtPoint(point);
            float slope = Mathf.Abs(centerHeight - pointHeight);

            // Convert slope to degrees
            float slopeAngle = Mathf.Atan(slope / halfWidth) * Mathf.Rad2Deg;

            if (slopeAngle > maxSlopeAngle)
            {
                return false; // Not flat enough
            }
        }

        return true; // Flat enough
    }

    // Helper method to destroy nearby vegetation (trees) within the structure's bounds
    private void DestroyNearbyVegetation(Vector3 structureCenter, GameObject structure)
    {

        // Ensure the structure has a Renderer
        Renderer structureRenderer = structure.GetComponentInChildren<Renderer>();

        float structureRadius = Mathf.Max(structureRenderer.bounds.extents.x, structureRenderer.bounds.extents.z);

        // Loop through all vegetation objects
        foreach (var vegetation in vegetation)
        {

            // Check the distance between the vegetation object and the structure center
            float distance = Vector3.Distance(structureCenter * structure.transform.localScale.x, vegetation.transform.position);
            if (distance < structureRadius)
            {
                // Destroy the vegetation object if it is within the structure's bounds
                Destroy(vegetation);
            }
        }
    }


    // Helper method to get the terrain height at a given point
    private float TerrainHeightAtPoint(Vector3 point)
    {
        if (Physics.Raycast(new Vector3(point.x, 1000f, point.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point.y;
        }
        return float.MinValue; // Return a default value if raycast fails
    }
}
