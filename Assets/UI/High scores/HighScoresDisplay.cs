using UnityEngine;
using System.Linq;
using UnityEngine.UI;

using System.Collections.Generic;

struct ScoreWithText {
	public Text       component;
	public FinalScore score;
}

public class HighScoresDisplay: MonoBehaviour, HighScoreRefreshReceiver {
	// Bad, but it works: https://youtu.be/J6FfcJpbPXE?t=1827
	private static HighScoresDisplay instance;

	public List<Text>       scoreComponents;
	public List<FinalScore> scores;

	void Start() {
		scores = new List<FinalScore>();
	}

	public void refreshScores(List<FinalScore> scores) {
		var zipped = scores.Zip(scoreComponents, (first, second) => new ScoreWithText {
			score     = first,
			component = second
		});

		for (var i = 0; i < zipped.Count(); i++) {
			var scoreWithText = zipped.ElementAt(i);

			scoreWithText.component.text = (
				(i + 1) + ". "
				+ scoreWithText.score.name
				+ " ........................ "
				+ scoreWithText.score.points.ToString().PadLeft(14, '0')
			);
		}
	}
}
