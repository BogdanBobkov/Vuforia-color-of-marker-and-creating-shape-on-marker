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

        public MeshFilter        MeshFilter       => _MeshFilter;
        public FillingTriangle[] FillingTriangles => _FillingTriangles;

        private void Start() => CreateMeshShape();
        public void CreateMeshShape() => Locator.SceneController.CreateMeshShape(this);
    }
}
