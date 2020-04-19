using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using netsis.jp.Hexagon;
using UnityEngine;
using UnityEngine.Serialization;
using Rect = UnityEngine.Rect;

namespace jp.netsis.LowPolyWater
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LowPolyWater : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 20f)]
        private float hexSize = 1f;
        [SerializeField, Range(0, 20)]
        private int hexRange = 5;
        [SerializeField]
        private bool isSetMaxRange = false;
        [SerializeField]
        private int maxRange = 0;
        [SerializeField]
        private HexagonType showHexagonType;

        [SerializeField]
        private Material lowPolyWaterMaterial = default;
    
        private MeshFilter hexMeshFilter;
        private MeshRenderer hexMeshRenderer;
        private Mesh hexMesh;
        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();
        private List<Vector2> uvs = new List<Vector2>();

        void Awake ()
        {
            hexMeshFilter = GetComponent<MeshFilter>();
            hexMeshRenderer = GetComponent<MeshRenderer>();
            hexMeshFilter.mesh = hexMesh = new Mesh();
            hexMeshRenderer.sharedMaterial = lowPolyWaterMaterial;
        }

        private void Start()
        {
            Create();
        }

        void OnValidate()
        {
            Create();
        }

        bool IsCanAddHexagon(in int maxRange, in int z)
        {
            if (Mathf.Abs(z)<=maxRange)
            {
                return true;
            }
            return false;
        }

        void Create()
        {
            var pointList = new List<HexagonAxialCoordinates>();

            var xMin = -hexRange;
            var xMax = hexRange;
            int yMin;
            int yMax;
            for (int x = xMin; x <= xMax; x++) {
                yMin = Mathf.Max(-hexRange,-x-hexRange);
                yMax = Mathf.Min(hexRange,-x+hexRange);
                for (int y = yMin; y <= yMax; y++)
                {
                    var z = -x - y;
                    if (isSetMaxRange && !IsCanAddHexagon(maxRange, z))
                    {
                        continue;
                    }
                    pointList.Add(new HexagonAxialCoordinates(x,z));
                }
            }

            var rect = HexagonConst.CreateHexagonRect(showHexagonType,hexRange,hexSize);
            Triangulate(showHexagonType,rect,hexSize,pointList.ToArray());
        }

        void Triangulate (HexagonType hexagonType, in Rect inHexagonRect, in float hexSize, in HexagonAxialCoordinates[] pointList) {
            hexMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            for (int i = 0; i < pointList.Length; i++) {
                Triangulate(hexagonType, inHexagonRect, hexSize, pointList[i]);
            }
            hexMesh.vertices = vertices.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.uv = uvs.ToArray();
            hexMesh.RecalculateNormals();
        }
        
        void Triangulate (HexagonType hexagonType, in Rect inHexagonRect, in float hexSize, in HexagonAxialCoordinates point) {
            Vector3 center = point.ToVector3(hexagonType) * hexSize;
            var leftBottom = new Vector2(inHexagonRect.x, inHexagonRect.y);
            var wh = new Vector2(inHexagonRect.width, inHexagonRect.height);
            var uvCenter = (new Vector2(center.x,center.y) - leftBottom) / wh;
            for (int i = 0; i < HexagonConst.HexTriangles; i++)
            {
                var corner1 = HexagonConst.GetCorner(hexagonType, i);
                var corner2 = HexagonConst.GetCorner(hexagonType, i+1);

                var p1 = center + corner1 * hexSize;
                var p2 = center + corner2 * hexSize;
                AddTriangle(
                    center,
                    p1,
                    p2
                );
                var uv1 = (new Vector2(p1.x,p1.z) - leftBottom) / wh;
                var uv2 = (new Vector2(p2.x,p2.z) - leftBottom) / wh;
                
                uvs.Add(uvCenter);
                uvs.Add(uv1);
                uvs.Add(uv2);
            }
        }
        
        void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }


    }
}
