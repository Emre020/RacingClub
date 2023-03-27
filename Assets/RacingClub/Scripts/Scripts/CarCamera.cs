using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    internal enum updateMethod
    {
        fixedUpdate,
        update,
        lateUpdate
    }
    [SerializeField] private updateMethod updateDemo;

    private GameObject attachedVehicle;

    private GameObject cameraPositionHolder;
    public Transform[] camlocations;
    private CarControllerAdvanced controllerRefernce;
    [Range(0,20)]public float smoothTime;
    public int locationIndicator = 1;

    void Start()
    {
        attachedVehicle = GameObject.FindGameObjectWithTag("Car");
        controllerRefernce = attachedVehicle.GetComponent<CarControllerAdvanced>();
        cameraPositionHolder = attachedVehicle.transform.Find("CAMERA").gameObject;
        camlocations = cameraPositionHolder.GetComponentsInChildren<Transform>();
    }

    private void FixedUpdate()
    {
        if (updateDemo == updateMethod.fixedUpdate)
            cameraBehaviour();
        /*if (Input.GetKey(KeyCode.V))
        {
            // change camLocation
            //locationIndicator = (locationIndicator >= 2) ? ++ : 2; 
        }
        */
    }

    void Update()
    {
        if (updateDemo == updateMethod.update)
            cameraBehaviour();
    }

    private void LateUpdate()
    {
        if (updateDemo == updateMethod.lateUpdate)
            cameraBehaviour();
    }

    void cameraBehaviour()
    {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, camlocations[1].transform.position, ref velocity, smoothTime * Time.deltaTime);
        transform.LookAt(camlocations[1].transform);
    }
}
