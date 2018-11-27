using UnityEngine;

public class WarpableEntity: MonoBehaviour, Warpable {
  private Rigidbody rigidBody;

  public float radius = 1.0f;

  virtual public void Start() {
    rigidBody = GetComponent<Rigidbody>();
  }

  void Update() {
		var forward = rigidBody.velocity;

    forward.Normalize();

    if (Warp.shouldWarp(forward, transform.position, radius)) {
      transform.position = Warp.warp(transform.position, radius);

      onWarp();
    }
  }

  virtual public void onWarp() {
  }
}
