using UnityEngine;
using Vuforia;
using Internals;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        private TargetBehaviour _currentTargetController;
        private Texture2D       _texture2D;

        private void Start() => Locator.UiSwitcher.UiErrorMessage.Show();

        private void Update()
        {
            if (_currentTargetController != null)
            {
                _currentTargetController.SetColor(GetPixelTexture());
            }
        }

        public void OnChangedStatus(TargetBehaviour targetController, TrackableBehaviour.Status newStatus)
        {
            switch (newStatus)
            {
                case TrackableBehaviour.Status.TRACKED:
                    _currentTargetController = targetController;
                    Locator.UiSwitcher.UiErrorMessage.Hide();
                    break;
                case TrackableBehaviour.Status.NO_POSE:
                    _currentTargetController = null;
                    Locator.UiSwitcher.UiErrorMessage.Show();
                    break;
            }
        }

        private Color GetPixelTexture()
        {
            if (_currentTargetController.RenderTextureCamera != null)
            {
                var renderTexture = _currentTargetController.RenderTextureCamera.GetRenderTexture();
                _texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);

                RenderTexture.active = renderTexture;
                _texture2D.ReadPixels(new Rect(0, 0, 1, 1), 0, 0);
                _texture2D.Apply();

                return _texture2D.GetPixel(0, 0);
            }

            return Color.green;
        }

        public void CreateMeshShape(TargetCreatingMesh target)
        {
            target.MeshFilter.mesh.Clear();
            target.MeshFilter.mesh = new Mesh();
            CombineInstance[] combine = new CombineInstance[(target.FillingTriangles[0].Points.Length + 2) * target.FillingTriangles.Length];

            var currentIndexOfCombine = 0;

            /* Down side */
            for (int i = 0; i < target.FillingTriangles.Length; i++)
            {
                var triangles = new System.Collections.Generic.List<int> { 0, 1, 2 };
                var vertices = new Vector3[] {
                                            target.FillingTriangles[i].Points[0].position,
                                            target.FillingTriangles[i].Points[1].position,
                                            target.FillingTriangles[i].Points[2].position,
                                          };

                combine[currentIndexOfCombine].mesh = new Mesh();
                combine[currentIndexOfCombine].transform = transform.localToWorldMatrix;
                combine[currentIndexOfCombine].mesh.vertices = vertices;
                combine[currentIndexOfCombine].mesh.triangles = triangles.ToArray();

                currentIndexOfCombine++;
            }

            /* create down surface */
            target.MeshFilter.mesh.CombineMeshes(combine);

            var height = (target.MeshFilter.mesh.bounds.size.x > target.MeshFilter.mesh.bounds.size.z) ? target.MeshFilter.mesh.bounds.size.x : target.MeshFilter.mesh.bounds.size.z;
            height /= 2f;

            for (int i = 0; i < target.FillingTriangles.Length; i++)
            {
                /* lateral sides */
                for (int j = 0; j < target.FillingTriangles[i].Points.Length; j++)
                {
                    var triangles1 = new System.Collections.Generic.List<int>
                                                                    {
                                                                        0, 1, 2,
                                                                        2, 1, 3,
                                                                    };
                    var vertices1 = new Vector3[] {
                                            target.FillingTriangles[i].Points[j].position,
                                            target.FillingTriangles[i].Points[(j+1) % target.FillingTriangles[i].Points.Length].position,
                                            target.FillingTriangles[i].Points[j].position + new Vector3(0, height, 0),
                                            target.FillingTriangles[i].Points[(j+1) % target.FillingTriangles[i].Points.Length].position + new Vector3(0, height, 0),
                                          };

                    combine[currentIndexOfCombine + j].mesh = new Mesh();
                    combine[currentIndexOfCombine + j].transform = transform.localToWorldMatrix;
                    combine[currentIndexOfCombine + j].mesh.vertices = vertices1;
                    combine[currentIndexOfCombine + j].mesh.triangles = triangles1.ToArray();
                }

                /* Top side */
                var triangles2 = new System.Collections.Generic.List<int> { 0, 1, 2 };
                var vertices2 = new Vector3[] {
                                            target.FillingTriangles[i].Points[0].position + new Vector3(0, height, 0),
                                            target.FillingTriangles[i].Points[1].position + new Vector3(0, height, 0),
                                            target.FillingTriangles[i].Points[2].position + new Vector3(0, height, 0),
                                          };

                combine[currentIndexOfCombine + target.FillingTriangles[i].Points.Length].mesh = new Mesh();
                combine[currentIndexOfCombine + target.FillingTriangles[i].Points.Length].transform = transform.localToWorldMatrix;
                combine[currentIndexOfCombine + target.FillingTriangles[i].Points.Length].mesh.vertices = vertices2;
                combine[currentIndexOfCombine + target.FillingTriangles[i].Points.Length].mesh.triangles = triangles2.ToArray();

                currentIndexOfCombine += 4;
            }

            /* create top and lateral sides */
            target.MeshFilter.mesh.CombineMeshes(combine);
        }

    }
}
