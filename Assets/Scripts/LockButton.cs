using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockButton : MonoBehaviour {
    public SandboxNodeBuilder energyNodes;
    
	void Start() {
        energyNodes.lockButton = GetComponent<Button>();
	}
}
