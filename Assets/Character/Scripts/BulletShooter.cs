using UnityEngine;

public class BulletShooter: MonoBehaviour {
  public GameObject spawnedBullet;
  public GameObject bulletsContainer;

  public ScoreEntity scoreEntity;
  public float       delayBetweenShots = 0.1f;
  public float       recoil            = 10.0f;
  public Vector3     spawnPosition     = new Vector3(0.0f, 0.0f, 2.0f);

  private Rigidbody   rigidBody;
  private AudioSource shotSound;
  private float       lastShotTime = 0.0f;

  public void detectDependencies() {
    rigidBody   = GetComponent<Rigidbody>();
    shotSound   = GetComponents<AudioSource>()[0];
    scoreEntity = GetComponent<DamageablePlayer>();
  }

  void Update() {
    // The game is paused!
    if (Time.timeScale == 0.0f) {
      return;
    }

		if (!Input.GetButton("Fire1")) {
      return;
    }

    float newTime = Time.realtimeSinceStartup;

    if (newTime - lastShotTime < delayBetweenShots) {
      return;
    }

    lastShotTime = newTime;

    spawnBullet();
	}

  void spawnBullet() {
    shotSound.Play();

    var spawnAt = transform.TransformPoint(spawnPosition);

    var bulletGameObject = Instantiate(spawnedBullet) as GameObject;

    bulletGameObject.transform.parent = bulletsContainer.transform;

    var bullet = bulletGameObject.GetComponent<Bullet>();

    bullet.scoresPointsFor(scoreEntity);

    bullet.propel(spawnAt, transform.forward);

    var recoilDirection = -transform.forward;

    recoilDirection.Normalize();

    rigidBody.AddForce(recoilDirection * recoil);
  }
}
