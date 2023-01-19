using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCar : MonoBehaviour
{
    public GameObject Car;
    public GameObject child;
    public float speed;

    private void Awake()
    {
        Car = GameObject.FindGameObjectWithTag("Car");
        child = Car.transform.Find("camera constraint").gameObject;
    }

    private void Update()
    {
        Follow();
    }

    void Follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, child.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(Car.gameObject.transform.position);
    }
}
