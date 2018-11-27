using UnityEngine;

public class TurnPurpleNearBorders: MonoBehaviour {
  public Color proximityColor = new Color(0.5f, 0.0f, 1.0f);

	private Material material;

  void Start() {
		material = GetComponentInChildren<Renderer>().material;
  }

  void Update() {
		var closeness = Warp.closenessFactor(transform.position, Facts.asteroidShakeDistance);

    material.color = new Color(
      Mathf.Lerp(1.0f, proximityColor.r, closeness),
      Mathf.Lerp(1.0f, proximityColor.g, closeness),
      Mathf.Lerp(1.0f, proximityColor.b, closeness)
    );
  }
}
