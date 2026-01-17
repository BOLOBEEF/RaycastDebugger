using UnityEngine;

public class ExampleOverlapBox : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 halfExtents = Vector3.one;
        Quaternion orientation = transform.rotation;

        Collider[] colliders = Physics.OverlapBox(position, halfExtents, orientation);

        RaycastDebugger.DebugOverlapBox(position, halfExtents, orientation, colliders, Color.red);
    }
}
