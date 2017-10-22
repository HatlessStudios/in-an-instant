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
    public bool lockCreated
    {
        get { return _lockCreated; }
        set { _lockCreated = value; }
    }

    [SerializeField]
    private bool _lockCreated;
    private EnergyNode _selected;
    private NodeType _selectedType = NodeType.NONE;
    private bool _paused;
    private bool selectionUpdated;
    private EnergyNode created;

    public void SetSelectedType(string type)
    {
        selectedType = (NodeType) Enum.Parse(typeof(NodeType), type);
    }

    void LockSelected(bool value)
    {
        _selected.lockPosition = value;
    }

    void DeleteSelected()
    {
        Destroy(_selected);
        _selected = null;
    }

    void Update()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            if (_selectedType == NodeType.NONE)
            {
                if (!selectionUpdated)
                {
                    selected = null;
                    selectionUpdated = false;
                }
                return;
            }
            paused = true;
            EnergyNode node = Instantiate(Resources.Load<GameObject>(ENERGY_NODE_PREFAB), GetTargetedPoint(Input.mousePosition), Quaternion.identity).GetComponent<EnergyNode>();
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material = GetMaterial(_selectedType);
            node.lockPosition = _lockCreated;
            created = node;
        }
        else if (Input.GetMouseButtonUp(0) && created != null)
        {
            paused = false;
            switch (selectedType)
            {
                case NodeType.GRAVITY_POSITIVE:
                    created.gravity = 1 / created.radius;
                    break;
                case NodeType.GRAVITY_NEGATIVE:
                    created.gravity = -1 / created.radius;
                    break;
                case NodeType.CHARGE_POSITIVE:
                    created.charge = 1 / created.radius;
                    break;
                case NodeType.CHARGE_NEGATIVE:
                    created.charge = -1 / created.radius;
                    break;
                case NodeType.TIME_POSITIVE:
                    created.time = 1 / created.radius;
                    break;
                case NodeType.TIME_NEGATIVE:
                    created.time = -1 / created.radius;
                    break;
            }
            created = null;
        }
        else if (Input.GetMouseButton(0) && created != null)
        {
            created.radius = Math.Max(1, (created.transform.position - GetTargetedPoint(Input.mousePosition)).magnitude);
        }
        selectionUpdated = false;
    }

    void ChangeSelected(EnergyNode selected)
    {
        if (_selected != null) _selected.halo.enabled = false;
        _selected = selected;
        if (_selected != null) _selected.halo.enabled = true;
        selectionUpdated = true;
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
