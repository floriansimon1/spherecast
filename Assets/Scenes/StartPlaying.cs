using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlaying: NoisyClickableText {
	protected override void onClick() {
		SceneManager.LoadScene("Game scene");
	}
}
