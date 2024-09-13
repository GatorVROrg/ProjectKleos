using UnityEngine;
using System.Collections.Generic;
using TreeEditor;

public class EndlessTerrain : MonoBehaviour 
{
    const float scale = 5f;

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDst;

    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    Vector2 viewerPositionOld;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

    void Start() {
        mapGenerator = FindObjectOfType<MapGenerator> ();

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

        UpdateVisibleChunks();
    }

    void Update() {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

	void UpdateVisibleChunks() 
	{
		// Store previously visible chunks
		List<Vector2> previouslyVisibleChunks = new List<Vector2>(terrainChunkDictionary.Keys);

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) 
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) 
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (!terrainChunkDictionary.ContainsKey(viewedChunkCoord)) 
				{
					TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial);
					terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
				}
				else 
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();

					// Remove this chunk from the list of previously visible chunks
					previouslyVisibleChunks.Remove(viewedChunkCoord);
				}
			}
		}
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
        
        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;

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
            meshObject.layer = LayerMask.NameToLayer("Ground");

            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++) {
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
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible) 
				{
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++) 
					{
						if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold) 
						{
							lodIndex = i + 1;
						}
						else 
						{
							break;
						}
					}

					if (lodIndex != previousLODIndex) 
					{
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh) 
						{
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
							meshCollider.sharedMesh = lodMesh.mesh;
						}
						else if (!lodMesh.hasRequestedMesh) 
						{
							lodMesh.RequestMesh(mapData);
						}
					}
				}		
                vegetationGenerator.GenerateVegetation();
				SetVisible(visible);
			}
		}

        public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
		}

		public bool IsVisible() {
			return meshObject.activeSelf;
		}
    }

    class LODMesh 
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback) {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataReceived(MeshData meshData) {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData) {
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
