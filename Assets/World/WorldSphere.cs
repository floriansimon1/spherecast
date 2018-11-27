using UnityEngine;

public class WorldSphere {
	// Not meant to be instantiated.
	private WorldSphere() {}

	/*
	* Finds the intersection between a ray and a sphere by finding
	* the (positive) distance on the `direction` vector starting from
	* `from` which is on the sphere, that is that solves the sphere
	* equation.
	*/
	public static Optional.Optional<Vector3> intersection(Vector3 from, Vector3 normalizedDirection) {
		/*
		*
		* Equation of a 3D sphere: radius² = (x - center.x)² + (y - center.y)² + (z + center.z)²
		* The center of the world being the origin, our sphere's equation is:
		* radius² = x² + y² + z²
		*
		* Equations of a 3D line:
		* - x = from.x + distance * direction.x
		* - y = from.y + distance * direction.y
		* - z = from.z + distance * direction.z
		*
		* Replacing x, y and z in the sphere equation:
		*
		* from.x² + 2 * from.x * distance + distance² * direction.x²
		* from.y² + 2 * from.y * distance + distance² * direction.y²
		* from.z² + 2 * from.z * distance + distance² * direction.z²
		*
		* radius² = (
		*		  (from.x + distance * direction.x)²
		*		+ (from.y + distance * direction.y)²
		*		+ (from.z + distance * direction.z)²
		* )
		*
		* Remembering vector.x² + vector.y² + vector.z² is the squared magnitude of `vector`, that
		* can be transformed to:
		*
		* radius² = (
		*     direction.magnitude² * distance²
		*   + 2 * (from.x + from.y + from.z) * distance
		*   + from.magnitude²
		* )
		*
		* We get the following quadratic equation:
		*
		* 0 = (
		*     (direction.magnitude²) * distance²
		*   + 2 * (from.x + from.y + from.z) * distance
		*   + (from.magnitude² - radius²)
		* )
		*/
		var a = normalizedDirection.sqrMagnitude;
		var b = 2.0f * (from.x + from.y + from.z);
		var c = from.sqrMagnitude - Mathf.Pow(Facts.worldSphereRadius, 2.0f);

		var bSquared = Mathf.Pow(b, 2.0f);

		var root = bSquared - 4.0f * a * c;

		if (root < 0) {
			return new Optional.None<Vector3>();
		}

		var distance = (-b + Mathf.Sqrt(root)) / (2.0f * a);

		/*
		* If the ray does not hit the sphere (negative distance),
		* try another solution if possible.
		*/
		if (distance <= 0.0f && root > 0.0f) {
			distance = (-b - Mathf.Sqrt(root)) / (2.0f * a);
		}

		/*
		* No solution in any direction... Therefore the ray does not hit the sphere.
		* This means from is outside the sphere and is not directed towards it.
		*/
		if (distance <= 0.0f) {
			return new Optional.None<Vector3>();
		}

		Debug.Log(distance);

		return new Optional.Some<Vector3>(from + normalizedDirection * distance);
	}

	public static Vector3 projectPoint(Vector3 point) {
		// Transforms the point into a direction. Works the center of the world sphere is the origin.
		point.Normalize();

		return point * Facts.worldSphereRadius;
	}
}
