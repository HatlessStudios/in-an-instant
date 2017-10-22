using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}
	
	void OnButtonClick ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
