using UnityEngine;

public class Asteroid: LevelEntity {
	public Conductor conductor;

	void OnCollisionEnter(Collision collision) {
		var damageable = collision.gameObject.GetComponent<DamageableByAsteroid>();

		if (damageable) {
			damageable.registerAsteroidHit();
		}
	}
}
