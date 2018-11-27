using UnityEngine;
using System.Linq; 
using System.Collections.Generic;

public class PrepareDebugWorldSphere: MonoBehaviour {
	void Awake() {
		transform.localScale *= (Facts.worldSphereRadius * 2.0f + 0.1f);

		var mesh = GetComponent<MeshFilter>().mesh;

		// Makes the sphere hollow.
		mesh.triangles = mesh.triangles.Reverse().ToArray();
		mesh.normals = mesh.normals.Select(o => -o).ToArray();
	}
}
