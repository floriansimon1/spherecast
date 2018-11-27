using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayText: MonoBehaviour, ScoreDisplay {
  private Text scoreTextComponent;

  void Start() {
    scoreTextComponent = GetComponent<Text>();
  }

  public void displayScore(ScoreEntity entity) {
    scoreTextComponent.text = entity.score.ToString().PadLeft(14, '0');
  }  
}
