using Internals;
using UnityEngine;
using System;

namespace Controllers
{
    public class TargetCreatingMesh : TargetBehaviour
    {
        [Serializable]
        public class FillingTriangle
        {
            public Transform[] Points = new Transform[3];
        }

        [SerializeField] private FillingTriangle[] _FillingTriangles;
        [SerializeField] private MeshFilter        _MeshFilter;

        private void Start() => CreateMeshShape();

        private void CreateMeshShape()
        {
            _MeshFilter.mesh.Clear();
            _MeshFilter.mesh = new Mesh();
            CombineInstance[] combine = new CombineInstance[(_FillingTriangles[0].Points.Length + 2) * _FillingTriangles.Length];

            var currentIndexOfCombine = 0;

            /* Down side */
            for (int i = 0; i < _FillingTriangles.Length; i++)
            {
                var triangles = new System.Collections.Generic.List<int> { 0, 1, 2 };
                var vertices = new Vector3[] {
                                            _FillingTriangles[i].Points[0].position,
                                            _FillingTriangles[i].Points[1].position,
                                            _FillingTriangles[i].Points[2].position,
                                          };

                combine[currentIndexOfCombine].mesh = new Mesh();
                combine[currentIndexOfCombine].transform = transform.localToWorldMatrix;
                combine[currentIndexOfCombine].mesh.vertices = vertices;
                combine[currentIndexOfCombine].mesh.triangles = triangles.ToArray();

                currentIndexOfCombine++;
            }

            /* create down surface */
            _MeshFilter.mesh.CombineMeshes(combine);

            var height = (_MeshFilter.mesh.bounds.size.x > _MeshFilter.mesh.bounds.size.z) ? _MeshFilter.mesh.bounds.size.x : _MeshFilter.mesh.bounds.size.z;
            height /= 2f;

            for (int i = 0; i < _FillingTriangles.Length; i++)
            {
                /* lateral sides */
                for (int j = 0; j < _FillingTriangles[i].Points.Length; j++)
                {
                    var triangles1 = new System.Collections.Generic.List<int>
                                                                    {
                                                                        0, 1, 2,
                                                                        2, 1, 3,
                                                                    };
                    var vertices1 = new Vector3[] {
                                            _FillingTriangles[i].Points[j].position,
                                            _FillingTriangles[i].Points[(j+1) % _FillingTriangles[i].Points.Length].position,
                                            _FillingTriangles[i].Points[j].position + new Vector3(0, height, 0),
                                            _FillingTriangles[i].Points[(j+1) % _FillingTriangles[i].Points.Length].position + new Vector3(0, height, 0),
                                          };

                    combine[currentIndexOfCombine + j].mesh = new Mesh();
                    combine[currentIndexOfCombine + j].transform = transform.localToWorldMatrix;
                    combine[currentIndexOfCombine + j].mesh.vertices = vertices1;
                    combine[currentIndexOfCombine + j].mesh.triangles = triangles1.ToArray();
                }

                /* Top side */
                var triangles2 = new System.Collections.Generic.List<int> { 0, 1, 2 };
                var vertices2 = new Vector3[] {
                                            _FillingTriangles[i].Points[0].position + new Vector3(0, height, 0),
                                            _FillingTriangles[i].Points[1].position + new Vector3(0, height, 0),
                                            _FillingTriangles[i].Points[2].position + new Vector3(0, height, 0),
                                          };

                combine[currentIndexOfCombine + _FillingTriangles[i].Points.Length].mesh = new Mesh();
                combine[currentIndexOfCombine + _FillingTriangles[i].Points.Length].transform = transform.localToWorldMatrix;
                combine[currentIndexOfCombine + _FillingTriangles[i].Points.Length].mesh.vertices = vertices2;
                combine[currentIndexOfCombine + _FillingTriangles[i].Points.Length].mesh.triangles = triangles2.ToArray();

                currentIndexOfCombine += 4;
            }

            /* create top and lateral sides */
            _MeshFilter.mesh.CombineMeshes(combine);
        }
    }
}
