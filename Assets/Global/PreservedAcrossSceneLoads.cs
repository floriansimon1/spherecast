using UnityEngine;

public class PreservedAcrossSceneLoads: MonoBehaviour {
	void Start() {
		DontDestroyOnLoad(gameObject);
	}
}
