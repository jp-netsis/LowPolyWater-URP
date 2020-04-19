using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace netsis.jp.Hexagon
{
    [Serializable]
    public struct HexagonAxialCoordinates
    {
        public static HexagonAxialCoordinates FromOffset(int q, int r)
        {
            return new HexagonAxialCoordinates(q, r);
        }

        public int Q { get; private set; }
        public int R { get; private set; }
        public int S { get; private set; }

        public HexagonAxialCoordinates(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        private Vector3 ToVector3Pointy()
        {
            // pointy_hex_to_pixel
            return new Vector3(
                HexagonConst.outerRadius * (HexagonConst.Sqrt3 * Q + HexagonConst.Sqrt3 * 0.5f * R),
                0f,
                HexagonConst.outerRadius * (3f * 0.5f * R) );
        }

        public Vector3 ToVector3(HexagonType hexagonType)
        {
            if (hexagonType == HexagonType.PointyTopped) return ToVector3Pointy();
            var vec = ToVector3Pointy();
            return new Vector3(vec.z,vec.y,vec.x);
        }

        public override string ToString()
        {
            return $"Q:{Q} R:{R} S:{S}";
        }
    }
}
