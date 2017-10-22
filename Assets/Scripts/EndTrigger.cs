using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
	private AudioSource audioSource;

	// Use this for initialization
	void Start()
    {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}

	void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.name == "Objective")
        {
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
