using UnityEngine;
using System.Collections.Generic;

public class GravityPull: MonoBehaviour {
  public virtual void onCompleteAspiration(Vector3 attractionCenter) {}
  public virtual void onDeviation()                                  {}
}
