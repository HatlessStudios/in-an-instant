using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRigidbody : MonoBehaviour {
    public Vector3 velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }
    public float timeScale
    {
        get { return _timeScale; }
        set { _timeScale = value; }
    }
    public bool lockPosition
    {
        get { return _lockPosition; }
        set { _lockPosition = value; }
    }

    [SerializeField]
    private Vector3 _velocity;
    [SerializeField]
    private float _timeScale = 1F;
    [SerializeField]
    private bool _lockPosition;
	
	void FixedUpdate()
    {
        if (_lockPosition) return;
        transform.position += timeScale * Time.fixedDeltaTime * velocity;
	}
}
