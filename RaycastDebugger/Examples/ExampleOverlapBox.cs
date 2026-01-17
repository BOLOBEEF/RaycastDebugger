using System.Linq;
using UnityEngine;

public class ExampleOverlapBox : MonoBehaviour
{
    [SerializeField] private float size = 2f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 halfExtents = Vector3.one * size;
        Quaternion orientation = transform.rotation;

        Collider[] colliders = Physics.OverlapBox(position, halfExtents, orientation);

        RaycastDebugger.DebugOverlapBox(position, halfExtents, orientation, colliders, colliders.Count() == 0 ? Color.white : Color.red);
    }
}
