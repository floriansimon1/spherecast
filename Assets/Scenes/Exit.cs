using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit: NoisyClickableText {
	protected override void onClick() {
		#if UNITY_EDITOR
    	UnityEditor.EditorApplication.isPlaying = false;
    #else
    	Application.Quit();
    #endif
	}
}
