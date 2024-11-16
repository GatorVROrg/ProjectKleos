using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [Header("Minotaur Entrance")]
    public GameObject MinotaurEntrancePrefab;
    public float MinotaurEntranceMinHeight = 49f; // Minimum height of terrain for spawning
    public float MinotaurEntranceSpawnRadius = 600; // Radius for spawn location
    public float maxSlopeAngle = 0f; // Maximum allowed slope (degrees) for flatness

    private List<GameObject> hitObjects = new();
    private GameObject hitObject;

    private VegetationGenerator vegetationGenerator;
    private List<GameObject> vegetation = new();

    public void GenerateStructures()
    {
        vegetationGenerator = GetComponent<VegetationGenerator>();
        vegetation = vegetationGenerator.GetVegetation(); // Assuming this gets all the generated vegetation objects
        GenerateMinotaurEntrance();
    }

    public void GenerateMinotaurEntrance()
    {
        MinotaurEntrancePrefab = Resources.Load<GameObject>("Structures/BossStructures/MinotaurEntrance");
        Vector3 islandCenter = transform.position; // Center of the island

        bool placed = false;
        int numOfShots = 0; 

        while ( numOfShots < 1000 && !placed)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * MinotaurEntranceSpawnRadius;

            // Raycast to terrain to get the height at the random point
            if (Physics.Raycast(new Vector3(randomPoint.x, 500, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(new Vector3(randomPoint.x, 500f, randomPoint.z), Vector3.down * 1000, Color.red, 100f); // Ray showing the hit

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

            numOfShots++;
        }

        if (hitObject != null)
        {
            hitObjects.Add(hitObject);
        }
    }

    // Helper method to check if the area is flat enough
    private bool IsFlatArea(Vector3 centerPoint, GameObject structure)
    {
        float halfWidth = structure.GetComponent<MeshRenderer>().bounds.extents.x + 10;
        float halfDepth = structure.GetComponent<MeshRenderer>().bounds.extents.z + 10; // Corrected to use z instead of y for depth

        // Define the four corner points of the rectangle
        Vector3[] pointsToCheck = new Vector3[]
        {
            centerPoint + new Vector3(halfWidth, 0, halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, halfDepth),
            centerPoint + new Vector3(halfWidth, 0, -halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, -halfDepth)
        };

        // Visualize the rectangle in the Scene view
        for (int i = 0; i < pointsToCheck.Length; i++)
        {
            Vector3 start = pointsToCheck[i];
            Vector3 end = pointsToCheck[(i + 1) % pointsToCheck.Length];  // Loop back to the first point
            Debug.DrawLine(start, end, Color.green, 100f);  // Draw a green line to visualize the bounds
        }

        float pointHeights = 0;
        int numHits = 0; // To count how many valid hits we get

        foreach (var point in pointsToCheck)
        {
            // Raycast from the point downward to check the terrain
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(point.x, point.y, point.z), Vector3.down, out hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // If ray hits something (typically the terrain), add the hit point's Y value
                    pointHeights += hit.point.y;
                    numHits++; // Count this as a valid hit
                    Debug.Log($"Raycast hit at: {hit.point}, height: {hit.point.y}");
                }
            }
            else
            {
                // If no hit, add the original point height (or handle as you see fit)
                pointHeights += point.y;
                Debug.Log($"No hit for point: {point}");
            }
        }

        // Debug log for total heights
        Debug.Log($"Total point heights: {pointHeights}");

        // Check if the average height is above a threshold (e.g., 196)
        if (numHits > 0 && pointHeights / numHits >= 196)
        {
            return true; // Flat enough
        }

        return false; // Not flat enough
    }



    // Helper method to destroy nearby vegetation (trees) within the structure's bounds
    private void DestroyNearbyVegetation(Vector3 structureCenter, GameObject structure)
    {

        // Ensure the structure has a Renderer
        Renderer structureRenderer = structure.GetComponent<Renderer>();

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
}