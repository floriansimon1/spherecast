using UnityEngine;
using UnityEngine.EventSystems;

// Don't forget to call start on children/
public abstract class NoisyClickableText: MonoBehaviour, IPointerDownHandler {
	private AudioSource audioSource;

  protected abstract void onClick();

  void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void OnPointerDown(PointerEventData _) {
		onClick();

		audioSource.Play();
	}
}
