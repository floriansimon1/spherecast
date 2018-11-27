using UnityEngine;

public class LogIntersection: MonoBehaviour {
	void Start() {
		var direction = new Vector3(1, 1, 0);

		var maybe = WorldSphere.intersection(Vector3.zero, direction);

		if (maybe.valuePresent) {
			var intersection = maybe.get();

			Debug.Log("Intersection: (" + intersection.x + ", " + intersection.y + ", " + intersection.z + ")");
		}
	}
}
