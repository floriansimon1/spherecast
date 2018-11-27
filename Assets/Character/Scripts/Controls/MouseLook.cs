using UnityEngine;

public class MouseLook: MonoBehaviour {
  public float verticalSensitivity   = 1200.0f;
  public float horizontalSensitivity = 900.0f;
  public float minimumVerticalAngle  = -70.0f;
  public float maximumVerticalAngle  = 70.0f;

  private float xAngle = 0.0f;

  void Start() {
    Rigidbody body = GetComponent<Rigidbody>();

    // If the object is a rigid body, we prevent the physics system from changing its angle.
    if (body != null) {
      body.freezeRotation = true;
    }
  }

  void Update() {
    float horizontalAngleΔ = Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
    float verticalAngleΔ   = Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime * -1;

    var clampedHorizontalAngle = transform.localEulerAngles.y + horizontalAngleΔ;

    xAngle = Mathf.Clamp(
      xAngle + verticalAngleΔ,
      minimumVerticalAngle,
      maximumVerticalAngle
    );

    transform.localEulerAngles = new Vector3(xAngle, clampedHorizontalAngle, 0);
  }
}
