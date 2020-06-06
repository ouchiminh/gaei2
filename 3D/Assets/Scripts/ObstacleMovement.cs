using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]
public class ObstacleMovement : MonoBehaviour{
    private NavMeshAgent navAgent = null;
    [SerializeField] private DestinationController destinationController;
    // Start is called before the first frame update
    void Start(){
        navAgent = GetComponent<NavMeshAgent>();
        destinationController = GetComponent<DestinationController>();
        navAgent.SetDestination(destinationController.GetDestination());
    }

    // Update is called once per frame
    void Update(){
        if(Vector3.Distance(transform.position, destinationController.GetDestination()) < 3f){
            destinationController.CreateDestination();
            navAgent.SetDestination(destinationController.GetDestination());
        }
    }
}
