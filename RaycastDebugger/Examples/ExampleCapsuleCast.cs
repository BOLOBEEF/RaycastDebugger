using UnityEngine;

public class ExampleCapsuleCast : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 point0 = transform.position;
        Vector3 point1 = transform.position - transform.right * 5f;
        float radius = 1f;
        Vector3 direction = transform.forward;
        float rayLength = 5f;


        if (Physics.CapsuleCast(point0, point1, radius, direction, out RaycastHit capsuleHit, rayLength))
        {
            // collider and color params are optional
            RaycastDebugger.DebugCapsuleCast(point0, point1, radius, direction, capsuleHit.distance, capsuleHit.collider, Color.red);
        }
        else
        {
            RaycastDebugger.DebugCapsuleCast(point0, point1, radius, direction, rayLength, color: Color.white);
        }
    }
}
