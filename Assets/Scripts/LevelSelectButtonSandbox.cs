using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButtonSandbox : MonoBehaviour {

	public AudioSource audio;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}
	
	// Update is called once per frame
	void OnButtonClick ()
    {
		StartCoroutine(playSoundThenLoad());
    }

	IEnumerator playSoundThenLoad()
	{
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length - 0.3f);
		SceneManager.LoadScene("Sandbox");
	}
}
