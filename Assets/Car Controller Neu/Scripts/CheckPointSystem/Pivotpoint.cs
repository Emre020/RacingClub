using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivotpoint : MonoBehaviour
{
    public float speed;
    public Transform pivotpoint;

    // Update is called once per frame
    void Update()
    {
        pivotpoint.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}

