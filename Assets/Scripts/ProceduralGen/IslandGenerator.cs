using UnityEngine;
using System.Collections.Generic;

public class SingleIslandTerrain : MonoBehaviour 
{
    const float scale = 5f;

    public LODInfo[] detailLevels;
    public static float maxViewDst;

    public Transform viewer;
    public Material mapMaterial;

    static MapGenerator mapGenerator;
    int chunkSize;

    void Start() 
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        chunkSize = MapGenerator.mapChunkSize - 1;

        // Generate the entire island
        GenerateIsland();
    }

    void GenerateIsland() 
    {
        Vector2 islandCenter = Vector2.zero; // Adjust the center as needed
        TerrainChunk newChunk = new TerrainChunk(islandCenter, chunkSize, detailLevels, transform, mapMaterial);
    }

    public class TerrainChunk 
    {
        public GameObject meshObject;
        public Vector2 position;
        public Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        VegetationGenerator vegetationGenerator;
        StructureGenerator structureGenerator;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        MapData mapData;
        bool mapDataReceived;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;

            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;
            vegetationGenerator = meshObject.AddComponent<VegetationGenerator>();
            structureGenerator = meshObject.AddComponent<StructureGenerator>();
            meshObject.layer = LayerMask.NameToLayer("Ground");

            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(true); // Make it visible since it is the only island

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++) 
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData) 
        {
            this.mapData = mapData;
            mapDataReceived = true;

            UpdateTerrainChunk();
        }

        public void UpdateTerrainChunk() 
        {
            if (mapDataReceived) 
            {
                SetVisible(true); // Always visible since it's a single island

                // LOD handling can be simplified or even removed for a single island
                int lodIndex = 0;

                LODMesh lodMesh = lodMeshes[lodIndex];
                if (lodMesh.hasMesh) 
                {
                    meshFilter.mesh = lodMesh.mesh;
                    meshCollider.sharedMesh = lodMesh.mesh;
                }
                else if (!lodMesh.hasRequestedMesh) 
                {
                    lodMesh.RequestMesh(mapData);
                }

                // Generate vegetation and structures
                vegetationGenerator.GenerateVegetation();
                structureGenerator.GenerateStructures();
            }
        }

        public void SetVisible(bool visible) 
        {
            meshObject.SetActive(visible);
        }
    }

    class LODMesh 
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback) 
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataReceived(MeshData meshData) 
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData) 
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }

    [System.Serializable]
    public struct LODInfo 
    {
        public int lod;
        public float visibleDstThreshold;
    }
}
