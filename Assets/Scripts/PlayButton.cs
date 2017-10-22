using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
	private AudioSource audioSource;

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audioSource = GetComponent<AudioSource>();
	}
		
	void TaskOnClick()
	{
		StartCoroutine(PlaySoundThenLoad());
	}

	IEnumerator PlaySoundThenLoad()
	{
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		SceneManager.LoadScene("LevelSelect");
	}
}
