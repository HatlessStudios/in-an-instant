using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SandboxNodeBuilder : NodeBuilder
{
    private const string OBSTACLE_PREFAB = "Prefabs/Obstacle";

    public bool selectedObstacle
    {
        get { return _selectedObstacle; }
        set { _selectedObstacle = value; ChangeSelected(null); _selectedType = NodeType.NONE; }
    }
    public override EnergyNode selected
    {
        get { return _selected; }
        set { ChangeSelected(value); _selectedType = NodeType.NONE; _selectedObstacle = false; }
    }
    public override NodeType selectedType
    {
        get { return _selectedType; }
        set { _selectedType = value; ChangeSelected(null); _selectedObstacle = false; }
    }
    public Behaviour lockButton
    {
        get { return _lockButton; }
        set { _lockButton = value; }
    }

    private bool _selectedObstacle;
    protected Behaviour _lockButton;
    private Obstacle createdObstacle;

    public void LockSelected()
    {
        if (_selected == null) return;
        RelativeRigidbody body = _selected.GetComponent<RelativeRigidbody>();
        _lockButton.GetComponentInChildren<Text>().text = (body.lockPosition = !body.lockPosition) ? "Unlock" : "Lock";
    }

    protected override void Update()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && _selectedObstacle && !selectionUpdated)
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
            return;
        }
        HandleInput();
    }
}
