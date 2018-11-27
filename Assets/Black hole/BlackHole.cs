using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BlackHole: MonoBehaviour {
	public float pullFactor = 10.0f;

	private float deathRadius;

	void Start() {
		var components = new List<SphereCollider>();
		
		GetComponents<SphereCollider>(components);

		deathRadius = components.Min(component => component.radius) * transform.localScale.x;
	}

	void OnTriggerEnter(Collider aspiredObject) {
		var pull = aspiredObject.GetComponent<GravityPull>();

		if (!pull) {
			return;
		}

		if (Vector3.Distance(transform.position, pull.transform.position) > deathRadius + 50.0f) {
			return;
		}

		pull.onCompleteAspiration(transform.position);
	}

	void OnTriggerStay(Collider attractedObject) {
		var pull = attractedObject.GetComponent<GravityPull>();

		if (!pull) {
			return;
		}

		pull.onDeviation();

		var body = attractedObject.GetComponent<Rigidbody>();

		var toCenter = transform.position - attractedObject.transform.position;

		body.AddForce(toCenter * Time.deltaTime * pullFactor / body.mass);
	}
}
