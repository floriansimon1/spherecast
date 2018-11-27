using UnityEngine;

public interface ScoreEntity {
  int score { get; }

  void scoreHit(Vector3 position, Bullet bullet);
}
