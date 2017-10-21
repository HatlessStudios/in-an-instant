using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBuilder : MonoBehaviour {
    private const string ENERGY_NODE_PREFAB = "Prefabs/EnergyNode";

    public bool paused
    {
        get { return _paused; }
        set { _paused = value; Time.timeScale = value ? 0 : 1; }
    }
    public EnergyNode selected
    {
        get { return _selected; }
        set { ChangeSelected(value); _selectedType = NodeType.NONE; }
    }
    public NodeType selectedType
    {
        get { return _selectedType; }
        set { _selectedType = value; ChangeSelected(null); }
    }

    private EnergyNode _selected;
    private NodeType _selectedType = NodeType.GRAVITY_NEGATIVE;
    private bool _paused;

    public void SetSelectedType(string type)
    {
        Debug.Log("Set type to: " + type);
        selectedType = (NodeType) Enum.Parse(typeof(NodeType), type);
        Debug.Log("New selected type: " + selectedType);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedType == NodeType.NONE) return;
            paused = true;
            EnergyNode node = Instantiate(Resources.Load<GameObject>(ENERGY_NODE_PREFAB), GetTargetedPoint(Input.mousePosition), Quaternion.identity).GetComponent<EnergyNode>();
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material = GetMaterial(_selectedType);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            paused = false;
        }
    }

    void ChangeSelected(EnergyNode selected)
    {
        if (_selected != null) _selected.halo.enabled = false;
        _selected = selected;
        if (_selected != null) _selected.halo.enabled = true;
    }

    Material GetMaterial(NodeType type)
    {
        switch (type)
        {
            case NodeType.GRAVITY_POSITIVE:
                return Resources.Load<Material>("Materials/Nodes/GravityNode");
            case NodeType.GRAVITY_NEGATIVE:
                return Resources.Load<Material>("Materials/Nodes/AntiGravityNode");
            case NodeType.CHARGE_POSITIVE:
                return Resources.Load<Material>("Materials/Nodes/PositiveMagNode");
            case NodeType.CHARGE_NEGATIVE:
                return Resources.Load<Material>("Materials/Nodes/NegativeMagNode");
            case NodeType.TIME_POSITIVE:
                return Resources.Load<Material>("Materials/Nodes/TimeForwardNode");
            case NodeType.TIME_NEGATIVE:
                return Resources.Load<Material>("Materials/Nodes/TimeBackNode");
        }
        return null;
    }

    Vector3 GetTargetedPoint(Vector3 mouse)
    {
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        return new Vector3(mouse.x, mouse.y, 0);
    }

    public enum NodeType
    {
        NONE,
        GRAVITY_POSITIVE, GRAVITY_NEGATIVE,
        CHARGE_POSITIVE, CHARGE_NEGATIVE,
        TIME_POSITIVE, TIME_NEGATIVE
    }
}
