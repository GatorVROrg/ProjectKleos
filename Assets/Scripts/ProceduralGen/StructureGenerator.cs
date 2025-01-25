using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [Header("Minotaur Entrance")]
    public GameObject MinotaurEntrancePrefab;
    public float MinotaurEntranceMinHeight = 49f; // Minimum height of terrain for spawning
    public float MinotaurEntranceSpawnRadius = 1200; // Radius for spawn location

    [Header("Spartoi Camps")]
    public GameObject SpartoiCampPrefab;
    public float SpartoiCampMinHeight = 49f;
    public float SpartoiCampSpawnRadius = 1200;

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

        while ( numOfShots < 2000 && !placed)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * MinotaurEntranceSpawnRadius;

            // Raycast to terrain to get the height at the random point
            if (Physics.Raycast(new Vector3(randomPoint.x, 500, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
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

            numOfShots++;
        }

        if (hitObject != null)
        {
            hitObjects.Add(hitObject);
        }
    }

    public void GenerateSpartoiCamps()
    {
        SpartoiCampPrefab = Resources.Load<GameObject>("Structures/BossStructures/SpartoiCamp");
        Vector3 islandCenter = transform.position; // Center of the island

        bool placed = false;
        int numOfShots = 0; 

        while (numOfShots < 2000 && !placed)
        {
            // Random point within spawn radius from the center of the island
            Vector3 randomPoint = islandCenter + Random.insideUnitSphere * SpartoiCampSpawnRadius;

            // Raycast to terrain to get the height at the random point
            if (Physics.Raycast(new Vector3(randomPoint.x, 500, randomPoint.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                hitObject = hit.transform.gameObject;

                // Check if terrain height is within the desired range and on the correct layer
                if (hit.point.y >= SpartoiCampMinHeight && !hitObjects.Contains(hitObject) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Check if the terrain is flat enough for the structure
                    if (IsFlatArea(hit.point, SpartoiCampPrefab))
                    {
                        // Destroy trees or vegetation that overlap with the structure's placement area
                        DestroyNearbyVegetation(hit.point, SpartoiCampPrefab);

                        // Spawn the structure if the area is flat
                        GameObject entrance = Instantiate(SpartoiCampPrefab, hit.point, SpartoiCampPrefab.transform.rotation);
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
        float halfDepth = structure.GetComponent<MeshRenderer>().bounds.extents.z + 10;

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
            //Debug.DrawLine(start, end, Color.green, 100f);  // Draw a green line to visualize the bounds
        }

        float pointHeights = 0;
        int numHits = 0;

        foreach (var point in pointsToCheck)
        {
            // Raycast from the point downward to check the terrain
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(point.x, point.y, point.z), Vector3.down, out hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    //Debug.DrawRay(new Vector3(point.x, point.y, point.z), Vector3.down * 1000, Color.red, 100f); // Ray showing the hit

                    // If ray hits something (typically the terrain), add the hit point's Y value
                    pointHeights += hit.point.y;
                    numHits++;
                }
            }
            else
            {
                // If no hit, add the original point height (or handle as you see fit)
                pointHeights += point.y;
            }
        }


        if (pointHeights >= 392)  // Check if the average height is above a threshold
        {   
            //Debug.Log($"Total point heights: {pointHeights}");
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

        // Get the corner points for the area
        Vector3[] cornerPoints = GetStructureCorners(structureCenter, structure);

        // Loop through all vegetation objects
        foreach (var vegetation in vegetation)
        {
            // Check if the vegetation is within the bounds of the four corners
            if (IsVegetationInsideBounds(vegetation.transform.position, cornerPoints))
            {
                // Destroy the vegetation object if it is inside the bounds
                Destroy(vegetation);
            }
        }
    }

    // Helper method to get the four corner points of the structure
    private Vector3[] GetStructureCorners(Vector3 centerPoint, GameObject structure)
    {
        float halfWidth = structure.GetComponent<MeshRenderer>().bounds.extents.x + 10;
        float halfDepth = structure.GetComponent<MeshRenderer>().bounds.extents.z + 10;

        return new Vector3[]
        {
            centerPoint + new Vector3(halfWidth, 0, halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, halfDepth),
            centerPoint + new Vector3(halfWidth, 0, -halfDepth),
            centerPoint + new Vector3(-halfWidth, 0, -halfDepth)
        };
    }

    // Helper method to check if the vegetation is inside the bounds defined by the four corners
    private bool IsVegetationInsideBounds(Vector3 vegetationPosition, Vector3[] cornerPoints)
    {
        // Use a simple bounding box check: the vegetation should be within the minimum and maximum X and Z bounds of the four corners
        float minX = Mathf.Min(cornerPoints[0].x, cornerPoints[1].x, cornerPoints[2].x, cornerPoints[3].x);
        float maxX = Mathf.Max(cornerPoints[0].x, cornerPoints[1].x, cornerPoints[2].x, cornerPoints[3].x);
        float minZ = Mathf.Min(cornerPoints[0].z, cornerPoints[1].z, cornerPoints[2].z, cornerPoints[3].z);
        float maxZ = Mathf.Max(cornerPoints[0].z, cornerPoints[1].z, cornerPoints[2].z, cornerPoints[3].z);

        return vegetationPosition.x >= minX && vegetationPosition.x <= maxX && vegetationPosition.z >= minZ && vegetationPosition.z <= maxZ;
    }
}