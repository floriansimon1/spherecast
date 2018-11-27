using UnityEngine;

public class Warp {
	// Not instantiable.
	private Warp() {}
	
	public static bool shouldWarp(Vector3 normalizedForward, Vector3 position, float radius) {
		var farthestPosition = position + normalizedForward * radius;

		return DistanceFromCenter.get(farthestPosition) >= Facts.worldSphereRadius;
	}

	public static Vector3 warp(Vector3 position, float radius) {
		return -position.normalized * (Facts.worldSphereRadius - radius);
	}

	public static float closenessFactor(Vector3 position, float zeroingDistance) {
		var distanceFromOuterRim = Facts.worldSphereRadius - DistanceFromCenter.get(position);

		if (distanceFromOuterRim > zeroingDistance) {
			return 0.0f;
		}

		return 1.0f - Mathf.Max(0.0f, distanceFromOuterRim) / zeroingDistance;
	}
}
