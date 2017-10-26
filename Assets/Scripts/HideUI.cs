using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour {

	public GameObject objectToBeHidden;

	// Use this for initialization
	void Start () {
		Destroy(objectToBeHidden);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
