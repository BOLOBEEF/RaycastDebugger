using UnityEngine;

public class ExampleRaycast : MonoBehaviour
{

    // works fine with Update as well
    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 direction = transform.forward;
        float rayLength = 5f;

        if (Physics.Raycast(position, direction, out RaycastHit rayHit, rayLength))
        {
            RaycastDebugger.DebugRaycast(position, direction, rayHit.distance, Color.red);
        }
        else
        {
            RaycastDebugger.DebugRaycast(position, direction, rayLength, Color.white);
        }
        // also supports Ray
        // RaycastDebugger.DebugRaycast(ray, rayLength, Color.white);
    }
}
