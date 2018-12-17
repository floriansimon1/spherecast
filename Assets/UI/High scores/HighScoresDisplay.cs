using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

struct ScoreWithText {
	public Text       component;
	public FinalScore score;
}

public class HighScoresDisplay: MonoBehaviour, HighScoreRefreshReceiver {
	public List<Text> scoreComponents;
	public InputField scoreInput;

	private IEnumerator startNameInput(List<FinalScore> highScores, int nameToInputIndex) {
		yield return new WaitForEndOfFrame();
		
		var nameInputField = scoreInput.GetComponent<NameInputField>();

		nameInputField.startForcedInput(scoreComponents, nameToInputIndex);

		nameInputField.onNameInputEnd = name => {
			var score = highScores[nameToInputIndex];
			
			score.name = name;

			highScores[nameToInputIndex] = score;

			refreshScores(highScores);
		};
	}

	public void refreshScores(List<FinalScore> highScores) {
		var zipped = highScores.Zip(scoreComponents, (first, second) => new ScoreWithText {
			score     = first,
			component = second
		});

		Optional.Optional<int> nameToInput = new Optional.None<int>();

		for (var i = 0; i < zipped.Count(); i++) {
			var scoreWithText = zipped.ElementAt(i);

			if (scoreWithText.score.name.Length == 0) {
				nameToInput = new Optional.Some<int>(i);
			}

			scoreWithText.component.text = (
				(i + 1) + ". "
				+ scoreWithText.score.name.PadLeft(3, ' ')
				+ " ........................ "
				+ scoreWithText.score.points.ToString().PadLeft(14, '0')
			);
		}

		nameToInput.tap(index => {
			StartCoroutine(startNameInput(highScores, index));
		});
	}
}
