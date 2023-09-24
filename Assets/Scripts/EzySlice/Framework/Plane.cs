using UnityEngine;

namespace EzySlice
{
    /**
     * Quick Internal structure which checks where the point lays on the
     * Plane. UP = Upwards from the Normal, DOWN = Downwards from the Normal
     * ON = Point lays straight on the plane
     */
    public enum SideOfPlane
    {
        UP,
        DOWN,
        ON
    }

    /**
     * Represents a simple 3D Plane structure with a position
     * and direction which extends infinitely in its axis. This provides
     * an optimal structure for collision tests for the slicing framework.
     */
    public struct Plane
    {
        // this is for editor debugging only! do NOT try to access this
        // variable at runtime, we will be stripping it out for final
        // builds
#if UNITY_EDITOR
        private Transform trans_ref;
#endif

        public Plane(Vector3 pos, Vector3 norm)
        {
            normal = norm;
            dist = Vector3.Dot(norm, pos);

            // this is for editor debugging only!
#if UNITY_EDITOR
            trans_ref = null;
#endif
        }

        public Plane(Vector3 norm, float dot)
        {
            normal = norm;
            dist = dot;

            // this is for editor debugging only!
#if UNITY_EDITOR
            trans_ref = null;
#endif
        }

        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            dist = -Vector3.Dot(normal, a);

            // this is for editor debugging only!
#if UNITY_EDITOR
            trans_ref = null;
#endif
        }

        public void Compute(Vector3 pos, Vector3 norm)
        {
            normal = norm;
            dist = Vector3.Dot(norm, pos);
        }

        public void Compute(Transform trans)
        {
            Compute(trans.position, trans.up);

            // this is for editor debugging only!
#if UNITY_EDITOR
            trans_ref = trans;
#endif
        }

        public void Compute(GameObject obj)
        {
            Compute(obj.transform);
        }

        public Vector3 normal { get; private set; }

        public float dist { get; private set; }

        /**
         * Checks which side of the plane the point lays on.
         */
        public SideOfPlane SideOf(Vector3 pt)
        {
            var result = Vector3.Dot(normal, pt) - dist;

            if (result > Intersector.Epsilon) return SideOfPlane.UP;

            if (result < -Intersector.Epsilon) return SideOfPlane.DOWN;

            return SideOfPlane.ON;
        }

        /**
         * Editor only DEBUG functionality. This should not be compiled in the final
         * Version.
         */
        public void OnDebugDraw()
        {
            OnDebugDraw(Color.white);
        }

        public void OnDebugDraw(Color drawColor)
        {
            // NOTE -> Gizmos are only supported in the editor. We will keep these function
            // signatures for consistancy however at final build, these will do nothing
            // TO/DO -> Should we throw a runtime exception if this function tried to get executed
            // at runtime?
#if UNITY_EDITOR

            if (trans_ref == null) return;

            var prevColor = Gizmos.color;
            var prevMatrix = Gizmos.matrix;

            // TO-DO
            Gizmos.matrix = Matrix4x4.TRS(trans_ref.position, trans_ref.rotation, trans_ref.localScale);
            Gizmos.color = drawColor;

            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.0f, 0.0f, 1.0f));

            Gizmos.color = prevColor;
            Gizmos.matrix = prevMatrix;

#endif
        }
    }
}