using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu: NoisyClickableText {
	public NameInputField nameInputField;

	protected override void onClick() {
		nameInputField.onHighScoresScreenExit();

		SceneManager.LoadScene("Main menu");
	}
}