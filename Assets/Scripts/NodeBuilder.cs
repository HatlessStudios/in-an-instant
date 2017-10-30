using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class NodeBuilder : MonoBehaviour {
    private const string ENERGY_NODE_PREFAB = "Prefabs/EnergyNode";
    private const float KEYBOARD_SENSITIVITY = 0.5F;

    public bool paused
    {
        get { return _paused; }
        set { _paused = value; Time.timeScale = value ? 0 : 1; _pauseButton.GetComponentInChildren<Text>().text = value ? "Unpause" : "Pause"; }
    }
    public virtual EnergyNode selected
    {
        get { return _selected; }
        set { ChangeSelected(value); _selectedType = NodeType.NONE; }
    }
    public virtual NodeType selectedType
    {
        get { return _selectedType; }
        set { _selectedType = value; ChangeSelected(null); }
    }
    public bool lockCreated
    {
        get { return _lockCreated; }
        set { _lockCreated = value; }
    }
    public double globalTime
    {
        get { return _globalTime; }
        set { _globalTime = value; }
    }

    public Behaviour pauseButton
    {
        get { return _pauseButton; }
        set { _pauseButton = value; }
    }

    [SerializeField]
    protected bool _lockCreated;
    [SerializeField]
    protected double _globalTime;
    protected EnergyNode _selected;
    protected NodeType _selectedType = NodeType.NONE;
    protected bool _paused;
    protected Behaviour _pauseButton;
    protected bool selectionUpdated;
    protected EnergyNode created;
    protected bool scroll;
    protected Vector3 lastMousePosition;
    protected bool lastPaused;

    public void SetSelectedType(string type)
    {
        selectedType = (NodeType) Enum.Parse(typeof(NodeType), type);
    }
    public void DeleteSelected()
    {
        if (_selected == null) return;
        Destroy(_selected.gameObject);
        _selected = null;
    }

    public void TogglePaused()
    {
        paused = !paused;
    }

    protected void ChangeSelected(EnergyNode selected)
    {
        if (_selected != null) _selected.halo.enabled = false;
        _selected = selected;
        if (_selected != null) _selected.halo.enabled = true;
        selectionUpdated = true;
    }

    protected Vector3 GetTargetedPoint(Vector3 mouse)
    {
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        return new Vector3(mouse.x, mouse.y, 0);
    }

    protected virtual void Update()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            if (_selectedType == NodeType.NONE)
            {
                if (!selectionUpdated) selected = null;
                scroll = true;
                lastMousePosition = GetTargetedPoint(Input.mousePosition);
                selectionUpdated = false;
                return;
            }
            lastPaused = paused;
            paused = true;
            EnergyNode node = Instantiate(Resources.Load<GameObject>(ENERGY_NODE_PREFAB), GetTargetedPoint(Input.mousePosition), Quaternion.identity).GetComponent<EnergyNode>();
            node.transform.parent = transform;
            node.GetComponent<Renderer>().material = GetMaterial(_selectedType);
            node.GetComponent<RelativeRigidbody>().lockPosition = _lockCreated;
            created = node;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (created != null)
            {
                paused = lastPaused;
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
            else if (scroll)
            {
                scroll = false;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (created != null)
            {
                created.radius = Math.Max(1, (created.transform.position - GetTargetedPoint(Input.mousePosition)).magnitude);
            }
            else if (scroll)
            {
                Vector3 current = GetTargetedPoint(Input.mousePosition);
                Camera.main.transform.position -= current - lastMousePosition;
            }
        }
        selectionUpdated = false;
        HandleInput();
    }

    protected void HandleInput()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Camera.main.orthographicSize = Math.Max(5, Camera.main.orthographicSize - 5 * Input.GetAxis("Mouse ScrollWheel"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedType = NodeType.GRAVITY_POSITIVE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            selectedType = NodeType.GRAVITY_NEGATIVE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            selectedType = NodeType.CHARGE_NEGATIVE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            selectedType = NodeType.CHARGE_POSITIVE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            selectedType = NodeType.TIME_POSITIVE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            selectedType = NodeType.TIME_NEGATIVE;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePaused();
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteSelected();
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += KEYBOARD_SENSITIVITY * Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position += KEYBOARD_SENSITIVITY * Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += KEYBOARD_SENSITIVITY * Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position += KEYBOARD_SENSITIVITY * Vector3.left;
        }
    }

    protected virtual void FixedUpdate()
    {
        RelativeRigidbody[] bodies = GetComponentsInChildren<RelativeRigidbody>();
        RelativeRigidbody[] withCollisions = bodies.Where(b => b.hasCollision).ToArray();
        foreach (RelativeRigidbody body in bodies)
        {
            foreach (Action listener in body.listeners)
            {
                listener.Invoke();
            }
        }
        float timeRemaining = 1F;
        bool collisionFound;
        int iterations = 0;
        do
        {
            foreach (RelativeRigidbody body in bodies)
            {
                body.CalculateVelocity(Time.fixedDeltaTime * timeRemaining);
            }
            collisionFound = false;
            float collisionTime = 1F;
            RelativeRigidbody collided1 = null, collided2 = null;
            foreach (RelativeRigidbody body in withCollisions)
            {
                RelativeRigidbody foundCollided;
                float foundTime;
                if (body.FindCollision(withCollisions.Where(b => b != body), out foundCollided, out foundTime))
                {
                    if (foundTime < collisionTime)
                    {
                        collisionFound = true;
                        collisionTime = foundTime;
                        collided1 = body;
                        collided2 = foundCollided;
                    }
                }
            }
            if (collisionTime > 0F)
            {
                foreach (RelativeRigidbody body in bodies)
                {
                    body.ApplyVelocity(collisionTime);
                }
                timeRemaining -= collisionTime;
            }
            if (collisionFound)
            {
                collided1.OnCollision(collided2);
                collided2.OnCollision(collided1);
            }
        } while (iterations++ < 1000 && collisionFound && timeRemaining > 0F);
        if (iterations > 1000)
        {
            Debug.Log("Reached maximum number of iterations");
        }
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

    public enum NodeType
    {
        NONE,
        GRAVITY_POSITIVE, GRAVITY_NEGATIVE,
        CHARGE_POSITIVE, CHARGE_NEGATIVE,
        TIME_POSITIVE, TIME_NEGATIVE
    }
}
