using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
	public string linkedLevel;

	public AudioSource audioSource;

	// Use this for initialization
	void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}
	
	// Update is called once per frame
	void OnButtonClick()
    {
		StartCoroutine(PlaySoundThenLoad());
    }

	IEnumerator PlaySoundThenLoad()
	{
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length - 0.3f);
		SceneManager.LoadScene("Level" + linkedLevel);
	}
}
