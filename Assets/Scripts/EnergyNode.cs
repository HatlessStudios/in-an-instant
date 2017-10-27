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
    public Behaviour halo { get; private set; }
    public DrawCircle circle { get; private set; }
    public AudioSource collision { get; private set; }

    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _charge;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _radius;

    private NodeBuilder parent;
    private RelativeRigidbody body;

    void Start()
    {
        parent = GetComponentInParent<NodeBuilder>();
        body = GetComponent<RelativeRigidbody>();
        halo = (Behaviour)GetComponent("Halo");
        circle = GetComponent<DrawCircle>();
        collision = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        EnergyNode[] nodes = transform.parent.GetComponentsInChildren<EnergyNode>();
        double timeFlow = GetTimeFlow(nodes);
        body.timeScale = (float) timeFlow;
        if (timeFlow == 0D) return;
        foreach (EnergyNode node in nodes.Where(n => n.transform.position != transform.position))
        {
            double nodeTimeFlow = timeFlow * node.GetTimeFlow(nodes);
            if (nodeTimeFlow == 0) continue;
            Vector3 path = node.transform.position - transform.position;
            if (path.magnitude > node._radius) continue;
            path /= Math.Max(1F, path.sqrMagnitude);
            body.velocity += (float) (STRENGTH * (_gravity * node._gravity - _charge * node._charge)) * path;
        }
    }

    void OnMouseDown()
    {
        parent.selected = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.collision.Play();
    }

    double GetTimeFlow(EnergyNode[] nodes)
    {
        return parent.globalTime + nodes.Where(n => n.radius >= (n.transform.position - transform.position).magnitude).Sum(n => n._time);
    }
}
