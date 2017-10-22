using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour {
	private AudioSource audioSource;

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audioSource = GetComponent<AudioSource>();
	}

	void TaskOnClick()
	{
		StartCoroutine(PlaySoundThenQuit());
	}

	IEnumerator PlaySoundThenQuit()
	{
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		Application.Quit ();
	}
}
