using UnityEngine;

namespace EzySlice
{
    public struct Line
    {
        public Line(Vector3 pta, Vector3 ptb)
        {
            positionA = pta;
            positionB = ptb;
        }

        public float dist => Vector3.Distance(positionA, positionB);

        public float distSq => (positionA - positionB).sqrMagnitude;

        public Vector3 positionA { get; }

        public Vector3 positionB { get; }
    }
}