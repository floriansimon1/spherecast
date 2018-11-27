using UnityEngine;

public class Bullet: GravityPull {
  private ScoreEntity scoreIncreaseEntity;
  private Rigidbody   rigidBody;

  public int pointsGiven { get; private set; }

  public float speed      = 500.0f;
	public float timeToLive = 5.0f;

  void Awake() {
    pointsGiven = 150;
    rigidBody   = GetComponent<Rigidbody>();

    Destroy(gameObject, timeToLive);
  }

  public void scoresPointsFor(ScoreEntity entity) {
    scoreIncreaseEntity = entity;
  }

  public override void onDeviation() {
    pointsGiven = 300;
  }

  // Must be called after the instantiation.
  public void propel(Vector3 from, Vector3 direction) {
    transform.position = from;

    rigidBody.velocity = direction.normalized * speed;
  }

  void OnTriggerEnter(Collider hitObject) {
    var damageable = hitObject.GetComponent<DamageableByBullet>();
    var invincible = hitObject.GetComponent<Invincible>();

    if (hitObject.isTrigger) {
      return;
    }

    if (damageable && !invincible) {
      scoreIncreaseEntity.scoreHit(hitObject.transform.position, this);

      damageable.registerBulletHit(this);
    }

    Destroy(gameObject);
  }
}
