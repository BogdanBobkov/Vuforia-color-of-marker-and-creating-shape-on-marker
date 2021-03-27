using UnityEngine;
using Internals;
using Vuforia;

namespace Controllers
{
    public class TargetBehaviour : MonoBehaviour
    {
        [SerializeField] private MeshRenderer        _ModelObject;
        [SerializeField] private RenderTextureCamera _RenderTextureCamera;
        public RenderTextureCamera RenderTextureCamera => _RenderTextureCamera;

        public void OnTargetFound() => Locator.SceneController.OnChangedStatus(this, TrackableBehaviour.Status.TRACKED);
        public void OnTargetLost() => Locator.SceneController.OnChangedStatus(this, TrackableBehaviour.Status.NO_POSE);
        public void SetColor(Color color) => _ModelObject.material.color = color;
    }
}
