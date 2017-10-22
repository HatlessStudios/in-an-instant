using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButtonSandbox : MonoBehaviour
{
	public AudioSource audioSource;
    
	void Start () {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}
	
	void OnButtonClick ()
    {
		StartCoroutine(PlaySoundThenLoad());
    }

	IEnumerator PlaySoundThenLoad()
	{
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		SceneManager.LoadScene("Sandbox");
	}
}
