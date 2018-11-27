using UnityEngine;

public class CelestialBody: MonoBehaviour {
	public float minimumRotationSpeed = 100f;
	public float maximumRotationSpeed = 700f;

	private float   rotationSpeed;
	private Vector3 axis;

	void Start() {
		rotationSpeed = UnityEngine.Random.Range(minimumRotationSpeed, maximumRotationSpeed);

		axis = new Vector3(
			UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f, 0.0f)
		);

		axis.Normalize();
	}

	void Update() {
		transform.Rotate(axis, rotationSpeed * Time.deltaTime);
	}
}
