using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRigidbody : MonoBehaviour {
    private const float COLLISION_LIMIT = 1e-6F;

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
    private List<RelativeRigidbody> clipping = new List<RelativeRigidbody>();

    public void CalculateVelocity(float deltaTime)
    {
        scaledVelocity = _lockPosition ? Vector3.zero : timeScale * deltaTime * velocity;
    }

    public void ApplyVelocity(float collisionTime)
    {
        transform.position += collisionTime * scaledVelocity + antiClipping;
        antiClipping = Vector3.zero;
        clipping.Clear();
    }

    public void OnCollision(RelativeRigidbody body)
    {
        if (_lockPosition) return;
        nextVelocity = _velocity - .5F * Vector3.Project(_velocity - body._velocity, transform.position - body.transform.position);
    }

    public bool FindCollision(IEnumerable<RelativeRigidbody> bodies, out RelativeRigidbody collidedBody, out float collisionTime)
    {
        collidedBody = null;
        collisionTime = 1F;
        foreach (RelativeRigidbody body in bodies)
        {
            Vector3 dp = transform.position - body.transform.position;
            Vector3 dv = scaledVelocity - body.scaledVelocity;
            float dpv = Vector3.Dot(dp, dv);
            if (dp.sqrMagnitude <= 1F && dpv < 1F)
            {
                Vector3 adjustment = .5F * (1F - dp.magnitude) * dp.normalized;
                antiClipping += adjustment;
                body.antiClipping -= adjustment;
                clipping.Add(body);
                body.clipping.Add(this);
                continue;
            }
            if (dv == Vector3.zero || clipping.Contains(body)) continue;
            double discriminant = dpv * dpv - 4D * dv.sqrMagnitude * (dp.sqrMagnitude - 1D);
            if (discriminant >= 0D)
            {
                discriminant = Math.Sqrt(discriminant);
                float collision1 = (float)(-dpv + discriminant) / 2F / dv.magnitude;
                float collision2 = (float)(-dpv - discriminant) / 2F / dv.magnitude;
                if (collision1 >= 0F && collision1 <= collisionTime)
                {
                    collidedBody = body;
                    collisionTime = collision1;
                }
                if (collision2 >= 0F && collision2 <= collisionTime)
                {
                    collidedBody = body;
                    collisionTime = collision2;
                }
            }
        }
        return collidedBody != null;
    }
}
