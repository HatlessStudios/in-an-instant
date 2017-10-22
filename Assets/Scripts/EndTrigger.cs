using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {

	private AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Objective") {
			StartCoroutine (playSoundThenLoad ());
		}
	}

	IEnumerator playSoundThenLoad()
	{
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length - 0.3f);
		Application.LoadLevel("LevelSelect");
	}
}
