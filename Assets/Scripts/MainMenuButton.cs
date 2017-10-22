using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {
	private AudioSource audioSource;

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audioSource = GetComponent<AudioSource>();
	}

	void TaskOnClick()
	{
		StartCoroutine(playSoundThenLoad());
	}

	IEnumerator playSoundThenLoad()
	{
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		SceneManager.LoadScene("Menu");
	}
}
