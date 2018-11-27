using UnityEngine;

public class DamageableAsteroid: DamageableByBullet {
	public float      separationShrinkFactor       = 0.7f;
	public float      separationAccelerationFactor = 1.1f;

	public Health      health;
	public AudioSource hitSound;
	public Conductor   conductor;
	public AudioSource explosionSound;
	public GameObject  explosionPrefab;

	private Quaternion[] rotations = {
		Quaternion.Euler(30.0f, 0.0f,    0.0f),
		Quaternion.Euler(-30.0f,  70.0f, 0.0f),
		Quaternion.Euler(-30.0f, -70.0f, 0.0f)
	};

	private uint remainingChildren = 3;

	private DamageableAsteroid asteroidParent;

  void Start() {
		pointsGiven = 150;
    health      = GetComponent<Health>();
	}

  public override void registerBulletHit(Bullet bullet) {
		health.current -= 1;

		if (health.current <= 0) {
			breakAsteroid(bullet);
		} else {
			hitSound.Play();
		}
  }

	private void propagateDestruction() {
		Destroy(gameObject);

		if (asteroidParent) {
			asteroidParent.childDestroyed();
		} else {
			conductor.onVitalEntityDestruction();
		}
	}

	void childDestroyed() {
		remainingChildren -= 1;

		if (remainingChildren == 0) {
			propagateDestruction();
		}
	}

	void breakAsteroid(Bullet bullet) {
		var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

		explosionSound.Play();

		explosion.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);

		health.decreaseMaximum();
		health.regenerate();

		if (health.maximum == 0) {
			propagateDestruction();

			return;
		}

		var bulletDirection = bullet.GetComponent<Rigidbody>().velocity.normalized;
		var newSpeed        = GetComponent<Rigidbody>().velocity.magnitude * separationAccelerationFactor;

		// Before instantiating children so that they copy health/size.
		transform.localScale *= separationShrinkFactor;

		foreach (var rotation in rotations) {
			var asteroid = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;

			var rigidBody          = asteroid.GetComponent<Rigidbody>();
			var damageableAsteroid = asteroid.GetComponent<DamageableAsteroid>();

			damageableAsteroid.blink();

			damageableAsteroid.asteroidParent = this;
			asteroid.transform.parent         = transform.parent;

			rigidBody.velocity = rotation * bulletDirection * newSpeed;

			asteroid.transform.position += rigidBody.velocity.normalized * transform.localScale.x;
		}

		gameObject.SetActive(false);
	}

	void blink() {
    var invincibilityAnimation = GetComponent<Animator>();
		var invincible             = gameObject.AddComponent<Invincible>();

		invincible.onInvincibilityEndCallback = () => {
			invincibilityAnimation.Play("Normal");
		};

		invincibilityAnimation.Play("Blink");
	}
}
