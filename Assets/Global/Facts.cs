using UnityEngine;

public class Facts {
  // Not meant to be instantiated.
  private Facts() {}

  public static readonly Vector3 up                    = new Vector3(0, 1, 0);
  public static readonly Vector3 origin                = Vector3.zero;
  public static readonly float   worldSphereRadius     = 800.0f;
  public static readonly float   portalDisplayDistance = 150.0f;
  public static readonly float   asteroidShakeDistance = 150.0f;
}
