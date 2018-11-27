using UnityEngine;

public class Utils {
  // Not meant to be instantiated.
  private Utils() {}

  public static Vector3 makeVelocity(Vector3 looseDirection, float scale) {
    return looseDirection.normalized * scale;
  }
}
