using UnityEngine;
using System;

public class Health: MonoBehaviour {
  public int maximum;

  [HideInInspector]
  public int current;

  void Start() {
    current = maximum;
  }

  public Health decreaseMaximum() {
    return setMaximum(Math.Max(0, maximum - 1));
  }

  public Health setMaximum(int value) {
    maximum = value;
    current = Math.Min(current, value);

    return this;
  }

  public void regenerate() {
    current = maximum;
  }
}