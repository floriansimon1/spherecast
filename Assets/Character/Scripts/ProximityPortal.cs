using UnityEngine;

public class ProximityPortal: MonoBehaviour {
	public float circleDiameter = 15.0f;

	private Vector3 minimumScale = new Vector3(0.1f, 0.01f, 0.1f);
	private Vector3 maximumScale = new Vector3(1.0f, 0.01f, 1.0f);

	private Transform meshTransform;
	private Transform cameraTransform;
	private Transform portalTransform;

	void Start() {
		portalTransform = transform.Find("Portal");

		meshTransform   = portalTransform.Find("Portal mesh");
		cameraTransform = portalTransform.Find("Portal camera");

		maximumScale *= circleDiameter;
	}

	void Update() {
		var closeness = Warp.closenessFactor(transform.position, Facts.portalDisplayDistance);

		if (closeness == 0.0f) {
			portalTransform.gameObject.SetActive(false);

			return;
		}

		portalTransform.gameObject.SetActive(true);

		meshTransform.localScale           = Vector3.Lerp(minimumScale, maximumScale, closeness);
		meshTransform.transform.position   = WorldSphere.projectPoint(transform.position);
		cameraTransform.transform.position = Warp.warp(portalTransform.position, 0.0f);

		cameraTransform.transform.LookAt(DistanceFromCenter.center, Facts.up);
		meshTransform.transform.LookAt(DistanceFromCenter.center, Facts.up);

		meshTransform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 90.0f);
	}
}
