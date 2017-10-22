using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Vector3 start
    {
        get { return _start; }
        set { _start = value; PrepareTransform(); }
    }
    public Vector3 end
    {
        get { return _end; }
        set { _end = value; PrepareTransform(); }
    }

    private Vector3 _start;
    private Vector3 _end;

    void PrepareTransform()
    {
        Vector3 path = _end - _start;
        transform.position = _start + path / 2;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, path);
        transform.localScale = new Vector3(path.magnitude, 0.1F, 0F);
    }
}
