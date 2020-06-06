using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationController : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] private Vector3 destination;
    [SerializeField] private Transform[] targets;
    [SerializeField] private int order = 0;

    public enum Route {inOrder, random}
    public Route route;
    // Start is called before the first frame update
    void Start(){
        startPosition = transform.position;
        SetDestination(transform.position);
    }

    public void CreateDestination(){
        if(route == Route.inOrder){
            CreateInOrderDestination();
        } else if(route == Route.random){
            CreateDestRandomination();
        }
    }

    private void CreateInOrderDestination(){
        if(order < targets.Length - 1){
            order++;
            SetDestination(new Vector3(targets[order].transform.position.x, 0, targets[order].transform.position.z));
        } else {
            order = 0;
            SetDestination(new Vector3(targets[order].transform.position.x, 0, targets[order].transform.position.z));
        }
    }

    private void CreateDestRandomination(){
        int num = Random.Range(0, targets.Length);
        SetDestination(new Vector3(targets[num].transform.position.x, 0, targets[num].transform.position.z));
    }

    public void SetDestination(Vector3 position){
        destination = position;
    }


    public Vector3 GetDestination(){
        return destination;
    }

}