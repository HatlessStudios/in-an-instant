using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRigidbody : MonoBehaviour {
    public Vector3 velocity
    {
        get { return _velocity = nextVelocity; }
        set { _velocity = value; nextVelocity = value; }
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
    public bool hasCollision
    {
        get { return _hasCollision; }
        set { _hasCollision = value; }
    }
    public List<Action> listeners
    {
        get { return _listeners; }
    }

    [SerializeField]
    private Vector3 _velocity;
    [SerializeField]
    private float _timeScale = 1F;
    [SerializeField]
    private bool _lockPosition;
    [SerializeField]
    private bool _hasCollision;
    private List<Action> _listeners = new List<Action>();
    private Vector3 scaledVelocity;
    private Vector3 nextVelocity;
    private Vector3 antiClipping;

    public void CalculateVelocity(float deltaTime)
    {
        scaledVelocity = _lockPosition ? Vector3.zero : timeScale * deltaTime * velocity;
    }

    public void ApplyVelocity(float collisionTime)
    {
        transform.position += collisionTime * scaledVelocity + antiClipping;
        antiClipping = Vector3.zero;
    }

    public void OnCollision(RelativeRigidbody body)
    {
        if (_lockPosition) return;
        nextVelocity = _velocity - .5F * Vector3.Project(_velocity - body._velocity, transform.position - body.transform.position);
    }

    public void OnClipping(RelativeRigidbody body)
    {
        Vector3 dp = transform.position - body.transform.position;
        antiClipping += 0.5F * (1F - dp.magnitude) * dp.normalized;
    }

    public bool FindCollision(IEnumerable<RelativeRigidbody> bodies, out RelativeRigidbody collidedBody, out float collisionTime)
    {
        collidedBody = null;
        collisionTime = 1F;
        if (timeScale == 0) return false;
        foreach (RelativeRigidbody body in bodies)
        {
            if (body.timeScale == 0) continue;
            Vector3 dp = transform.position - body.transform.position;
            Vector3 dv = scaledVelocity - body.scaledVelocity;
            float dpv = Vector3.Dot(dp, dv);
            if (dp.sqrMagnitude <= 1F && dpv < 1F)
            {
                OnClipping(body);
                body.OnClipping(this);
                continue;
            }
            if (dv == Vector3.zero) continue;
            double discriminant = dpv * dpv - 4D * dv.sqrMagnitude * (dp.sqrMagnitude - 1D);
            if (discriminant >= 0D)
            {
                discriminant = Math.Sqrt(discriminant);
                float collision1 = (float) (-dpv + discriminant) / 2F / dv.magnitude;
                float collision2 = (float) (-dpv - discriminant) / 2F / dv.magnitude;
                if (collision1 >= 0F && collision1 <= collisionTime)
                {
                    collidedBody = body;
                    collisionTime = Math.Max(0F, collision1);
                }
                if (collision2 >= 0F && collision2 <= collisionTime)
                {
                    collidedBody = body;
                    collisionTime = Math.Max(0F, collision2);
                }
            }
        }
        return collidedBody != null;
    }
}
