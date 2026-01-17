using UnityEngine;

public class ExampleOverlapSphere : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        float radius = 1f;

        Collider[] colliders = Physics.OverlapSphere(position, radius);

        RaycastDebugger.DebugOverlapSphere(position, radius, colliders, Color.red);
    }
}
