using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
	private AudioSource audioSource;
	private AudioSource levelAudio;

	// Use this for initialization
	void Start()
    {
		audioSource = GetComponent<AudioSource>();
		levelAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.name == "Objective")
        {
			levelAudio.Stop ();
			StartCoroutine (playSoundThenLoad ());
		}
	}

	IEnumerator playSoundThenLoad()
	{
        audioSource.volume = 1.5f;
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		SceneManager.LoadScene("LevelSelect");
	}
}
