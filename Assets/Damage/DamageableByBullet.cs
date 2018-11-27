using UnityEngine;

public abstract class DamageableByBullet: MonoBehaviour {
  public int pointsGiven { get; protected set; }

  public abstract void registerBulletHit(Bullet bullet);
}
