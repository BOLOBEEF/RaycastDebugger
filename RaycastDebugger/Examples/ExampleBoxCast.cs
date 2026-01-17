using UnityEngine;

public class ExampleBoxCast : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 direction = transform.forward;
        Vector3 halfExtents = Vector3.one;
        Quaternion orientation = transform.rotation;
        float rayLength = 5f;

        if (Physics.BoxCast(position, halfExtents, direction, out RaycastHit boxHit, orientation, rayLength))
        {
            // collider and color params are optional
            RaycastDebugger.DebugBoxCast(position, halfExtents, direction, orientation, boxHit.distance, boxHit.collider, color: Color.red);
        }
        else
        {
            RaycastDebugger.DebugBoxCast(position, halfExtents, direction, orientation, rayLength, color: Color.white);
        }
    }
}
