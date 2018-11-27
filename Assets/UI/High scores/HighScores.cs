using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HighScores: MonoBehaviour {
	private static HighScores instance;

	public List<FinalScore> scores;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);

			instance = this;
		}
	}

	void Start() {
		scores = new List<FinalScore>();

		hydrateScores();
	}

	void hydrateScores() {
		for (var i = 0; i < 7; i++) {
			scores.Add(new FinalScore { points = 999, name = "AAA" } );
		}
	}

	// Must be called after having switched scenes.
	public void registerScore(FinalScore score) {
		var index = scores.FindLastIndex(topScore => {
			return topScore.points > score.points;
		});

		// All scores are higher, therefore this is not a high score.
		if (index < scores.Count - 1) {
			scores.Insert(index + 1, score);
		}

		var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

		foreach (var rootObject in rootGameObjects) {
			ExecuteEvents.Execute<HighScoreRefreshReceiver>(rootObject, null, (target, _) => {
				target.refreshScores(scores);
			});
		}
	}
}
