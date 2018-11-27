using UnityEngine;

public class WasdControl: MonoBehaviour {
  private AudioSource   accelerationSound;
  private Rigidbody     rigidBody;

  public float acceleration           = 700.0f;
  public float speedCap               = 80.0f;
  public float lowVelocityThreshold   = 40.0f;
  public float brakeDeceleration      = 2.7f;
  public float oppositeMovementWeight = 2.0f;
  
	void Start() {
    accelerationSound = GetComponents<AudioSource>()[3];
    rigidBody         = GetComponent<Rigidbody>();
	}

  float getMovementWeight(Vector3 direction) {
    int angleBetweenDirections = (int) Vector3.Angle(rigidBody.velocity, direction);

    var angleScale = ((angleBetweenDirections + 360) % 180) / 180.0f;

    return 1.0f + (oppositeMovementWeight - 1.0f) * angleScale;
  }

	void Update() {
    var movement = transform.TransformDirection(
      new Vector3(
        Input.GetAxis("Horizontal"),
        0,
        Input.GetAxis("Vertical")
      )
    );

    // Jump controls the world's Y axis movement
    movement.y += Input.GetAxis("Jump");
    
    movement.Normalize();

    rigidBody.AddForce(movement * acceleration * Time.deltaTime * getMovementWeight(movement));

    if (rigidBody.velocity.magnitude < lowVelocityThreshold) {
      if (accelerationSound.isPlaying) {
        // accelerationSound.Stop();
      }
    } else {
      if (!accelerationSound.isPlaying) {
        // accelerationSound.Play();
      }

      rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, speedCap);
    }

    if (Input.GetButton("Brake")) {
      rigidBody.velocity *= (1.0f - Time.deltaTime * brakeDeceleration);
    }
  }
}
