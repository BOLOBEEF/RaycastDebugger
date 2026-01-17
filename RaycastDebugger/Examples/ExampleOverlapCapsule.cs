using UnityEngine;

public class ExampleOverlapCapsule : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 point0 = transform.position;
        Vector3 point1 = transform.position + Vector3.forward * 5f;
        float radius = 1f;

        Collider[] colliders = Physics.OverlapCapsule(point0, point1, radius);

        RaycastDebugger.DebugOverlapCapsule(point0, point1, radius, colliders, Color.red);
    }
}


