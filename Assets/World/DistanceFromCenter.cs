using UnityEngine;

public class DistanceFromCenter {
  public static readonly Vector3 center = Vector3.zero;

  // Not instantiable.
  private DistanceFromCenter() {}

  // Works because our center is the world origin.
  public static float get(Vector3 point) {
    return point.magnitude;
  }
}