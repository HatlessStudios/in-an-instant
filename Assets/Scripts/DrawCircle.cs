using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour {
    public float thetaScale = 0.01f;
    public float radius
    {
        get { return _radius; }
        set
        {
            _radius = value;
            float theta = 0F;
            positions = new Vector3[(int) (1F / thetaScale + 1F)];
            for (int i = 0; i < positions.Length; i++)
            {
                theta += (2.0f * Mathf.PI * thetaScale);
                float x = _radius * Mathf.Cos(theta);
                float y = _radius * Mathf.Sin(theta);
                positions[i] = new Vector3(x, y, 0);
            }
        }
    }
    [SerializeField]
    private float _radius = 0F;
    private LineRenderer lineDrawer;
    private Vector3[] positions = new Vector3[0];

    void Start()
    {
        lineDrawer = GetComponent<LineRenderer>();
        radius = _radius;
    }

    void Update()
    {
        lineDrawer.positionCount = positions.Length;
        lineDrawer.SetPositions(positions);
    }
}
