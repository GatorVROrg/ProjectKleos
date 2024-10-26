using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    public Transform trackedTransform;
    public GameObject planePrefab;
    public int gridSize = 5; // Initial grid size
    public int viewDistance = 5; // Distance to keep tiles active

    private Vector3Int lastGridPosition;
    private Transform[,] grid;

    void Start()
    {
        grid = new Transform[gridSize, gridSize];
        lastGridPosition = WorldToGrid(trackedTransform.position);
        GenerateInitialGrid();
    }

    void Update()
    {
        Vector3Int currentGridPosition = WorldToGrid(trackedTransform.position);

        // Check if the transform has moved to a new grid cell
        if (currentGridPosition != lastGridPosition)
        {
            UpdateGrid(currentGridPosition);
            lastGridPosition = currentGridPosition;
        }
    }

    void GenerateInitialGrid()
    {
        // Calculate the offset to position the transform at the center of the grid
        Vector3 offset = new Vector3((gridSize - 1) * 0.5f, 0, (gridSize - 1) * 0.5f);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                Vector3 worldPosition = GridToWorld(gridPosition) - offset; // Apply the offset
                GameObject plane = Instantiate(planePrefab, worldPosition, Quaternion.identity);
                grid[x, z] = plane.transform;
            }
        }
    }

    void UpdateGrid(Vector3Int currentGridPosition)
    {
        Vector3Int diff = currentGridPosition - lastGridPosition;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3Int newGridPosition = lastGridPosition + new Vector3Int(x, 0, z);
                Vector3 worldPosition = GridToWorld(newGridPosition);
                float distanceToTransform = Vector3.Distance(worldPosition, trackedTransform.position);

                if (distanceToTransform <= viewDistance)
                {
                    if (grid[x, z] == null && Vector3.Dot(diff, new Vector3(x, 0, z)) > 0)
                    {
                        GameObject plane = Instantiate(planePrefab, worldPosition, Quaternion.identity);
                        grid[x, z] = plane.transform;
                    }
                    else if (grid[x, z] != null && Vector3.Dot(diff, new Vector3(x, 0, z)) <= 0)
                    {
                        Destroy(grid[x, z].gameObject);
                        grid[x, z] = null;
                    }
                }
            }
        }
    }

    Vector3Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x);
        int z = Mathf.FloorToInt(worldPosition.z);
        return new Vector3Int(x, 0, z);
    }

    Vector3 GridToWorld(Vector3Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z);
    }
}
