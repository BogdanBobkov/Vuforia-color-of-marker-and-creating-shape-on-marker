using UnityEngine;
using Vuforia;
using Internals;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        private TargetBehaviour _currentTargetController;
        private Texture2D       _texture2D;

        private void Start() => Locator.UiSwitcher.SetLostMarkerMode();

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
                    Locator.UiSwitcher.SetFoundMarkerMode();
                    break;
                case TrackableBehaviour.Status.NO_POSE:
                    _currentTargetController = null;
                    Locator.UiSwitcher.SetLostMarkerMode();
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
    }
}
