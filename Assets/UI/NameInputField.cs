using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NameInputField: MonoBehaviour {
	private InputField field;

	private bool forceInput = true;

	public int                  position;
	public HighScores           highScores;
	public InputField           inputValue;
	public NameInputEndCallback onNameInputEnd;
	public List<Text>           scoreComponents;

	public delegate void NameInputEndCallback(string name);

	public void startForcedInput(List<Text> scoreTexts, int index) {
		scoreComponents = scoreTexts;
		position        = index;
		forceInput      = true;

		this.enabled  = true;
		field.enabled = true;
	}

	private void stopForcedInput() {
		forceInput    = false;
		this.enabled  = false;
		field.enabled = false;
		field.text    = "";
	}

	void Start() {
		field = GetComponent<InputField>();

		stopForcedInput();

		var regexp = new Regex("[^a-z]");

		inputValue = GetComponent<InputField>();

		inputValue.onValueChanged.AddListener(_ => {
			inputValue.text = regexp.Replace(inputValue.text, "");

			if (inputValue.text.Length == 3) {
				var name = inputValue.text;

				stopForcedInput();

				if (onNameInputEnd != null) {
					onNameInputEnd(name);

					onNameInputEnd = null;
				}
			}
		});
	}

	IEnumerator deselectInNextFrame() {
		yield return new WaitForEndOfFrame();

		field.MoveTextEnd(true);
	}

	public void onHighScoresScreenExit() {
		if (onNameInputEnd != null) {
			onNameInputEnd(inputValue.text.PadRight(3, 'a'));
		}
	}

	void Update() {
		if (!forceInput) {
			return;
		}

		if (EventSystem.current.currentSelectedGameObject != gameObject) {
			field.ActivateInputField();

			field.transform.position = new Vector3(
				518.0f,
				scoreComponents[position].transform.position.y + 12.0f,
				0.0f
			);

			StartCoroutine(deselectInNextFrame());
		}
	}
}
