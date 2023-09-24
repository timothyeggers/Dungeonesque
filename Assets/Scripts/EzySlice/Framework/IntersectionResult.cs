using UnityEngine;

namespace EzySlice
{
    /**
     * A Basic Structure which contains intersection information
     * for Plane->Triangle Intersection Tests
     * TO-DO -> This structure can be optimized to hold less data
     * via an optional indices array. Could lead for a faster
     * intersection test aswell.
     */
    public sealed class IntersectionResult
    {
        // our intersection points/triangles

        // general tag to check if this structure is valid

        // our counters. We use raw arrays for performance reasons

        public IntersectionResult()
        {
            isValid = false;

            upperHull = new Triangle[2];
            lowerHull = new Triangle[2];
            intersectionPoints = new Vector3[2];

            upperHullCount = 0;
            lowerHullCount = 0;
            intersectionPointCount = 0;
        }

        public Triangle[] upperHull { get; }

        public Triangle[] lowerHull { get; }

        public Vector3[] intersectionPoints { get; }

        public int upperHullCount { get; private set; }

        public int lowerHullCount { get; private set; }

        public int intersectionPointCount { get; private set; }

        public bool isValid { get; private set; }

        /**
         * Used by the intersector, adds a new triangle to the
         * upper hull section
         */
        public IntersectionResult AddUpperHull(Triangle tri)
        {
            upperHull[upperHullCount++] = tri;

            isValid = true;

            return this;
        }

        /**
         * Used by the intersector, adds a new triangle to the
         * lower gull section
         */
        public IntersectionResult AddLowerHull(Triangle tri)
        {
            lowerHull[lowerHullCount++] = tri;

            isValid = true;

            return this;
        }

        /**
         * Used by the intersector, adds a new intersection point
         * which is shared by both upper->lower hulls
         */
        public void AddIntersectionPoint(Vector3 pt)
        {
            intersectionPoints[intersectionPointCount++] = pt;
        }

        /**
         * Clear the current state of this object
         */
        public void Clear()
        {
            isValid = false;
            upperHullCount = 0;
            lowerHullCount = 0;
            intersectionPointCount = 0;
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
#if UNITY_EDITOR

            if (!isValid) return;

            var prevColor = Gizmos.color;

            Gizmos.color = drawColor;

            // draw the intersection points
            for (var i = 0; i < intersectionPointCount; i++) Gizmos.DrawSphere(intersectionPoints[i], 0.1f);

            // draw the upper hull in RED
            for (var i = 0; i < upperHullCount; i++) upperHull[i].OnDebugDraw(Color.red);

            // draw the lower hull in BLUE
            for (var i = 0; i < lowerHullCount; i++) lowerHull[i].OnDebugDraw(Color.blue);

            Gizmos.color = prevColor;

#endif
        }
    }
}