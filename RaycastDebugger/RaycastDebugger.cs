using System;
using System.Collections.Generic;
using UnityEngine;

public static class RaycastDebugger
{
    // class to store each raycast info
    private class RaycastInfo
    {
        public Vector3 position;
        public Vector3 direction;
        public float distance;
        public Color color;
        public int fixedStep;       // to ensure rays added in FixedUpdate last the entire FixedUpdate step

        public RaycastInfo(Vector3 position, Vector3 direction, float distance, Color color)
        {
            // self destruct if shipped as a build
            if (!Application.isEditor) return;

            // always ensure that the runner is active
            EnsureRunner();

            this.position = position;
            this.direction = direction;
            this.distance = distance;
            this.color = color;
            this.fixedStep = Time.inFixedTimeStep ? runner.fixedStep : -1;
        }
    }

    private class BoxCastInfo : RaycastInfo
    {
        public Vector3 halfExtents;
        public Quaternion orientation;
        public BoxCastInfo(Vector3 position, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance, Color color) : base(position, direction, distance, color)
        {
            this.halfExtents = halfExtents;
            this.orientation = orientation;
        }
    }

    private class SphereCastInfo : RaycastInfo
    {
        public float radius;
        public SphereCastInfo(Vector3 position, float radius, Vector3 direction, float distance, Color color) : base(position, direction, distance, color)
        {
            this.radius = radius;
        }
    }

    private class CapsuleCastInfo : RaycastInfo
    {
        public Vector3 point1;
        public float radius;
        public CapsuleCastInfo(Vector3 point0, Vector3 point1, float radius, Vector3 direction, float distance, Color color) : base(point0, direction, distance, color)
        {
            this.point1 = point1;
            this.radius = radius;
        }
    }

    private static List<RaycastInfo> raycastInfos = new List<RaycastInfo>();


    // raycasts
    public static void DebugRaycast(Vector3 position, Vector3 direction, float distance, Color color = default)
    {
        raycastInfos.Add(new RaycastInfo(position, direction, distance, color == default ? Color.white : color));
    }

    public static void DebugRaycast(Ray ray, float distance, Color color = default)
    {
        raycastInfos.Add(new RaycastInfo(ray.origin, ray.direction, distance, color == default ? Color.white : color));
    }

    // Box, Sphere, Capsule casts
    public static void DebugBoxCast(Vector3 position, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance, Collider collider = null, Color color = default)
    {
        raycastInfos.Add(new BoxCastInfo(position, halfExtents, direction, orientation, distance, color == default ? Color.white : color));

        if (collider != null)
            DebugColliders(position + direction * distance, sphereRadius: 0.2f, collider);
    }
    public static void DebugSphereCast(Vector3 position, float radius, Vector3 direction, float distance, Collider collider = null, Color color = default)
    {
        raycastInfos.Add(new SphereCastInfo(position, radius, direction, distance, color == default ? Color.white : color));

        if (collider != null)
            DebugColliders(position + direction * distance, sphereRadius: 0.2f, collider);
    }
    public static void DebugCapsuleCast(Vector3 point0, Vector3 point1, float radius, Vector3 direction, float distance, Collider collider = null, Color color = default)
    {
        raycastInfos.Add(new CapsuleCastInfo(point0, point1, radius, direction, distance, color == default ? Color.white : color));

        if (collider != null)
            DebugColliders(point0 + direction * distance, point1 + direction * distance, sphereRadius: 0.2f, collider);
    }

    // Overlap Box, Spher, Capsule
    public static void DebugOverlapBox(Vector3 position, Vector3 halfExtents, Quaternion orientation, Collider[] colliders, Color color = default)
    {
        raycastInfos.Add(new BoxCastInfo(position, halfExtents, Vector3.zero, orientation, 0f, color == default ? Color.white : color));
        DebugColliders(position, sphereRadius: 0.2f, colliders);
    }
    public static void DebugOverlapSphere(Vector3 position, float radius, Collider[] colliders, Color color = default)
    {
        raycastInfos.Add(new SphereCastInfo(position, radius, Vector3.zero, 0f, color == default ? Color.white : color));
        DebugColliders(position, sphereRadius: 0.2f, colliders);
    }
    public static void DebugOverlapCapsule(Vector3 point0, Vector3 point1, float radius, Collider[] colliders, Color color = default)
    {
        raycastInfos.Add(new CapsuleCastInfo(point0, point1, radius, Vector3.zero, 0f, color == default ? Color.white : color));
        DebugColliders(point0, point1, sphereRadius: 0.2f, colliders);
    }


    private static void DebugColliders(Vector3 position, float sphereRadius = 0.2f, params Collider[] colliders)
    {
        // debug the closes point of every detected collider
        foreach (Collider collider in colliders)
        {
            Vector3 hitPosition = collider.ClosestPointOnBounds(position);
            DebugSphereCast(position, sphereRadius, (hitPosition - position).normalized, (hitPosition - position).magnitude, color: Color.green);
        }
    }
    private static void DebugColliders(Vector3 point1, Vector3 point2, float sphereRadius, params Collider[] colliders)
    {
        LineSegment segment = new LineSegment(point1, point2);

        // debug the closes point on the segment between the two point for every detected collider
        foreach (Collider collider in colliders)
        {
            Vector3 closestPointOnSegment = segment.ClosestPoint(collider.transform.position);

            Vector3 hitPosition = collider.ClosestPointOnBounds(closestPointOnSegment);

            // debug from closest point on the segment from (point1 to point2) to the closest point on the collider's bounds
            DebugSphereCast(closestPointOnSegment, sphereRadius, (hitPosition - closestPointOnSegment).normalized, (hitPosition - closestPointOnSegment).magnitude, color: Color.green);
        }
    }
    private struct LineSegment
    {
        public Vector3 start;
        public Vector3 end;
        private Vector3 dir;
        private float length;

        public LineSegment(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            dir = (end - start).normalized;
            length = Vector3.Distance(start, end);
        }

        // get the closest point on the segment to a point
        public Vector3 ClosestPoint(Vector3 point)
        {
            float t = Vector3.Dot(point - start, dir);
            t = Mathf.Clamp(t, 0f, length);
            return start + dir * t;
        }
    }



    // a gameobject with a simple monobehaviour to call our DrawGizmos method
    private static GizmosRunner runner;
    private static void EnsureRunner()
    {
        if (runner != null) return;

        GameObject go = new GameObject("RaycastDebugger_GizmosRunner");
        go.hideFlags = HideFlags.HideInInspector;
        runner = go.AddComponent<GizmosRunner>();
        runner.OnDraw = DrawGizmos;
    }

    private class GizmosRunner : MonoBehaviour
    {
        public Action OnDraw;
        public int fixedStep;   // FixedUpdate frame counter

        private void FixedUpdate()
        {
            fixedStep++;
        }

        private void OnDrawGizmos()
        {
            OnDraw?.Invoke();
        }
    }

    // draw gizmos which is automatically called by the runner GameObject
    public static void DrawGizmos()
    {
        // self destruct if shipped as a build 
        if (!Application.isEditor)
        {
            raycastInfos.Clear();
            return;
        }

        for (int i = raycastInfos.Count - 1; i >= 0; i--)
        {
            // draw raycast with color
            RaycastInfo raycastInfo = raycastInfos[i];
            Gizmos.color = raycastInfo.color;
            Gizmos.DrawLine(raycastInfo.position, raycastInfo.position + raycastInfo.direction * raycastInfo.distance);

            if (raycastInfo is BoxCastInfo boxCastInfo)
            {
                Vector3 position = boxCastInfo.position + boxCastInfo.direction * boxCastInfo.distance;
                Quaternion orientation = boxCastInfo.orientation;

                // draw the rotated cube
                // Note that this already translates the draw position 
                DrawGizmosTranslatedRotated(position, orientation, () =>
                {
                    Gizmos.DrawWireCube(Vector3.zero, boxCastInfo.halfExtents * 2f);
                });
            }

            else if (raycastInfo is SphereCastInfo sphereCastInfo)
            {
                Gizmos.DrawWireSphere(sphereCastInfo.position + sphereCastInfo.direction * sphereCastInfo.distance, sphereCastInfo.radius);
            }
            else if (raycastInfo is CapsuleCastInfo capsuleCastInfo)
            {
                // draw a sphere for both points
                Gizmos.DrawWireSphere(capsuleCastInfo.position + capsuleCastInfo.direction * capsuleCastInfo.distance, capsuleCastInfo.radius);
                Gizmos.DrawWireSphere(capsuleCastInfo.point1 + capsuleCastInfo.direction * capsuleCastInfo.distance, capsuleCastInfo.radius);
                // connect the spheres
                Gizmos.DrawLine(capsuleCastInfo.position + capsuleCastInfo.direction * capsuleCastInfo.distance, capsuleCastInfo.point1 + capsuleCastInfo.direction * capsuleCastInfo.distance);
                Gizmos.DrawLine(capsuleCastInfo.point1 + capsuleCastInfo.direction * capsuleCastInfo.distance, capsuleCastInfo.point1);
            }

            // remove raycasts drawn in update
            // or raycasts drawn in fixedUpdate that their fixedUpdate step isn't the last fixedUpdate step
            if (raycastInfo.fixedStep < runner.fixedStep - 1)
                raycastInfos.RemoveAt(i);
        }
    }

    private static void DrawGizmosTranslatedRotated(Vector3 aroundPosition, Quaternion orientation, Action action)
    {
        // Store previous matrix
        Matrix4x4 oldMatrix = Gizmos.matrix;
        // Set new matrix with rotation and translated position
        Gizmos.matrix = Matrix4x4.TRS(aroundPosition, orientation, Vector3.one);
        // do an action with the gizmos rotated around position
        action();
        // return Gizmos to previous matrix
        Gizmos.matrix = oldMatrix;
    }
}