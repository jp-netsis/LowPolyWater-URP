using UnityEngine;

namespace netsis.jp.Hexagon
{
    public enum HexagonType
    {
        PointyTopped,
        FlatTopped,
    }
    public static class HexagonConst
    {
        public const int HexTriangles = 6;
        public static readonly float Sqrt3 = Mathf.Sqrt(3f); // sin( 60 degree )
        public const float hexagonDiameter  = 1f;
        public const float outerRadius = hexagonDiameter * 0.5f;
        public static readonly float innerRadius = outerRadius * Sqrt3 * 0.5f;
        public static Vector3[] corners = {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };

        public static Vector3 GetCorner(HexagonType hexagonType, int index)
        {
            if (hexagonType == HexagonType.PointyTopped) return corners[index];
            return new Vector3(corners[index].z,corners[index].y,-corners[index].x);
        }
        
        public static Rect CreateHexagonRect(HexagonType hexagonType, in int range, in float hexSize)
        {
            var width = hexSize * (hexagonDiameter + range * 2) * Sqrt3 * 0.5f;
            var hight = hexSize * (hexagonDiameter + range * 3 * 0.5f);
            if (hexagonType == HexagonType.FlatTopped)
            {
                var bkup = width;
                width = hight;
                hight = bkup;
            }
            return new Rect(-(width/2),-(hight/2),width,hight);
        }
        

    }
}
