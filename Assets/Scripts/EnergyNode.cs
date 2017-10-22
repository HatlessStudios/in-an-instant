using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyNode : MonoBehaviour {
    public const double STRENGTH = 20;

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
        set { _radius = value; circle.radius = value; }
    }
    public bool lockPosition
    {
        get { return _lockPosition; }
        set { _lockPosition = value; }
    }
    public Behaviour halo { get; private set; }
    public DrawCircle circle { get; private set; }

    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _charge;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private bool _lockPosition;

    private NodeBuilder parent;
    private Rigidbody body;
    private Vector3 trueVelocity;
    private Rigidbody bodySim;

    private void Start()
    {
        parent = GetComponentInParent<NodeBuilder>();
        body = GetComponent<Rigidbody>();
        halo = (Behaviour) GetComponent("Halo");
        circle = GetComponent<DrawCircle>();
        trueVelocity = body.velocity;
        bodySim = Instantiate(Resources.Load<GameObject>("Prefabs/PhysicsSim"), Vector3.zero, Quaternion.identity).GetComponent<Rigidbody>();
        bodySim.transform.parent = transform;
    }

    void Update()
    {
        if (parent.paused || lockPosition) return;
        EnergyNode[] nodes = transform.parent.GetComponentsInChildren<EnergyNode>();
        double rawTimeFlow = GetTimeFlow(nodes);
        double timeFlow = rawTimeFlow * Time.deltaTime;
        body.AddForce(-body.velocity, ForceMode.VelocityChange);
        bodySim.AddForce(-bodySim.velocity, ForceMode.VelocityChange);
        foreach (EnergyNode node in nodes.Where(n => n.transform.position != transform.position))
        {
            double nodeTimeFlow = timeFlow * node.GetTimeFlow(nodes);
            if (nodeTimeFlow == 0) continue;
            Vector3 path = node.transform.position - transform.position;
            if (path.magnitude > node._radius) continue;
            path /= path.sqrMagnitude;
            bodySim.AddForce((float) (STRENGTH * _gravity * node._gravity) * path, ForceMode.Impulse);
            bodySim.AddForce((float) (-STRENGTH * _charge * node._charge) * path, ForceMode.Impulse);
        }
        trueVelocity += bodySim.velocity;
        body.AddForce((float) rawTimeFlow * trueVelocity, ForceMode.VelocityChange);
        //body.AddForce(-trueVelocity, ForceMode.VelocityChange);
        //if (((float) rawTimeFlow * trueVelocity).magnitude != 0F) Debug.Log("Time Flow: " + rawTimeFlow + " | Velocity: " + body.velocity);
    }

    void OnMouseDown()
    {
        parent.selected = this;
    }

    double GetTimeFlow(EnergyNode[] nodes)
    {
        return nodes.Where(n => n.radius >= (n.transform.position - transform.position).magnitude).Sum(n => n._time);
    }
}
