﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    public NodeBuilder nodeBuilder;
    
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
        foreach (Obstacle obstacle in nodeBuilder.GetComponentsInChildren<Obstacle>())
        {
            Destroy(obstacle.gameObject);
        }
	}
}
