using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
	public Button button;
	public AudioSource audio;

	void Start()
	{
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
		
	void TaskOnClick()
	{
		StartCoroutine(playSoundThenLoad());
	}

	IEnumerator playSoundThenLoad()
	{
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length - 0.3f);
		SceneManager.LoadScene("LevelSelect");
	}
}
