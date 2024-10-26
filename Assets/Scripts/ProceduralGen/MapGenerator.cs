using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public Noise.NormalizeMode normalizeMode;

	public const int mapChunkSize = 481;
	[Range(0,6)]
	public int editorPreviewLOD;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool useFalloff;
	public float fallOffStart;
	public float fallOffEnd;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	float[,] falloffMap;

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	void Awake() {
		Vector2Int Size = new Vector2Int(mapChunkSize, mapChunkSize);
		falloffMap = FalloffGenerator.GenerateFalloffMap (Size, fallOffStart, fallOffEnd);
	}

	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		// Generate random values for falloff parameters
		float randomFallOffStart = UnityEngine.Random.Range(0.5f, 0.7f);
		float randomFallOffEnd = UnityEngine.Random.Range(0.1f, 0.3f);

		// Start the thread with the generated random values
		ThreadStart threadStart = delegate {
			MapDataThread (centre, randomFallOffStart, randomFallOffEnd, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MapDataThread(Vector2 centre, float randomFallOffStart, float randomFallOffEnd, Action<MapData> callback) {
		MapData mapData = GenerateMapData (centre, randomFallOffStart, randomFallOffEnd);
		lock (mapDataThreadInfoQueue) {
			mapDataThreadInfoQueue.Enqueue (new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
		ThreadStart threadStart = delegate {
			MeshDataThread (mapData, lod, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
		MeshData meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
		lock (meshDataThreadInfoQueue) {
			meshDataThreadInfoQueue.Enqueue (new MapThreadInfo<MeshData> (callback, meshData));
		}
	}

	void Update() {
		if (mapDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}
	}

	MapData GenerateMapData(Vector2 centre, float randomFallOffStart, float randomFallOffEnd) {
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);
		
		// Generate falloff map using random values
		Vector2Int Size = new Vector2Int(mapChunkSize, mapChunkSize);
		falloffMap = FalloffGenerator.GenerateFalloffMap(Size, randomFallOffStart, randomFallOffEnd);
		
		// Apply falloff map to noise map if useFalloff is true
		if (useFalloff) {
			for (int y = 0; y < mapChunkSize; y++) {
				for (int x = 0; x < mapChunkSize; x++) {
					noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
				}
			}
		}

		// Return only the height map, without assigning colors
		return new MapData(noiseMap);
	}

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
		// Vector2Int Size = new Vector2Int(mapChunkSize, mapChunkSize);
		// falloffMap = FalloffGenerator.GenerateFalloffMap (Size, fallOffStart, fallOffEnd);
	}

	struct MapThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo (Action<T> callback, T parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}

	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}

public struct MapData {
	public readonly float[,] heightMap;

	public MapData (float[,] heightMap)
	{
		this.heightMap = heightMap;
	}
}

