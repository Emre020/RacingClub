using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public GameObject[] Targets;
    public float DistanceFromTarget;
    public bool DebugMode;

    // Start is called before the first frame update
    void Start()
    {
        Targets = GameObject.FindGameObjectsWithTag("AITarget");
        int Targetnum = Random.Range(0, Targets.Length);
        this.GetComponent<NavMeshAgent>().SetDestination(Targets[Targetnum].transform.position);
        if(DebugMode = true)
        {
            Debug.Log(Targets[Targetnum].gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int Targetnum = Random.Range(0, Targets.Length);
        if(this.GetComponent<NavMeshAgent>().remainingDistance < 1f)
        {
            this.GetComponent<NavMeshAgent>().SetDestination(Targets[Targetnum].transform.position);
        }
        DistanceFromTarget = this.GetComponent<NavMeshAgent>().remainingDistance;
    }
}
