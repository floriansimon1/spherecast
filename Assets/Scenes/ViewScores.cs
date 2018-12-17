using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ViewScores: NoisyClickableText {
	protected override void onClick() {
		SceneManager.LoadScene("High scores");
	}
}
