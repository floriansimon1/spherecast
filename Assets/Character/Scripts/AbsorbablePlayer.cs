using UnityEngine;
using System.Collections.Generic;

public class AbsorbablePlayer: GravityPull {
  private AudioSource aspiredSound;

  public void Start() {
    aspiredSound = GetComponents<AudioSource>()[2];
  }

  public override void onCompleteAspiration(Vector3 attractionCenter) {
    aspiredSound.Play();

    GetComponent<DamageablePlayer>().StartCoroutine("die");
  }
}
