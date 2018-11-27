using UnityEngine;

public class DeformAsteroid: MonoBehaviour {
	private float randomScale(float seed, int n) {
		return Mathf.PerlinNoise(n * 0.01f, seed) * 0.6f + 0.7f;
	}

	private Texture2D randomNormalMap() {
		var texture = new Texture2D(512, 512);
		var seed    = generateSeed();

		for (float i = 0; i < texture.width; i++) {
			for (float j = 0; j < texture.width; j++) {
				var normal = new Vector3(
					Mathf.PerlinNoise(seed + i * 0.07f, seed + j * 0.07f),
					Mathf.PerlinNoise(seed + (i + 20.0f) * 0.07f, seed + (j + 20.0f) * 0.07f),
					Mathf.PerlinNoise(seed + (i + 50.0f) * 0.07f, seed + (j + 50.0f) * 0.07f)
				);

				normal.Normalize();

				var x = new Color(
					0.5f + normal.x / 2,
					0.5f + normal.y / 2,
					0.5f + normal.z / 2
				);

				texture.SetPixel((int) i, (int) j, x);
			}
		}

		texture.wrapMode = TextureWrapMode.Repeat;

		texture.Apply();

		return texture;
	}

	private float generateSeed() {
		return Random.Range(0, 1) * 10000;
	}

	public void randomizeGeometry() {
		var seed     = generateSeed();
		var mesh     = GetComponent<MeshFilter>().mesh;
		var material = GetComponent<Renderer>().material;

		// material.SetTexture("_BumpMap", randomNormalMap());

		var vertices = mesh.vertices;

		bool[] recalculated = new bool[vertices.Length];

		for (var i = 0; i < recalculated.Length; i++) {
			recalculated[i] = false;
		}

		for (var i = 0; i < vertices.Length; i++) {
			// This vertex has already been transformed because it was a duplicate.
			if (recalculated[i]) {
				continue;
			}

			var transformedVertex = vertices[i] * randomScale(seed, i);

			// All duplicates need to be changed in the same way.
			for (var j = i + 1; j < vertices.Length; j++) {
				if (vertices[i] == vertices[j]) {
					vertices[j] = transformedVertex;

					recalculated[j] = true;
				}
			}

			/*
			* Finally changing the vertex at position [i]. No need
			* to mark it as calculated because when looking at the
			* vertex n, the duplicates are searched for for vertex
			* n + 1 and up only, we're not searching backwards.
			*/
			vertices[i] = transformedVertex;
		}

		mesh.vertices = vertices;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	void Awake() {
		randomizeGeometry();
	}
}
