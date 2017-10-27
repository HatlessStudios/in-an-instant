using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {
	void Start () {
        NodeBuilder builder = gameObject.scene.GetRootGameObjects().SelectMany(o => o.GetComponents<NodeBuilder>()).Single();
        builder.pauseButton = GetComponent<Button>();
	}
}
