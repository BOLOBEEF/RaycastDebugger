using System.Linq;
using UnityEngine;

public class ExampleOverlapSphere : MonoBehaviour
{
    [SerializeField] private float radius = 2f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;

        Collider[] colliders = Physics.OverlapSphere(position, radius);

        RaycastDebugger.DebugOverlapSphere(position, radius, colliders, colliders.Count() == 0 ? Color.white : Color.red);
    }
}
