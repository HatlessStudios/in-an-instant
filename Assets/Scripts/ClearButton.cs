using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    public NodeBuilder nodeBuilder;

	// Use this for initialization
	void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}
	
	void OnButtonClick()
    {
        foreach (EnergyNode node in nodeBuilder.GetComponentsInChildren<EnergyNode>())
        {
            Destroy(node.gameObject);
        }
	}
}
