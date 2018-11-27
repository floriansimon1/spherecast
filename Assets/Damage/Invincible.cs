using UnityEngine;
using System.Collections;

public class Invincible: MonoBehaviour {
  public float duration = 2.0f;

  public delegate void OnInvincibilityEndCallback();

  public OnInvincibilityEndCallback onInvincibilityEndCallback; 

  void Start() {
    Destroy(this, duration);
  }

  void OnDestroy() {
    if (onInvincibilityEndCallback != null) {
      onInvincibilityEndCallback();
    }
  }
}
