using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        Vector3 pos = new Vector3(target.position.x, target.position.y, -10);
        transform.position = pos;
    }
}
