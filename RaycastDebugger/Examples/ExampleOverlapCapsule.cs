using System.Linq;
using UnityEngine;

public class ExampleOverlapCapsule : MonoBehaviour
{
    [SerializeField] private float radius = 2f;

    void FixedUpdate()
    {
        Vector3 point0 = transform.position;
        Vector3 point1 = transform.position + Vector3.forward * 5f;

        Collider[] colliders = Physics.OverlapCapsule(point0, point1, radius);
        
        RaycastDebugger.DebugOverlapCapsule(point0, point1, radius, colliders, colliders.Count() == 0 ? Color.white : Color.red);
    }
}
