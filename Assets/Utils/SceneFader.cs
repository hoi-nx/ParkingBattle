using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : SingletonMonoBehaviour<SceneFader> {

	[SerializeField]
	private GameObject fadeCanvas;
	private Animator fadeAnim;

	void Start() {
		fadeCanvas = Instantiate(fadeCanvas);
		fadeAnim = fadeCanvas.GetComponentInChildren<Animator>();
	}

	IEnumerator FadeInAnimate(string levelName) {
		fadeCanvas.SetActive(true);
		fadeAnim.Play("FadeIn");
		yield return StartCoroutine(WaitForRealSeconds(0.1f));
		SceneManager.LoadScene(levelName);
		FadeOut();
	}

	IEnumerator FadeOutAnimate() {
		fadeAnim.Play("FadeOut");
		yield return StartCoroutine(WaitForRealSeconds(0.1f));
		fadeCanvas.SetActive(false);
	}

	public void FadeIn(string levelName) {
		StartCoroutine(FadeInAnimate(levelName));
	}

	public void FadeOut() {
		StartCoroutine(FadeOutAnimate());
	}

	IEnumerator WaitForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < (start + time)) {
			yield return null;
		}
	}
}
