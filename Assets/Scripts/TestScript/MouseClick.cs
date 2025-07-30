using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class MouseClick : MonoBehaviour
{
    NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    Ray ray;
    RaycastHit hit;
    NavMeshSurface navMeshSurface;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshSurface.RemoveData();
        navMeshSurface.AddData();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            obstacle.carving = !obstacle.carving;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hit.point, 100f);
        Gizmos.DrawRay(ray.origin, ray.direction * 100);
    }
}
