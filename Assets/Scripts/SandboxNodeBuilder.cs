using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class SandboxNodeBuilder : NodeBuilder
{
    private const string OBSTACLE_PREFAB = "Prefabs/Obstacle";

    public bool selectedObstacle
    {
        get { return _selectedObstacle; }
        set { _selectedObstacle = value; ChangeSelected(null); _selectedType = NodeType.NONE; }
    }
    public new EnergyNode selected
    {
        get { return _selected; }
        set { ChangeSelected(value); _selectedType = NodeType.NONE; _selectedObstacle = false; }
    }
    public new NodeType selectedType
    {
        get { return _selectedType; }
        set { _selectedType = value; ChangeSelected(null); _selectedObstacle = false; }
    }

    private bool _selectedObstacle;
    private Obstacle createdObstacle;

    public void LockSelected()
    {
        _selected.lockPosition = !_selected.lockPosition;
    }

    public void DeleteSelected()
    {
        Destroy(_selected.gameObject);
        _selected = null;
    }

    protected new void Update()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && _selectedObstacle)
        {
            paused = true;
            Obstacle obstacle = Instantiate(Resources.Load<GameObject>(OBSTACLE_PREFAB), Vector3.zero, Quaternion.identity).GetComponent<Obstacle>();
            obstacle.transform.parent = transform;
            obstacle.start = obstacle.end = GetTargetedPoint(Input.mousePosition);
            createdObstacle = obstacle;
        }
        else if (Input.GetMouseButtonUp(0) && createdObstacle != null)
        {
            paused = false;
            createdObstacle = null;
        }
        else if (Input.GetMouseButton(0) && createdObstacle != null)
        {
            createdObstacle.end = GetTargetedPoint(Input.mousePosition);
        }
        else
        {
            base.Update();
        }
    }
}
