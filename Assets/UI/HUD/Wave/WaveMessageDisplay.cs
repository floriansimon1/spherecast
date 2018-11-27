using UnityEngine;
using UnityEngine.UI;

public class WaveMessageDisplay: MonoBehaviour {
  private Text     textComponent;
  private Animator levelAnimator;

  void Start() {
    textComponent = GetComponent<Text>();
    levelAnimator = GetComponent<Animator>();
  }

  public void showLevel(int level) {
    textComponent.text = "Wave " + level + '!';

    levelAnimator.Play("Appear");
  }
}
