using System.Collections.Generic;
using UnityEngine;

public class StartReactors: MonoBehaviour {
	private List<ParticleSystem> emitters;

	private bool destroyed = false;

	void Start() {
		emitters = new List<ParticleSystem>();

		GetComponentsInChildren<ParticleSystem>(emitters);
	}

	public void destroyThrusters() {
		destroyed = true;

		foreach (var emitter in emitters) {
			if (emitter.isPlaying) {
				emitter.Stop();
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (destroyed) {
			return;
		}

		var goingForward = Input.GetAxis("Vertical") > 0.0f;

		foreach (var emitter in emitters) {
			// Avoid calling play when the particle system's already emitting particles.
			if (emitter.isPlaying == goingForward) {
				continue;
			}

			if (goingForward) {
				emitter.Play();
			} else {
				emitter.Stop();
			}
		}
	}
}
