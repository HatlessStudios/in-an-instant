using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyNode : MonoBehaviour {
    public const double STRENGTH = 1e-2;

    public float gravity
    {
        get { return _gravity; }
        set { _gravity = value; }
    }
    public float charge
    {
        get { return _charge; }
        set { _charge = value; }
    }
    public float time
    {
        get { return _time; }
        set { _time = value; }
    }
    public float radius
    {
        get { return _radius; }
        set { _radius = value; }
    }

    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _charge;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _radius;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EnergyNode[] nodes = transform.parent.GetComponentsInChildren<EnergyNode>();
        double timeFlow = GetTimeFlow(nodes);
        foreach (EnergyNode node in nodes.Where(n => n.transform.position != transform.position))
        {
            double nodeTimeFlow = timeFlow * node.GetTimeFlow(nodes);
            if (nodeTimeFlow == 0) continue;
            Vector3 path = node.transform.position - transform.position;
            if (path.magnitude > node._radius) continue;
            path /= path.sqrMagnitude;
            body.AddForce((float) (STRENGTH * _gravity * node._gravity) * path, ForceMode.Impulse);
            body.AddForce((float) (STRENGTH * _charge * node._charge) * path, ForceMode.Impulse);
        }
	}

    double GetTimeFlow(EnergyNode[] nodes)
    {
        return nodes.Where(n => n.radius >= (n.transform.position - transform.position).magnitude).Sum(n => n._time);
    }
}
