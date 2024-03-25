using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
	public List<Material> environment;

    public void DrawMesh(MeshData meshData) {
        meshFilter.sharedMesh = meshData.CreateMesh();
		meshRenderer.SetMaterials(environment);
    }
}
