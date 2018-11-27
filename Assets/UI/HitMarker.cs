using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker: MonoBehaviour {
	public float      timeToLive  = 1.5f;
	public GameObject facedObject;

	private float    remainingTime;
	private float    baseHue;
	private TextMesh mesh;

	void Start() {
		remainingTime = timeToLive;

		mesh = GetComponent<TextMesh>();

		baseHue = Random.Range(0.0f, 1.0f);

		Destroy(gameObject, timeToLive);
	}

	void Update() {
		remainingTime -= Time.deltaTime;

		var remainingTimePercent =  remainingTime / timeToLive;

		var elapsedTimePercent = 1.0f - remainingTimePercent;

		var hue = Mathf.Repeat(baseHue + elapsedTimePercent, 1.0f);

		var color = Color.HSVToRGB(hue, 0.8f, 0.5f);

		color.a = remainingTimePercent;

		mesh.color = color;

		var distance = Vector3.Distance(facedObject.transform.position, transform.position);

		var scale = distance * 0.05f;

		transform.localScale = scale * Vector3.one;

		transform.LookAt(facedObject.transform.position);

		transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

		transform.Translate(new Vector3(0.0f, Time.deltaTime * 0.05f * distance, 0.0f));
	}
}
